<script setup>
import { useUserStore } from '@/stores'

const userStore = useUserStore()
</script>
<template>
  <router-view></router-view>
  <hr />
  <test-demo></test-demo>
  <div>
    <el-button>按钮</el-button>
    <p>{{ userStore.token }}</p>
    <el-button @click="userStore.setToken('bearer')">登录</el-button>
    <el-button @click="userStore.removeToken()">退出</el-button>
  </div>
</template>

<style scoped></style>
