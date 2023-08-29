<script setup>
import { ref } from 'vue'
import { Plus } from '@element-plus/icons-vue'

//控制抽屉显示隐藏
const visibleDrawer = ref(false)

//默认表单数据
const defaultForm = {
  cls: '',
  sbjgCount: '',
  createDate: '',
  isManualReview: ''
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
}
const open = (row) => {
  visibleDrawer.value = true
  if (row.id) {
    //如果id存在，要发请求获取对应数据，进行回显
  } else {
    formModel.value = {
      ...defaultForm
    }
    console.log('注册')
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
        <el-button type="primary">保存</el-button>
        <el-button type="info">取消</el-button>
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
