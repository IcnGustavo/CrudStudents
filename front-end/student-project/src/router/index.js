/**
 * router/index.ts
 *
 * Automatic routes for `./src/pages/*.vue`
 */

// Composables
import { createRouter, createWebHistory } from 'vue-router'
import { routes as autoRoutes } from 'vue-router/auto-routes'
import ViewLayout from '@/layouts/view.vue'
import Home from '@/pages/Home.vue'
import StudentRegister from '@/pages/StudentRegister.vue'

const routes = [
  {
    path: '/',
    component: ViewLayout,
    children: [
      ...autoRoutes,
      {
        path: '',
        name: 'Home',
        component: Home, // seu componente Home.vue
      },
      {
        path: 'student-register/:ra?',
        name: 'StudentRegister',
        component: StudentRegister,
      }] },
  {
    path: '/:catchAll(.*)', // rota para 404
    redirect: '/' }]

const router = createRouter({
  history: createWebHistory(),
  routes })

// Workaround for https://github.com/vitejs/vite/issues/11804
router.onError((err, to) => {
  if (err?.message?.includes?.('Failed to fetch dynamically imported module')) {
    if (localStorage.getItem('vuetify:dynamic-reload')) {
      console.error('Dynamic import error, reloading page did not fix it', err)
    } else {
      console.log('Reloading page to fix dynamic import error')
      localStorage.setItem('vuetify:dynamic-reload', 'true')
      location.assign(to.fullPath)
    }
  } else {
    console.error(err)
  }
})

router.isReady().then(() => {
  localStorage.removeItem('vuetify:dynamic-reload')
})

export default router
