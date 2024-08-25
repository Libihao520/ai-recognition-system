<script setup>
import { ref } from 'vue'
import { useUserStore } from '@/stores'

const formRef = ref()

// 是在使用仓库中数据的初始值 (无需响应式) 解构无问题
const {
  user: { id, name, createDate },
  getUser
} = useUserStore()

const form = ref({
  id,
  name,
  createDate
})
</script>
<template>
  <page-containel title="用户角色管理">
    <el-form inline>
      <el-form-item label="用户名称:">
        <el-input placeholder="请输入用户名称"></el-input>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="getPkqtbList">搜索</el-button>
        <el-button @click="Resetsearchbox">重置</el-button>
      </el-form-item>
    </el-form>

    <!-- 表单部分 -->
    <el-form ref="formRef" :model="form" :rules="rules" label-width="100px">
      <el-form-item label="用户昵称" prop="nickname">
        <el-input v-model="form.name"></el-input>
      </el-form-item>
      <el-form-item label="创建时间" prop="email">
        <el-input v-model="form.createDate"></el-input>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="submitForm">提交修改</el-button>
      </el-form-item>
    </el-form>
  </page-containel>
</template>
<style lang="scss" scoped>
.select {
  width: 200px;
}
</style>