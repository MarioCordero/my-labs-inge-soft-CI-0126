<template>
  <div class="container mt-5">
    <h1 class="display-4 text-center">Lista de países</h1>
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
        <tr v-for="(country, index) in countries" :key="index">
          <td>{{ country.name }}</td>
          <td>{{ country.continent }}</td>
          <td>{{ country.language }}</td>
          <td>
            <button class="btn btn-secondary btn-sm me-1">Editar</button>
            <!-- Eliminar con v-on:click -->
            <button 
              class="btn btn-danger btn-sm" 
              @click="deleteCountry(index)"
            >
              Eliminar
            </button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
export default {
  name: "CountriesList",
  data() {
    return {
      countries: [
        { name: "Costa Rica", continent: "América", language: "Español" },
        { name: "Japón", continent: "Asia", language: "Japonés" },
        { name: "Corea del Sur", continent: "Asia", language: "Coreano" },
        { name: "Italia", continent: "Europa", language: "Italiano" },
        { name: "Alemania", continent: "Europa", language: "Alemán" },
      ],
    };
  },
  methods: {
    deleteCountry(index) {
      this.countries.splice(index, 1);
    }
  }
};
</script>

<style scoped>
.table {
  margin-top: 20px;
}
</style>