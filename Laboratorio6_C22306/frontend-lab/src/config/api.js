import axios from 'axios';

/**
 * Base URL de la API.
 * Puedes sobrescribirlo con la variable de entorno VUE_APP_API_BASE
 * por ejemplo: VUE_APP_API_BASE=http://localhost:5011/api npm run serve
 */
export const API_BASE = process.env.VUE_APP_API_BASE || 'http://localhost:5011/api';

const api = axios.create({
  baseURL: API_BASE,
  // timeout: 10000, // descomenta si quieres timeout por defecto
});

export default api;

// Helpers opcionales para usar en componentes
export const getCountries = () => api.get('/country');
export const getCountry = (id) => api.get(`/country/${id}`);
export const createCountry = (payload) => api.post('/country', payload);
export const updateCountry = (id, payload) => api.put(`/country/${id}`, payload);
export const deleteCountry = (id) => api.delete(`/country/${id}`);