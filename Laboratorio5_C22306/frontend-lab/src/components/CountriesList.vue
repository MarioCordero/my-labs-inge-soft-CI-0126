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
        <tr v-for="(country, index) of countries" :key="index">
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
import axios from "axios";

export default {
  name: "CountriesList",
  data() {
    return {
      countries: []
    };
  },
  methods: {
    deleteCountry(index) {
      this.countries.splice(index, 1);
    },
    getCountries() {
      axios.get("https://localhost:7019/api/country")
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

<style>
/* Estilos opcionales */
</style>