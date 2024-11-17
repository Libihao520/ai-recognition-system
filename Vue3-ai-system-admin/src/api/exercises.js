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


//导出用户作答情况
export const DownloadWordService = (id) => {
  return request.get('/Exercises/DownloadWord', {
    params: { id },
    responseType: 'blob' 
  }).then(response => {
    const url = window.URL.createObjectURL(new Blob([response.data]));
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', '用户作答情况.docx'); // 设置下载文件名  
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    window.URL.revokeObjectURL(url);
  });
};