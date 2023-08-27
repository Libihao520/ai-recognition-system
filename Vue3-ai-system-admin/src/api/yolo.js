import request from '@/utils/request'

//获取pkq表单数据
export const GetPkqTbService = () => request.get('/yolo/yolopkq')

export const PutPhotoService = (photo, name) => {
  return request.put('/yolo/PutPhoto', { photo, name })
}
