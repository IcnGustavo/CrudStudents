<template>
  <v-app id="inspire">
    <v-navigation-drawer
      v-model="drawer"
      class="custom-drawer"
      color="#fcfcfc"
    >
      <v-list density="compact">
        <v-list-subheader class="custom-subheader">
          Módulo Acadêmico
        </v-list-subheader>

        <v-divider class="custom-divider"></v-divider>
        <router-link
          v-for="(item, i) in items"
          :key="i"
          :to="item.to"
        >
          <v-list-item class="custom-list-item">
            <template v-slot:prepend>
              <v-icon :icon="item.icon"></v-icon>
            </template>
            <v-list-item-title :textContent="item.text"></v-list-item-title>
          </v-list-item>
        </router-link>
      </v-list>
    </v-navigation-drawer>

    <v-app-bar
      class="custom-app-bar"
      color="#fcfcfc"
    >
      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>

      <img alt="Logo" src="@/assets/logoA.png" style="height:45px; width:60px; margin-right:0px;" />

      <v-app-bar-title>+A Educação</v-app-bar-title>
    </v-app-bar>

    <v-main>
      <router-view></router-view>
    </v-main>
  </v-app>
</template>

<script setup>
  import { ref } from 'vue'

  const drawer = ref(null)
  const items = [
    { text: 'Consulta de alunos', icon: 'mdi-home', to: '/Home' },
    { text: 'Cadastro de alunos', icon: 'mdi-folder', to: '/StudentRegister' },
    { text: 'Sobre', icon: 'mdi-information', to: '/About' },
  ]
</script>

<script>
  export default {
    data: () => ({ drawer: null }),
  }
</script>

<style scoped>
  a {
    text-decoration: none;
    color: black;
  }

  .custom-drawer {
    border-right: 2px solid #a10707 !important;
  }

  .custom-app-bar {
    border-bottom: 2px solid #a10707 !important;
  }

  .custom-subheader {
    color: black !important;
    font-size: 1.1rem !important;
    font-weight: 400 !important;
    line-height: 1.4 !important;
    padding: 12px 16px !important;
    background-color: transparent !important;
    margin-bottom: 4px !important;
  }

  .custom-divider {
    border-color: #a10707 !important;
    border-width: 1.5px !important;
    border-style: solid !important;
    opacity: 0.5 !important;
    margin: 8px 10px !important;
  }

  .custom-list-item {
    color: black !important;
  }

  .custom-list-item .v-list-item__title {
    color: black !important;
  }

  .v-list-item__title {
    color: #1976d2 !important;
  }

  .v-list .v-list-item .v-list-item__title {
    color: #1976d2 !important;
  }
</style>
