/**
 * main.js
 *
 * Bootstraps Vuetify and other plugins then mounts the App`
 */

import { createApp } from 'vue'
import Toast from 'vue-toastification'
// Plugins
import { registerPlugins } from '@/plugins'

// Components
import App from './App.vue'

// Styles
import 'unfonts.css'

// Composables
import 'vue-toastification/dist/index.css'
import 'vuetify/styles'
const app = createApp(App)

app.use(Toast, {
  // opções (opcional)
  position: 'bottom-right',
  timeout: 5000,
  closeOnClick: true,
  pauseOnHover: true,
})

registerPlugins(app)

app.mount('#app')
