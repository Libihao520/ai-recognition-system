<script setup>
import { ref } from 'vue'
import { Plus, Aim, Promotion } from '@element-plus/icons-vue'
import { PutPhotoService } from '../../api/yolo'
import { getPkqImage, getAnimalImage } from '@/utils/image'

const loading = ref(false)
const imgUrl = ref('')
const uploadRef = ref()
const onSelectFile = (uploadFile) => {
  // 基于 FileReader 读取图片做预览
  const reader = new FileReader()
  reader.readAsDataURL(uploadFile.raw)
  reader.onload = () => {
    imgUrl.value = reader.result
  }
}
const name = ref('皮卡丘')
const sbTest = ref('')
const ModelClass = [
  { label: '目标检测', value: '目标检测' },
  { label: '目标分类', value: '目标分类' },
  { label: '皮卡丘', value: '皮卡丘' }
]
const options = [
  { label: '皮卡丘', value: '皮卡丘' },
  { label: '动物识别', value: '动物识别' },
  { label: '车牌识别（暂未开放）', value: '车牌识别' }
]
//发送请求
const onUpdateAvatar = async () => {
  if (name.value == '车牌识别') {
    ElMessage.error('未开放！')
    return
  }
  // 上传图片
  if (imgUrl.value) {
    loading.value = true
    const res = await PutPhotoService(imgUrl.value, name.value)
    console.log(res.data)
    if (res.data.data) {
      if (name.value == '动物识别') {
        sbTest.value = res.data.data
      } else {
        imgUrl.value = res.data.data
      }
      ElMessage.success('识别成功')
    } else {
      ElMessage.error('未识别出目标')
    }
  } else {
    ElMessage.error('请先上传图片')
  }
  loading.value = false
}
function onExamples() {
  if (name.value == '皮卡丘') {
    imgUrl.value = getPkqImage()
  } else {
    imgUrl.value = getAnimalImage()
  }
}

function clearImage() {
  imgUrl.value = ''
}

function handleNameChange() {
  sbTest.value = ''
}
</script>
<template>
  <page-containel title="AI识别">
    <el-form-item class="select" label="选择模型：">
      <el-select v-model="name" @change="handleNameChange">
        <el-option
          v-for="option in options"
          :key="option.value"
          :label="option.label"
          :value="option.value"
        ></el-option>
        <!-- <el-option label="皮卡丘" value="皮卡丘"></el-option>
        <el-option label="动物识别" value="动物识别"></el-option>
        <el-option label="车牌识别（暂未开放）" value="车牌识别"></el-option> -->
      </el-select>
    </el-form-item>
    <el-form-item class="sbjg" v-if="name != '皮卡丘'" label="识别结果：">
      <el-input
        v-model="sbTest"
        class="input"
        placeholder="现在还木有开始识别呢！"
        :suffix-icon="Calendar"
      />
    </el-form-item>
    <el-upload
      ref="uploadRef"
      :auto-upload="false"
      class="avatar-uploader"
      :show-file-list="false"
      :on-change="onSelectFile"
    >
      <el-icon v-if="!imgUrl" class="avatar-uploader-icon"><Plus /></el-icon>
    </el-upload>
    <!-- 当图片存在时，使用缩略图形式显示 -->
    <div class="image-container">
      <el-image
        style="width: 300px; height: 300px"
        v-if="imgUrl"
        :src="imgUrl"
        :zoom-rate="1.2"
        :max-scale="7"
        :min-scale="0.2"
        :preview-src-list="[imgUrl]"
        :initial-index="0"
        fit="cover"
      />
      <span v-if="imgUrl" class="close-btn" @click="clearImage">×</span>
    </div>
    <br />
    <el-button
      @click="uploadRef.$el.querySelector('input').click()"
      type="primary"
      :icon="Plus"
      size="large"
      >选择图片</el-button
    >
    <el-button @click="onExamples" type="primary" :icon="Promotion" size="large">示例</el-button>
    <el-button @click="onUpdateAvatar" type="success" :icon="Aim" size="large" v-loading="loading"
      >开始识别</el-button
    >
  </page-containel>
</template>
<style lang="scss" scoped>
.image-container {
  position: relative;
  display: inline-block;

  .close-btn {
    position: absolute;
    top: 0;
    right: 0;
    color: white;
    font-size: 20px;
    cursor: pointer;
    padding: 5px;
    background-color: rgba(0, 0, 0, 0.5);
    border-radius: 50%;
    line-height: 1;

    &:hover {
      background-color: rgba(0, 0, 0, 0.7); // 鼠标悬停时改变背景色
    }
  }
}
.sbjg {
  .input {
    width: 208px;
  }
}
.select {
  width: 300px;
}
.avatar-uploader {
  :deep() {
    .avatar {
      width: 278px;
      height: 278px;
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
      width: 278px;
      height: 278px;
      text-align: center;
    }
  }
}
</style>
