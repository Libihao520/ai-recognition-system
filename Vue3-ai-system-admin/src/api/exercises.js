import request from '@/utils/request'

//获取数学题目
export const getmMthematics = () => request.get('/Exercises/GetmMthematics')

//提交
export const postSubmitExercises = (data) =>
  request.post('/Exercises/Submit', data)

//成绩中心
export const GetAchievementCenter = (params) => request.get('/Exercises/GetAchievementCenter',{ params })

//删除
export const DeletedService = (id) =>
  request.delete('/Exercises/Deleted', { params: { id } })