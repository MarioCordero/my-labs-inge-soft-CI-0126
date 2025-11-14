<template>
  <div class="container mt-5">
    <h1 class="display-4 text-center">Lista de países</h1>
    <div class="row justify-content-end mb-3">
      <div class="col-2">
        <router-link to="/country">
          <button type="button" class="btn btn-outline-secondary w-100">
            Agregar país
          </button>
        </router-link>
      </div>
    </div>

    <table class="table table-bordered table-striped table-hover">
      <thead>
        <tr>
          <th>Nombre</th>
          <th>Continente</th>
          <th>Idioma</th>
          <th>Acciones</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(country, index) of countries" :key="country.id ?? country.Id ?? index">
          <td>{{ country.Name ?? country.name }}</td>
          <td>{{ country.Continent ?? country.continent }}</td>
          <td>{{ country.Language ?? country.language }}</td>
          <td>
            <button @click="goEdit(country)" class="btn btn-warning btn-sm">Editar</button>
            <button @click="deleteCountry(index)" class="btn btn-danger btn-sm ms-1">Eliminar</button>
          </td>
        </tr>
      </tbody>
    </table>

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
import { getCountries as apiGetCountries, deleteCountry as apiDeleteCountry } from '../config/api';
import SuccessModal from './SuccessModal.vue';
import ErrorModal from './ErrorModal.vue';

export default {
  name: "CountriesList",
  components: { SuccessModal, ErrorModal },
  data() {
    return {
      countries: [],
      showSuccess: false,
      successMessage: '',
      showError: false,
      errorMessage: ''
    };
  },
  methods: {
    goEdit(country) {
      const id = country.id ?? country.Id;
      if (id) this.$router.push({ path: '/country', query: { id }});
    },
    deleteCountry(index) {
      const country = this.countries[index];
      const id = country.id ?? country.Id;
      if (!id) {
        this.errorMessage = "ID del país no encontrado.";
        this.showError = true;
        return;
      }
      apiDeleteCountry(id)
        .then(() => {
          this.successMessage = "País eliminado correctamente.";
          this.showSuccess = true;
          this.getCountries();
        })
        .catch((error) => {
          this.errorMessage = error?.response?.data || "Error al eliminar el país.";
          this.showError = true;
          console.error(error);
        });
    },
    getCountries() {
      apiGetCountries()
        .then((response) => {
          this.countries = response.data;
        })
        .catch((error) => {
          this.errorMessage = "Error obteniendo países.";
          this.showError = true;
          console.error(error);
        });
    }
  },
  created() {
    this.getCountries();
  }
};
</script>