import { defineStore } from 'pinia'
import {
  deleteStudent,
  getStudentByCpf,
  getStudentByName,
  getStudentByRa,
  getStudents,
} from '@/api/studentService'

interface Student {
  ra: number
  name: string
  cpf: string
  email: string
}

export const useStudentStore = defineStore('student', {
  state: () => ({
    students: [] as Student[],
    totalPages: 1,
    totalItems: 0,
    currentPage: 1,
  }),
  actions: {
    async fetchStudents (page: number, pageSize: number) {
      try {
        const response = await getStudents('', '', page, pageSize)
        if (response?.data) {
          const studentsData = Array.isArray(response.data.data) ? response.data.data : []
          this.students = studentsData
          this.totalPages = response.data.totalPages || 1
          this.totalItems = response.data.totalRegisters || studentsData.length > 0
          this.currentPage = page
        } else {
          this.students = []
          this.totalPages = 1
          this.totalItems = 0
        }
      } catch (error) {
        console.error('ERRO:', error)
        this.students = []
        this.totalPages = 1
        this.totalItems = 0
      }
    },

    async fetchStudentByRa (ra: number) {
      try {
        const response = await getStudentByRa(ra)
        this.students = response?.data ? [response.data.data] : []
      } catch (error) {
        console.error('Erro ao buscar por RA:', error)
        this.students = []
      }
    },

    async fetchStudentByCpf (cpf: string) {
      try {
        const response = await getStudentByCpf(cpf)
        this.students = response?.data ? [response.data.data] : []
        return response
      } catch (error) {
        console.error('Erro ao buscar por CPF:', error)
        this.students = []
        return null
      }
    },

    async fetchStudentByName (name: string) {
      try {
        const response = await getStudentByName(name)
        this.students = response?.data?.data || []
      } catch (error) {
        console.error('Erro ao buscar por Nome:', error)
        this.students = []
      }
    },

    async deleteStudent (ra: number) {
      try {
        await deleteStudent(ra.toString())
        if (Array.isArray(this.students)) {
          this.students = this.students.filter(student => student.ra !== ra)
        }
      } catch (error) {
        console.error('Erro ao excluir:', error)
      }
    },
  },
})
