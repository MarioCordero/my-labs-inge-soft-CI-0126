<template>
  <div class="d-flex justify-content-center align-items-start mt-5">
    <div class="card p-4 shadow" style="max-width: 640px; width: 100%;">
      <h3 class="text-center mb-4">Crear país</h3>

      <form @submit.prevent="saveCountry" novalidate>
        <div class="mb-3">
          <label for="name" class="form-label">Nombre</label>
          <input id="name" v-model="formData.Name" class="form-control" required />
        </div>

        <div class="mb-3">
          <label for="continent" class="form-label">Continente</label>
          <select id="continent" v-model="formData.Continent" class="form-control" required>
            <option value="" disabled>Seleccione un continente</option>
            <option>África</option>
            <option>Asia</option>
            <option>Europa</option>
            <option>América</option>
            <option>Oceania</option>
            <option>Antártida</option>
          </select>
        </div>

        <div class="mb-4">
          <label for="language" class="form-label">Idioma</label>
          <input id="language" v-model="formData.Language" class="form-control" required />
        </div>

        <div class="d-flex gap-2">
          <button type="submit" class="btn btn-success">Guardar</button>
          <button type="button" class="btn btn-secondary" @click="cancel">Cancelar</button>
        </div>
      </form>
    </div>

    <SuccessModal
      :visible="showSuccess"
      :message="successMessage"
      @close="showSuccess = false"
    />
    <ErrorModal
      :visible="showError"
      :message="errorMessage"
      @close="showError = false"
    />
  </div>
</template>

<script>
import { createCountry } from '../config/api';
import SuccessModal from './SuccessModal.vue';
import ErrorModal from './ErrorModal.vue';

export default {
  name: 'CountryForm',
  components: { SuccessModal, ErrorModal },
  data() {
    return {
      formData: { Name: '', Continent: '', Language: '' },
      showSuccess: false,
      successMessage: '',
      showError: false,
      errorMessage: ''
    };
  },
  methods: {
    saveCountry() {
      if (!this.formData.Name || !this.formData.Continent || !this.formData.Language) {
        this.errorMessage = 'Complete todos los campos.';
        this.showError = true;
        return;
      }

      createCountry(this.formData)
        .then(() => {
          this.successMessage = 'País creado correctamente.';
          this.showSuccess = true;
          setTimeout(() => {
            this.showSuccess = false;
            this.$router.push('/');
          }, 1200);
        })
        .catch((err) => {
          this.errorMessage = err?.response?.data || 'Error al crear el país.';
          this.showError = true;
          console.error(err);
        });
    },
    cancel() {
      this.$router.back();
    }
  }
};
</script>