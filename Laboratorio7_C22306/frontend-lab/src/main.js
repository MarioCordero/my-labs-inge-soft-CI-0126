import { createApp } from 'vue'
import App from './App.vue'
import { createRouter, createWebHistory } from 'vue-router'
import CountriesList from './components/CountriesList.vue'
import CountryForm from './components/CountryForm.vue'
import HelloWorld from './components/HelloWorld.vue'

const routes = [
  { path: '/', component: CountriesList },
  { path: '/country', component: CountryForm },
  { path: '/hello', component: HelloWorld, props: { msg: 'Hello World desde ruta /hello' } }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

createApp(App).use(router).mount('#app')