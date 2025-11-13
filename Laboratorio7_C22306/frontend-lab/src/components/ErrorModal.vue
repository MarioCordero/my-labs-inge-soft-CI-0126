<template>
  <div v-if="visible" class="modal-backdrop">
    <div class="modal-card">
      <h4 class="mb-2 text-danger">{{ title }}</h4>
      <p class="mb-3">{{ message }}</p>
      <div class="text-end">
        <button class="btn btn-secondary" @click="close">Cerrar</button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "ErrorModal",
  props: {
    visible: { type: Boolean, default: false },
    message: { type: String, default: "" },
    title: { type: String, default: "Error" }
  },
  emits: ["close"],
  methods: {
    close() {
      this.$emit("close");
    }
  }
};
</script>

<style scoped>
.modal-backdrop {
  position: fixed;
  inset: 0;
  background: rgba(0,0,0,0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1050;
}
.modal-card {
  background: #fff;
  padding: 1.25rem;
  border-radius: .375rem;
  width: 90%;
  max-width: 420px;
  box-shadow: 0 2px 10px rgba(0,0,0,.2);
}
.text-danger { color: #dc3545; }
</style>