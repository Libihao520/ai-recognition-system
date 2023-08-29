<script setup>
import { ref } from 'vue'
import { Plus } from '@element-plus/icons-vue'
import { PubListPkqTbService, getPkqEditTbService } from '@/api/yolo'

//控制抽屉显示隐藏
const visibleDrawer = ref(false)

//默认表单数据
const defaultForm = {
  id: '',
  cls: '',
  sbjgCount: '',
  createDate: '',
  isManualReview: '',
  cover_img: ''
  // TODO
}

//准备数据
const formModel = ref({
  ...defaultForm
})
//图片上传相关逻辑
const imgUrl = ref('')
const onUploadFile = (uploadFile) => {
  imgUrl.value = URL.createObjectURL(uploadFile.raw)
  //将上传的图片保存到formModel，后面提交
  formModel.value.cover_img = uploadFile.raw
}
//提交
const emit = defineEmits(['sucess'])
const onSave = async (state) => {
  if (state == '取消') {
    visibleDrawer.value = false
  } else {
    const fd = new FormData()
    for (let key in formModel.value) {
      fd.append(key, formModel.value[key])
    }
    //有id是编辑操作
    if (formModel.value.id) {
      console.log('编辑保存操作')
    } else {
      console.log('添加保存操作')
      await PubListPkqTbService(fd)
      ElMessage.success('手动添加数据成功')
      visibleDrawer.value = false
      //通知
      emit('success', 'add')
    }
  }
}
// 父类点击抽屉时传row到这里
const open = async (row) => {
  visibleDrawer.value = true
  console.log(row)
  if (row.id) {
    //如果id存在，要发请求获取对应数据，进行回显
    const res = await getPkqEditTbService(row.id)
    formModel.value = res.data.data
  } else {
    formModel.value = {
      ...defaultForm
    } //手动重置数据
    imgUrl.value = ''
    console.log('添加')
  }
}

defineExpose({
  open
})
</script>
<template>
  <el-drawer
    v-model="visibleDrawer"
    :title="formModel.cls ? '编辑' : '添加'"
    direction="rtl"
    size="50%"
  >
    <!-- l类别框 -->
    <el-form :model="formModel" ref="formRef" label-width="100px">
      <el-form-item label="类别" prop="title">
        <el-input v-model="formModel.title" placeholder="请输入类别"></el-input>
      </el-form-item>
      <!-- 图片 -->
      <el-form-item label="识别图片" prop="cover_img">
        <el-upload
          class="avatar-uploader"
          :auto-upload="false"
          :show-file-list="false"
          :on-change="onUploadFile"
        >
          <img v-if="imgUrl" :src="imgUrl" class="avatar" />
          <el-icon v-else class="avatar-uploader-icon"><Plus /></el-icon>
        </el-upload>
      </el-form-item>
      <el-form-item>
        <el-button @click="onSave('已保存')" type="primary">保存</el-button>
        <el-button @click="onSave('取消')" type="info">取消</el-button>
      </el-form-item>
    </el-form>
  </el-drawer>
</template>
<style lang="scss" scoped>
.avatar-uploader {
  :deep() {
    .avatar {
      width: 178px;
      height: 178px;
      display: block;
    }
    .el-upload {
      border: 1px dashed var(--el-border-color);
      border-radius: 6px;
      cursor: pointer;
      position: relative;
      overflow: hidden;
      transition: var(--el-transition-duration-fast);
    }
    .el-upload:hover {
      border-color: var(--el-color-primary);
    }
    .el-icon.avatar-uploader-icon {
      font-size: 28px;
      color: #8c939d;
      width: 178px;
      height: 178px;
      text-align: center;
    }
  }
}
</style>
