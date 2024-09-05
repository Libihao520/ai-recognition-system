import request from '@/utils/request'
//获取用户角色列表
export const getUserRoleService = (data) =>
  request.post('/RoleManagement/GetUserRole', data)
//添加用户角色列表
export const putUserRoleService = (data) =>
  request.put('/RoleManagement/PutUserRole', data)

  export const DeletedService = (id) =>
  request.delete('/RoleManagement/Deleted', { params: { id } })