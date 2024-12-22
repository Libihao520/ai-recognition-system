<script setup>
import { ref, toRaw } from 'vue'
import { PutModelService } from '../../api/Aigc'
import { encrypt } from '@/utils/util'
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
  pagenum: 0,
  pagesize: 0,
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
  isAdd.value = true
}
//表单提交
const emit = defineEmits(['sucess'])
const onSave = async (state) => {
  const putFrom = ref({})
  putFrom.value = JSON.parse(JSON.stringify(formModel.value))
  putFrom.value.password = encrypt(formModel.value.password)

  if (state == '取消') {
    visibleDrawer.value = false
  } else {
    await PutModelService(putFrom.value)
    visibleDrawer.value = false
    // 回调
    emit('success', 'add')
  }
}
defineExpose({
  open
})
</script>
<template>
  <el-drawer v-model="visibleDrawer" :title="isAdd ? '新增' : '编辑'" direction="rtl" size="50%">
    <el-form :model="formModel.value" ref="formRef" label-width="100px" v-loading="loading">
      <el-form-item label="模型类型:">
        <el-select class="inputcss" v-model="formModel.modleCls">
          <el-option label="目标监测" value="目标监测"></el-option>
          <el-option label="图像分类" value="图像分类"></el-option>
          <el-option label="其他模型" value="其他模型"></el-option>
        </el-select>
      </el-form-item>
      <el-form-item label="模型名称:">
        <el-input
          class="inputcss"
          v-model="formModel.modelName"
          placeholder="请输入模型名称！"
          clearable
        ></el-input>
      </el-form-item>
      <el-form-item>
        <el-button @click="onSave('已保存')" type="primary">{{
          isAdd ? '提交' : '保存'
        }}</el-button>
        <el-button @click="onSave('取消')" type="info">取消</el-button>
      </el-form-item>
    </el-form>
  </el-drawer>
</template>


<style lang="scss" scoped>
.inputcss {
  width: 220px;
}

.selectcss {
  width: 320px;
}
</style>
