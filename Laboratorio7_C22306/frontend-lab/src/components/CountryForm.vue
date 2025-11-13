<template>
  <div class="d-flex justify-content-center align-items-center vh-100">
    <div class="card p-4 shadow" style="max-width: 400px; width: 100%">
      <h3 class="text-center mb-4">Formulario de creación de países</h3>
      <form @submit.prevent="saveCountry">
        <div class="form-group mb-3">
          <label for="name" class="form-label">Nombre:</label>
          <input
            v-model="formData.Name"
            type="text"
            id="name"
            class="form-control"
            required
          />
        </div>
        <div class="form-group mb-3">
          <label for="continent" class="form-label">Continente:</label>
          <select
            v-model="formData.Continent"
            id="continent"
            required
            class="form-control"
          >
            <option value="" disabled selected>Seleccione un continente</option>
            <option value="África">África</option>
            <option value="Asia">Asia</option>
            <option value="Europa">Europa</option>
            <option value="América">América</option>
            <option value="Oceania">Oceania</option>
            <option value="Antártida">Antártida</option>
          </select>
        </div>
        <div class="form-group mb-4">
          <label for="language" class="form-label">Idioma:</label>
          <input
            v-model="formData.Language"
            type="text"
            id="language"
            class="form-control"
            required
          />
        </div>
        <div>
          <button type="submit" class="btn btn-success w-100">
            Guardar
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
import { createCountry } from '../config/api';

export default {
  name: "CountryForm",
  data() {
    return {
      formData: {
        Name: "",
        Continent: "",
        Language: ""
      }
    };
  },
  methods: {
    saveCountry() {
      console.log("Datos a guardar:", this.formData);
      
      createCountry(this.formData)
        .then((response) => {
          console.log("Respuesta del servidor:", response);
          this.$router.push("/");
        })
        .catch((error) => {
          console.error("Error al guardar:", error);
          alert("Error al guardar el país. Por favor, intente nuevamente.");
        });
    }
  }
};
</script>