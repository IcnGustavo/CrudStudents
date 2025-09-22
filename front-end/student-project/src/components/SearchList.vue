<template>
  <v-card class="mx-auto custom-card">
    <v-toolbar color="#a10707">
      <v-toolbar-title>Consulta de alunos</v-toolbar-title>
      <v-text-field
        v-model="search"
        class="search-field"
        density="compact"
        hide-details
        label="Search"
        single-line
        variant="outlined"
        @keyup.enter="searchStudentAux"
      />

      <v-btn color="white" variant="outlined" @click="searchStudentAux">
        <v-icon>mdi-magnify</v-icon>
      </v-btn>
      <div class="checkbox-group">
        <v-checkbox
          v-model="filterByName"
          class="mx-2"
          color="white"
          density="compact"
          :disabled="filterByCPF || filterByRA"
          label="Nome"
        />
        <v-checkbox
          v-model="filterByRA"
          class="mx-2"
          color="white"
          density="compact"
          :disabled="filterByName || filterByCPF"
          label="RA"
        />
        <v-checkbox
          v-model="filterByCPF"
          class="mx-2"
          color="white"
          density="compact"
          :disabled="filterByName || filterByRA"
          label="CPF"
        />
      </div>
      <v-btn class="btn-spacing" color="white" variant="outlined" @click="createStudentAux">
        <template #prepend>
          <v-icon>mdi-account-plus-outline</v-icon>
        </template>
        Novo aluno
      </v-btn>
    </v-toolbar>

    <div v-if="showDebug" class="debug-info">
      <pre>{{ debugData }}</pre>
    </div>

    <v-list class="custom-list" lines="two">
      <v-list-item
        v-for="(student, index) in validStudents"
        :key="student.ra || `student-${index}`"
      >
        <template #prepend>
          <v-avatar>
            <v-icon color="black">mdi-account-circle-outline</v-icon>
          </v-avatar>
        </template>

        <template #title>
          <span class="custom-title">{{ student.name || 'Nome não encontrado' }}</span>
        </template>

        <template #subtitle>
          <span class="custom-subtitle">
            RA: {{ student.ra || 'N/A' }} - CPF: {{ student.cpf || 'N/A' }}
          </span>
        </template>

        <template #append>
          <v-btn
            color="black"
            icon="mdi-pencil-outline"
            variant="text"
            @click="editStudentAux(student.ra)"
          ></v-btn>
          <v-btn
            color="black"
            icon="mdi-delete-outline"
            variant="text"
            @click="openDeleteDialog(student.ra)"
          ></v-btn>
        </template>
      </v-list-item>

      <v-list-item v-if="validStudents.length === 0" disabled>
        <template #title>
          <span class="text-center grey--text">
            Nenhum aluno encontrado (Total: {{ studentStore.students?.length || 0 }})
          </span>
        </template>
      </v-list-item>
    </v-list>
    <v-pagination
      v-model="currentPage"
      class="mt-4 custom-pagination"
      color="#a10707"
      :length="totalPages"
      :total-visible="7"
      @update:model-value="loadStudents"
    />
    <v-dialog v-model="showDeleteDialog" max-width="400">
      <v-card>
        <v-card-title class="text-h6">Deletar aluno permanentemente?</v-card-title>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="grey" text @click="cancelDelete">Cancelar</v-btn>
          <v-btn color="red" text @click="deleteStudent">Deletar</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-card>
</template>

<script setup lang="ts">
  import { computed, onMounted, ref } from 'vue'
  import { useRouter } from 'vue-router'
  import { useToast } from 'vue-toastification'
  import { useStudentStore } from '@/stores/studentStore'

  const toast = useToast()
  const filterByName = ref(false)
  const filterByCPF = ref(false)
  const filterByRA = ref(false)
  const studentStore = useStudentStore()
  const search = ref('')
  const showDebug = ref(false)
  const router = useRouter()
  const currentPage = ref(1)
  const totalPages = computed(() => studentStore.totalPages || 1)
  const itemsPerPage = 10

  onMounted(() => {
    loadStudents(1)
  })

  async function loadStudents (page = 1) {
    await studentStore.fetchStudents(page, itemsPerPage)
    currentPage.value = page
  }

  const validStudents = computed(() => {
    const students = Array.isArray(studentStore.students) ? studentStore.students : []
    const valid = students.filter(student =>
      student
      && typeof student === 'object'
      && student.ra !== undefined)
    return valid
  })

  const debugData = computed(() => ({
    total: studentStore.students?.length || 0,
    isArray: Array.isArray(studentStore.students),
    first: studentStore.students?.[0],
    validCount: validStudents.value.length }))

  function editStudentAux (raAux: number) {
    router.push({ name: 'StudentRegister', params: { ra: raAux } })
  }

  function createStudentAux () {
    router.push('StudentRegister')
  }

  function isValidCpf (cpf: string): boolean {
    cpf = cpf.replace(/[^\d]+/g, '')
    if (cpf.length !== 11 || /^(\d)\1+$/.test(cpf)) return false

    let soma = 0
    let resto

    for (let i = 1; i <= 9; i++) soma += Number.parseInt(cpf.slice(i - 1, i)) * (11 - i)
    resto = (soma * 10) % 11
    if (resto === 10 || resto === 11) resto = 0
    if (resto !== Number.parseInt(cpf.slice(9, 10))) return false

    soma = 0
    for (let i = 1; i <= 10; i++) soma += Number.parseInt(cpf.slice(i - 1, i)) * (12 - i)
    resto = (soma * 10) % 11
    if (resto === 10 || resto === 11) resto = 0
    if (resto !== Number.parseInt(cpf.slice(10, 11))) return false

    return true
  }

  async function searchStudentAux () {
    if (!search.value || (!filterByName.value && !filterByCPF.value && !filterByRA.value)) {
      await loadStudents(1)
    }

    if (filterByCPF.value) {
      if (!isValidCpf(search.value)) {
        toast.info('Digite um CPF válido com 11 dígitos')
        return
      }
      await studentStore.fetchStudentByCpf(search.value)
      currentPage.value = 1
    } else if (filterByName.value) {
      await studentStore.fetchStudentByName(search.value)
      currentPage.value = 1
    } else if (filterByRA.value) {
      const raNumber = Number(search.value)
      if (Number.isNaN(raNumber)) {
        toast.info('O RA deve ser um número válido')
        return
      }
      await studentStore.fetchStudentByRa(raNumber)
      currentPage.value = 1
    }
  }

  const showDeleteDialog = ref(false)
  const studentToDelete = ref<number | null>(null)

  function openDeleteDialog (ra: number) {
    studentToDelete.value = ra
    showDeleteDialog.value = true
  }

  async function deleteStudent () {
    if (studentToDelete.value !== null) {
      await studentStore.deleteStudent(studentToDelete.value)
      studentStore.fetchStudents(1, itemsPerPage)
      showDeleteDialog.value = false
      studentToDelete.value = null
      loadStudents(currentPage.value)
    }
  }

  function cancelDelete () {
    showDeleteDialog.value = false
    studentToDelete.value = null
  }
</script>

<style scoped>
.custom-card {
  width: calc(100% - 12px) !important;
  height: calc(100vh - 34px) !important;
  margin: 16px auto !important;
  border-radius: 8px !important;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1) !important;
}

.custom-list {
  height: auto !important;
  max-height: calc(100% - 150px);
  overflow-y: auto !important;
}

.v-pagination {
  margin-top: 16px;
}

.custom-pagination {
  --v-theme-on-primary: #000000 !important;
  --v-theme-on-surface: #000000 !important;
  color: #000 !important;
}

.debug-info {
  background: #f5f5f5;
  padding: 8px;
  font-size: 12px;
  max-height: 200px;
  overflow-y: auto;
  margin: 8px;
}

.black-title {
  color: #000000 !important;
  font-weight: 500 !important;
}

.custom-title {
  color: #000000 !important;
  font-weight: 500 !important;
}

.custom-subtitle {
  color: #666666 !important;
  font-size: 0.875rem !important;
}
.search-field {
  max-width: 350px !important;
  min-width: 150px !important;
  margin-right: 8px !important;
}
.btn-spacing {
  margin-right: 8px !important;
  margin-left: 8px !important;
}
.checkbox-group {
  display: flex;
  gap: 0.5px;
  align-items: center;
  margin-top: 20px;
}
</style>
