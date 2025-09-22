import axios from 'axios'

axios.defaults.baseURL = import.meta.env.VITE_BASE_URL

export function getStudents (name: string, cpf: string, page: number, pageSize: number) {
  const params = new URLSearchParams()
  if (name) {
    params.append('name', name)
  }
  if (cpf) {
    params.append('cpf', cpf)
  }
  if (page) {
    params.append('page', page.toString())
  }
  if (pageSize) {
    params.append('pageSize', pageSize.toString())
  }
  return axios.get('/api/Student/ListStudents', { params })
}
export const getStudentByRa = (ra: number) => axios.get(`/api/Student/SearchStudentByRa/${ra}`)
export const getStudentByCpf = (cpf: string) => axios.get(`/api/Student/SearchStudentByCpf/${cpf}`)
export const getStudentByName = (name: string) => axios.get(`/api/Student/SearchStudentByName/${name}`)
export function createStudent (data: any) {
  return axios.post('/api/Student/CreateStudent', data, {
    headers: { 'Content-Type': 'application/json' } })
}
export const editStudent = (data: any) => axios.put(`/api/Student/EditStudent`, data)
export function deleteStudent (id: string) {
  return axios.delete(`/api/Student/DeleteStudent`, {
    params: { id },
  })
}
