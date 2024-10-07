import request from '@/utils/request'

//获取数学题目
export const getmMthematics = () => request.get('/Exercises/GetmMthematics')

//提交
export const postSubmitExercises = (data) =>
  request.post('/Exercises/Submit', data)