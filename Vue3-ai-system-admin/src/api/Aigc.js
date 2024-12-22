import request from '@/utils/request'

//查询所有模型
export const getModelService = (params) => request.get('/Aigc/GetModelService',{params})

export const PutModelService = (data) =>
  request.put('/Aigc/PutModelService', data)