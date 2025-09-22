<template>
  <div class="form-container">
    <h2 class="form-title">
      {{ isEdit ? 'Editar Aluno' : 'Cadastrar Novo Aluno' }}
    </h2>

    <form class="student-form" @submit="submitForm">
      <div class="form-group">
        <label class="form-label" for="name">Nome Completo *</label>
        <input
          id="name"
          v-model="name"
          class="input"
          :class="{ 'input-error': errors.name }"
          placeholder="Digite o nome completo do aluno"
          type="text"
        />
        <span v-if="errors.name" class="error-message">{{ errors.name }}</span>
      </div>

      <div v-if="!isEdit" class="form-group">
        <label class="form-label" for="cpf">CPF *</label>
        <input
          id="cpf"
          v-model="cpf"
          class="input"
          :class="{ 'input-error': errors.cpf }"
          maxlength="14"
          placeholder="000.000.000-00"
          type="text"
        />
        <span v-if="errors.cpf" class="error-message">{{ errors.cpf }}</span>
      </div>

      <div class="form-group">
        <label class="form-label" for="email">Email *</label>
        <input
          id="email"
          v-model="email"
          class="input"
          :class="{ 'input-error': errors.email }"
          placeholder="exemplo@dominio.com"
          type="email"
        />
        <span v-if="errors.email" class="error-message">{{ errors.email }}</span>
      </div>

      <div class="form-actions">
        <button
          class="btn btn-secondary"
          type="button"
          @click="router.push('/Home')"
        >
          Cancelar
        </button>
        <button
          class="btn btn-primary"
          :disabled="isSubmitting"
          type="submit"
        >
          <span v-if="isSubmitting" class="loading-spinner">⏳</span>
          {{ isEdit ? 'Atualizar Aluno' : 'Cadastrar Aluno' }}
        </button>
      </div>
    </form>
  </div>
</template>

<script setup lang="ts">
  import { onMounted, ref } from 'vue'
  import { useRoute, useRouter } from 'vue-router'
  import { useToast } from 'vue-toastification'
  import { createStudent, editStudent, getStudentByRa } from '@/api/studentService'
  import { useStudentStore } from '@/stores/studentStore'

  const route = useRoute()
  const router = useRouter()
  const toast = useToast()
  const StudentStore = useStudentStore()
  const name = ref('')
  const cpf = ref('')
  const email = ref('')
  const isSubmitting = ref(false)
  const errors = ref<{ [key: string]: string }>({})

  const ra = route.params.ra
  const raNumber = ra ? Number(ra) : null
  const isEdit = raNumber !== null && !Number.isNaN(raNumber)

  function validateCPF (cpfStr: string) {
    const clean = cpfStr.replace(/[^\d]+/g, '')
    if (clean.length !== 11) return false
    if (/^(\d)\1+$/.test(clean)) return false
    return true
  }

  function validateForm () {
    errors.value = {}
    if (!name.value || name.value.length < 3) {
      errors.value.name = 'Nome deve ter pelo menos 3 caracteres'
    }
    if (!cpf.value || !validateCPF(cpf.value)) {
      errors.value.cpf = 'CPF inválido'
    }
    if (!email.value || !/^\S+@\S+\.\S+$/.test(email.value)) {
      errors.value.email = 'Email inválido'
    }
    return Object.keys(errors.value).length === 0
  }

  onMounted(async () => {
    if (isEdit) {
      try {
        const response = await getStudentByRa(raNumber as number)
        const student = response.data.data
        name.value = student.name || ''
        cpf.value = student.cpf || ''
        email.value = student.email || ''
      } catch (error) {
        console.error(error)
        toast.error('Erro ao carregar aluno')
      }
    }
  })

  async function checkCpfUnique (cpf: string) {
    const response = await StudentStore.fetchStudentByCpf(cpf)
    const student = response?.data?.data
    return !student
  }

  async function submitForm (e: Event) {
    e.preventDefault()

    if (!validateForm()) return

    if (!(await checkCpfUnique(cpf.value)) && !isEdit) {
      toast.error('CPF já cadastrado!')
      return
    }

    isSubmitting.value = true
    try {
      if (isEdit) {
        const response = await editStudent({
          ra: raNumber,
          name: name.value,
          cpf: cpf.value,
          email: email.value,
        })
        if (response.status) {
          toast.success('Aluno atualizado com sucesso!')
        } else {
          toast.error(response.data?.message || 'Erro ao atualizar aluno')
          return
        }
        StudentStore.fetchStudents(1, 5)
      } else {
        const response = await createStudent({ name: name.value, cpf: cpf.value, email: email.value })
        if (response.data?.status) {
          toast.success(response.data?.message || 'Aluno cadastrado com sucesso!')
          StudentStore.fetchStudents(1, 5)
        } else {
          toast.error(response.data?.message || 'Erro ao cadastrar aluno')
          return
        }
      }
      router.push('/Home')
    } catch (error) {
      toast.error('Erro ao salvar aluno')
      console.error(error)
    } finally {
      isSubmitting.value = false
    }
  }
</script>

<style scoped>
.form-container {
  max-width: 600px;
  margin: 2rem auto;
  padding: 2rem;
  background: #fff;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

.form-title {
  text-align: center;
  color: #333;
  margin-bottom: 2rem;
  font-size: 1.8rem;
}

.student-form {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.form-group {
  display: flex;
  flex-direction: column;
}

.form-label {
  font-weight: 600;
  color: #555;
  margin-bottom: 0.5rem;
  font-size: 0.9rem;
}

.input {
  width: 100%;
  padding: 0.8rem;
  border: 2px solid #ddd;
  border-radius: 6px;
  font-size: 1rem;
  transition: border-color 0.2s ease;
  box-sizing: border-box;
}

.input:focus {
  outline: none;
  border-color: #a10707;
  box-shadow: 0 0 0 3px rgba(76, 175, 80, 0.1);
}

.input-error {
  border-color: #eed707;
}

.error-message {
  color: #eed707;
  font-size: 0.8rem;
  margin-top: 0.25rem;
  font-weight: 500;
}

.form-actions {
  display: flex;
  gap: 1rem;
  justify-content: flex-end;
  margin-top: 2rem;
  padding-top: 1.5rem;
  border-top: 1px solid #eee;
}

.btn {
  padding: 0.8rem 1.5rem;
  border: none;
  border-radius: 6px;
  font-size: 1rem;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s ease;
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
}

.btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.btn-primary {
  background-color: #a10707;
  color: white;
}

.btn-primary:hover:not(:disabled) {
  background-color: #a10707;
  transform: translateY(-1px);
}

.btn-secondary {
  background-color: #6c757d;
  color: white;
}

.btn-secondary:hover {
  background-color: #5a6268;
}

.loading-spinner {
  animation: spin 1s linear infinite;
}

@keyframes spin {
  from { transform: rotate(0deg); }
  to { transform: rotate(360deg); }
}

@media (max-width: 768px) {
  .form-container {
    margin: 1rem;
    padding: 1.5rem;
  }

  .form-actions {
    flex-direction: column;
  }

  .btn {
    width: 100%;
    justify-content: center;
  }
}
</style>
