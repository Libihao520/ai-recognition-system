<script setup>
import { ref } from 'vue'
import { useUserStore } from '@/stores'
import { getUserRoleService,DeletedService } from '../../api/Role'
import { Edit, Delete } from '@element-plus/icons-vue'
import { formatTime } from '@/utils/format.js'
import UserRoleManagementEdit from './UserRoleManagementEdit.vue'

//表单数据
const channelList = ref([])

//请求体
const selectcondition = ref({
  // pagenum: 1, //当前页
  // pagesize: 5, //每页条数
  username: ''
})
//转菊花
const loading = ref(false)

//抽屉
const RoleManagementEditRef = ref()

//添加逻辑（不传值）
const onAddTblist = () => {
  RoleManagementEditRef.value.open({})
}
//编辑逻辑(传值抽屉)
const onEditChannel = (row) => {
  RoleManagementEditRef.value.open(row)
}
//添加或者编辑成功回调
const onSuccess = (type) => {
  getUserRoleList()
}

//删除逻辑
const onDelChannel = async (row, $index) => {
  console.log(row.id)
  const res = await DeletedService(row.id)
  getUserRoleList()
}

//重置搜索框
const Resetsearchbox = () => {
  selectcondition.value.username = ''
}

const getUserRoleList = async () => {
  loading.value = true
  const res = await getUserRoleService(selectcondition.value)
  channelList.value = res.data.data
  // total.value = res.data.total
  loading.value = false
}
getUserRoleList()
</script>
<template>
  <page-containel title="用户角色管理">
    <el-form inline>
      <el-form-item label="用户名称:">
        <el-input placeholder="请输入用户名称" v-model="selectcondition.username"></el-input>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="getPkqtbList">搜索</el-button>
        <el-button @click="Resetsearchbox">重置</el-button>
      </el-form-item>
    </el-form>
    <template #extra><el-button type="primary" @click="onAddTblist">添加用户</el-button></template>
    <!-- 表单数据 -->
    <el-table v-loading="loading" :data="channelList" style="width: 100%">
      <el-table-column type="index" label="序号" width="100"></el-table-column>
      <el-table-column prop="name" label="用户名称"></el-table-column>
      <el-table-column prop="role" label="用户角色"></el-table-column>
      <el-table-column prop="createDate" label="时间">
        <template #default="{ row }">
          {{ formatTime(row.createDate) }}
        </template>
      </el-table-column>
      <el-table-column prop="email" label="邮箱"></el-table-column>
      <el-table-column label="操作" width="150">
        <template #default="{ row, $index }">
          <el-button
            :icon="Edit"
            circle
            plain
            type="primary"
            @click="onEditChannel(row, $index)"
          ></el-button>

          <el-button
            :icon="Delete"
            circle
            plain
            type="danger"
            @click="onDelChannel(row, $index)"
          ></el-button>
        </template>
      </el-table-column>
    </el-table>
  </page-containel>
  <!-- 添加编辑抽屉 -->
  <User-role-Management-Edit
    ref="RoleManagementEditRef"
    @success="onSuccess"
  ></User-role-Management-Edit>
</template>
<style lang="scss" scoped>
.select {
  width: 200px;
}
</style>