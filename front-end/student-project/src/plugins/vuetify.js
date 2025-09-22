/**
 * plugins/vuetify.js
 *
 * Framework documentation: https://vuetifyjs.com`
 */

// Styles
import '@mdi/font/css/materialdesignicons.css'
import 'vuetify/lib/styles/main.css'

// Composables
import { createVuetify } from 'vuetify'

const vuetify = createVuetify({
  theme: {
    defaultTheme: 'custom',
    themes: {
      custom: {
        dark: false,
        colors: {
          'on-surface': '#000000ff',
          'on-surface-variant': '#000000ff',
          primary: '#fdfdfdff',
          secondary: '#000000ff',
        } } } } })

export default vuetify
