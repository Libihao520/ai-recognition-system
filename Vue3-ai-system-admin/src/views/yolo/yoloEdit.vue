<script setup>
import { ref } from 'vue'
//控制抽屉显示隐藏
const visibleDrawer = ref(false)

const open = (row) => {
  visibleDrawer.value = true
  console.log(row)
}

defineExpose({
  open
})
</script>
<template>
  <!-- 抽屉 -->
  <el-drawer v-model="visibleDrawer" title="大标题" size="50%">
    <span>Hi there!</span></el-drawer
  >
</template>
