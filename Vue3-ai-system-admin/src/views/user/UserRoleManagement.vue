<script setup>
import { ref } from 'vue'
import { useUserStore } from '@/stores'
import { getUserRoleService } from '../../api/Role'
import { Edit, Delete } from '@element-plus/icons-vue'
import { formatTime } from '@/utils/format.js'

const formRef = ref()
const channelList = ref([])

//转菊花
const loading = ref(false)

// 是在使用仓库中数据的初始值 (无需响应式) 解构无问题
const {
  user: { id, name, createDate },
  getUser
} = useUserStore()

//请求体
const selectcondition = ref({
  // pagenum: 1, //当前页
  // pagesize: 5, //每页条数
  username: ''
})
//重置搜索框
const Resetsearchbox = () => {
  selectcondition.value.username = ''
}
const form = ref({
  id,
  name,
  createDate
})

const getUserRoleList = async () => {
  loading.value = true
  const res = await getUserRoleService(selectcondition.value)
  channelList.value = res.data.data
  console.log(res.data.data)
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
</template>
<style lang="scss" scoped>
.select {
  width: 200px;
}
</style>