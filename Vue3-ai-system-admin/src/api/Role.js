import request from '@/utils/request'
//获取用户角色列表
export const getUserRoleService = (data) =>
  request.post('/RoleManagement/GetUserRole', data)