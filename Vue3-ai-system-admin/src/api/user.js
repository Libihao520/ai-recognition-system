import request from '@/utils/request'

//用户注册接口
export const userRegisterService = ({ username, password, repassword }) =>
  request.post('/login/add', { username, password, repassword })
