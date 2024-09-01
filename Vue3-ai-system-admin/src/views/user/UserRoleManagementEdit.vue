<script setup>
import { ref } from 'vue'
import { getUserRoleService } from '../../api/Role'

const loading = ref(false)
//控制抽屉显示隐藏
const visibleDrawer = ref(false)
//抽屉是添加还是编辑
const isAdd = ref(true)

//默认表单数据
const defaultForm = {
  id: null,
  name: ''
}

//请求体
const selectcondition = ref({
  // pagenum: 1, //当前页
  // pagesize: 5, //每页条数
  id: '',
  username: ''
})
//准备数据
const formModel = ref({
  ...defaultForm
})

// 父类点击抽屉时传row到这里
const open = async (row) => {
  visibleDrawer.value = true
  //手动重置数据
  formModel.value = {
    ...defaultForm
  }
  //如果id存在发请求获取对应数据，进行回显
  if (row.id) {
    loading.value = true
    isAdd.value = false
    selectcondition.value.id = row.id
    const res = await getUserRoleService(selectcondition.value)
    formModel.value = res.data.data[0]
    loading.value = false
    console.log(formModel.value)
  } else {
    isAdd.value = true
  }
}
defineExpose({
  open
})
</script>
<template>
  <el-drawer
    v-model="visibleDrawer"
    :title="isAdd ? '手动新增' : '编辑'"
    direction="rtl"
    size="50%"
  >
    <el-form :model="formModel.value" ref="formRef" label-width="100px" v-loading="loading">
      <el-form-item label="姓名:" prop="title">
        <el-input class="inputcss" v-model="formModel.name" placeholder="请输入用户姓名"></el-input>
      </el-form-item>
    </el-form>
  </el-drawer>
</template>


<style lang="scss" scoped>
.inputcss {
  width: 220px;
}
</style>
