<template>
  <div class="container mt-5">
    <h1 class="display-4 text-center">Lista de países</h1>
    <div class="row justify-content-end">
      <div class="col-2">
        <router-link to="/country">
          <button type="button" class="btn btn-outline-secondary float-right">
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
      <tr v-for="(country, index) of countries" :key="country.Id">
        <td>{{ country.name }}</td>
        <td>{{ country.continent }}</td>
        <td>{{ country.language }}</td>
        <td>
          <button class="btn btn-warning btn-sm">Editar</button>
          <button @click="deleteCountry(index)" class="btn btn-danger btn-sm ms-1">Eliminar</button>
        </td>
      </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import { getCountries as apiGetCountries, deleteCountry as apiDeleteCountry } from '../config/api';

export default {
  name: "CountriesList",
  data() {
    return {
      countries: []
    };
  },
  methods: {
    deleteCountry(index) {
      const country = this.countries[index];
      apiDeleteCountry(country.id)
        .then(() => {
          this.getCountries();
        })
        .catch((error) => {
          alert("Error al eliminar el país.");
          console.error(error);
        });
    },
    getCountries() {
      apiGetCountries()
        .then((response) => {
          this.countries = response.data;
        })
        .catch((error) => {
          console.error("Error obteniendo países:", error);
        });
    }
  },
  created() {
    this.getCountries();
  }
};
</script>