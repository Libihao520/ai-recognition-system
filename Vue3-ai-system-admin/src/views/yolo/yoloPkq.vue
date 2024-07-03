<script setup>
import { ref } from 'vue'
import { GetPkqTbService, DeletedService } from '../../api/yolo'
import { Edit, Delete } from '@element-plus/icons-vue'
import { formatTime } from '@/utils/format.js'
import yoloEdit from './yoloEdit.vue'

const channelList = ref([])
const selectcondition = ref({
  clsName: '全部',
  isaudit: '0'
})
//转菊花
const loading = ref(false)
const getPkqtbList = async () => {
  loading.value = true
  const res = await GetPkqTbService(selectcondition.value)
  channelList.value = res.data.data
  loading.value = false
}
getPkqtbList()
const yoloEditRef = ref()

//添加逻辑
const onAddTblist = () => {
  yoloEditRef.value.open({})
}
//编辑逻辑
const onEditChannel = (row) => {
  console.log(row)
  yoloEditRef.value.open(row)
}
//删除逻辑
const onDelChannel = async (row, $index) => {
  console.log(row.id)
  const res = await DeletedService(row.id)
  getPkqtbList()
}
//重置搜索框
const Resetsearchbox = () => {
  selectcondition.value = {
    clsName: '全部',
    isaudit: '0'
  }
}

//添加或者编辑成功回调
const onSuccess = (type) => {
  if (type === 'add') {
    //TODO渲染最后一页
    getPkqtbList()
  } else {
    //编辑直接渲染当前页
    getPkqtbList()
  }
}
</script>
<template>
  <page-containel title="目标监测识别详情"
    ><el-form inline>
      <!-- 类别 -->
      <el-form-item label="类别:">
        <el-select class="select" v-model="selectcondition.clsName">
          <el-option label="全部" value="全部"></el-option>
          <el-option label="皮卡丘" value="皮卡丘"></el-option>
          <el-option label="动物识别" value="动物识别"></el-option>
          <el-option label="车牌识别" value="车牌识别"></el-option>
        </el-select>
      </el-form-item>
      <!-- 是否审核 -->
      <el-form-item label="审核情况:">
        <el-select class="select" v-model="selectcondition.isaudit">
          <el-option label="全部" value="0"></el-option>
          <el-option label="已审核" value="1"></el-option>
          <el-option label="未审核" value="2"></el-option>
        </el-select>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="getPkqtbList">搜索</el-button>
        <el-button @click="Resetsearchbox">重置</el-button>
      </el-form-item>
    </el-form>
    <template #extra><el-button type="primary" @click="onAddTblist">人工新增</el-button></template>
    <!-- 表单数据 -->
    <el-table v-loading="loading" :data="channelList" style="width: 100%">
      <el-table-column type="index" label="序号" width="100"></el-table-column>
      <el-table-column prop="cls" label="类别"></el-table-column>
      <el-table-column prop="sbjgCount" label="数量"></el-table-column>
      <el-table-column prop="createDate" label="时间">
        <template #default="{ row }">
          {{ formatTime(row.createDate) }}
        </template>
      </el-table-column>
      <el-table-column prop="isManualReview" label="审核">
        <template #default="{ row }">
          {{ row.isManualReview == true ? '已审核' : '未审核' }}</template
        >
      </el-table-column>
      <el-table-column label="操作" width="150">
        <!-- row就是channelList的一项，$index是下标 -->
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

    <!-- 添加编辑抽屉 -->
    <!-- //success监听 -->
    <yolo-edit ref="yoloEditRef" @success="onSuccess"></yolo-edit>
  </page-containel>
</template>
<style lang="scss" scoped>
.select {
  width: 200px;
}
</style>
