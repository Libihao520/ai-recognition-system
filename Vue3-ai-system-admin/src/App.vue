<script setup>
import { useUserStore } from '@/stores/user'

const userStore = useUserStore()
</script>
<template>
  <test-demo></test-demo>
  <div>
    <el-button>按钮</el-button>
    <p>{{ userStore.token }}</p>
    <el-button @click="userStore.setToken('bearer')">登录</el-button>
    <el-button @click="userStore.removeToken()">退出</el-button>
  </div>
</template>

<style scoped></style>
