#!/usr/bin/env bash
set -euo pipefail

BASEDIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
FRONT="$BASEDIR/frontend-lab"
BACK="$BASEDIR/backend-lab"
LOGDIR="$BASEDIR/logs"
mkdir -p "$LOGDIR"

# Si hay gnome-terminal, abrir cada servicio en su propia ventana
if command -v gnome-terminal >/dev/null 2>&1; then
  gnome-terminal -- bash -c "cd '$FRONT' && if [ ! -d node_modules ]; then npm install --no-audit --no-fund; fi && npm run serve; exec bash"
  gnome-terminal -- bash -c "cd '$BACK' && dotnet run; exec bash"
  exit 0
fi

# Fallback simple: arrancar en background y guardar logs/pids
nohup bash -c "cd '$FRONT' && if [ ! -d node_modules ]; then npm install --no-audit --no-fund; fi && npm run serve" > "$LOGDIR/frontend.log" 2>&1 &
echo $! > "$BASEDIR/.frontend.pid"

nohup bash -c "cd '$BACK' && dotnet run" > "$LOGDIR/backend.log" 2>&1 &
echo $! > "$BASEDIR/.backend.pid"

echo "Frontend and backend started in background. Logs: $LOGDIR/*.log"