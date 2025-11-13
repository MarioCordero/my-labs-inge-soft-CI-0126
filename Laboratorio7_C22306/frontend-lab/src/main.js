import { createApp } from 'vue'
import App from './App.vue'
import { createRouter, createWebHistory } from 'vue-router'
import CountriesList from './components/CountriesList.vue'
import CountryForm from './components/CountryForm.vue'
import HelloWorld from './components/HelloWorld.vue'

const routes = [
  { path: '/', component: CountriesList, meta: { title: 'Lista de países' } },
  { path: '/country', component: CountryForm, meta: { title: 'Crear / Editar país' } },
  { path: '/hello', component: HelloWorld, props: { msg: 'Hello World desde ruta /hello' }, meta: { title: 'Saludo' } }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.afterEach((to) => {
  document.title = to.meta && to.meta.title ? to.meta.title : 'Mi App Vue'
})

createApp(App).use(router).mount('#app')