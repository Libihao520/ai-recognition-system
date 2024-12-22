import request from '@/utils/request'

//查询所有模型
export const getModelService = (params) => request.get('/Aigc/GetModelService', { params })

// 创建模型
export const PutModelService = (file, additionalData = {}) => {
  const formData = new FormData();
  formData.append('Model', file);
  for (const [key, value] of Object.entries(additionalData)) {
    formData.append(key, value);
  }
  return request.post('/Aigc/PutModelService', formData, {
    headers: {
      'Content-Type': 'multipart/form-data'
    }
  });
};
//删除
export const DelModelService = (id) =>
  request.delete('/Aigc/DelModelService', { params: { id } })