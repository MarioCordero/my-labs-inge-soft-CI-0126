#!/usr/bin/env bash
set -euo pipefail

BASEDIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
FRONT="$BASEDIR/frontend-lab"
BACK="$BASEDIR/backend-lab"
-LOGDIR="$BASEDIR/logs"
-ARTIFACTS_DIR="$BASEDIR/UIAutomationTests/artifacts"
-mkdir -p "$LOGDIR" "$ARTIFACTS_DIR"
+DOCS_DIR="$BASEDIR/Docs"
+LOGDIR="$DOCS_DIR/logs"
+ARTIFACTS_DIR="$DOCS_DIR/artifacts"
+mkdir -p "$LOGDIR" "$ARTIFACTS_DIR" "$DOCS_DIR"

# timeouts
START_TIMEOUT=60   # segundos para que cada servicio responda
SLEEP_INTERVAL=1

cleanup() {
  echo "Cleaning up..."
  if [ -n "${FRONT_PID:-}" ] && kill -0 "$FRONT_PID" 2>/dev/null; then
    echo "Killing frontend PID $FRONT_PID"
    kill "$FRONT_PID" || true
  fi
  if [ -n "${BACK_PID:-}" ] && kill -0 "$BACK_PID" 2>/dev/null; then
    echo "Killing backend PID $BACK_PID"
    kill "$BACK_PID" || true
  fi
}
trap cleanup EXIT

# start frontend in background
echo "Starting frontend..."
nohup bash -c "cd '$FRONT' && if [ ! -d node_modules ]; then npm install --no-audit --no-fund; fi && npm run serve" > "$LOGDIR/frontend.log" 2>&1 &
FRONT_PID=$!
echo $FRONT_PID > "$BASEDIR/.frontend.pid"
echo "Frontend PID: $FRONT_PID (logs: $LOGDIR/frontend.log)"

# start backend in background
echo "Starting backend..."
nohup bash -c "cd '$BACK' && dotnet run" > "$LOGDIR/backend.log" 2>&1 &
BACK_PID=$!
echo $BACK_PID > "$BASEDIR/.backend.pid"
echo "Backend PID: $BACK_PID (logs: $LOGDIR/backend.log)"

# If gnome-terminal exists, open tails to logs for visibility
if command -v gnome-terminal >/dev/null 2>&1; then
  gnome-terminal -- bash -c "echo 'Frontend logs (tail)'; tail -n +1 -f '$LOGDIR/frontend.log'" >/dev/null 2>&1 &
  gnome-terminal -- bash -c "echo 'Backend logs (tail)'; tail -n +1 -f '$LOGDIR/backend.log'" >/dev/null 2>&1 &
fi

# helper to wait for HTTP
wait_for_http() {
  local url=$1
  local timeout=${2:-$START_TIMEOUT}
  local elapsed=0
  while [ $elapsed -lt $timeout ]; do
    if curl -sS --head --fail "$url" >/dev/null 2>&1; then
      echo "OK: $url"
      return 0
    fi
    sleep $SLEEP_INTERVAL
    elapsed=$((elapsed + SLEEP_INTERVAL))
  done
  return 1
}

# wait for frontend (default localhost:8080)
echo "Waiting for frontend (http://localhost:8080/) ..."
if ! wait_for_http "http://localhost:8080/" "$START_TIMEOUT"; then
  echo "Frontend did not respond within ${START_TIMEOUT}s. Last 200 lines of frontend log:"
  tail -n 200 "$LOGDIR/frontend.log" || true
  exit 1
fi

# wait for backend - try common ports and also check backend.log for "Now listening on"
echo "Waiting for backend (ports 5000/5001) or log readiness..."
BACK_READY=1
elapsed=0
while [ $elapsed -lt $START_TIMEOUT ]; do
  if wait_for_http "http://localhost:5000" 1 || wait_for_http "http://localhost:5001" 1 || wait_for_http "http://localhost:80" 1; then
    BACK_READY=0
    break
  fi
  if grep -q -m1 "Now listening on" "$LOGDIR/backend.log" 2>/dev/null; then
    BACK_READY=0
    break
  fi
  sleep $SLEEP_INTERVAL
  elapsed=$((elapsed + SLEEP_INTERVAL))
done

if [ $BACK_READY -ne 0 ]; then
  echo "Backend did not become ready within ${START_TIMEOUT}s. Last 200 lines of backend log:"
  tail -n 200 "$LOGDIR/backend.log" || true
  exit 1
fi
echo "Backend ready."

# run tests if tests project exists
TEST_DIR="$BASEDIR/UIAutomationTests"
if [ -d "$TEST_DIR" ]; then
  echo "Running UI tests in $TEST_DIR ..."
  pushd "$TEST_DIR" >/dev/null
  # ensure artifacts folder exists inside test working dir
  mkdir -p "artifacts/screenshots" "artifacts/page-source"
  dotnet test --logger "console;verbosity=detailed"
  TEST_EXIT_CODE=$?
  popd >/dev/null
else
  echo "No UIAutomationTests directory found at $TEST_DIR. Skipping tests."
  TEST_EXIT_CODE=0
fi

# exit with test exit code (cleanup trap will run)
exit $TEST_EXIT_CODE