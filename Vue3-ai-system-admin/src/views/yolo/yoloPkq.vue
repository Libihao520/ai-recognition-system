<script setup>
import { ref } from 'vue'
import { GetPkqTbService } from '../../api/yolo'
import { Edit, Delete } from '@element-plus/icons-vue'
const channelList = ref([])
//转菊花
const loading = ref(false)
const getPkqtbList = async () => {
  loading.value = true
  const res = await GetPkqTbService()
  channelList.value = res.data.data
  loading.value = false
}
getPkqtbList()
const onEditChannel = (row, $index) => {
  console.log(row, $index)
}
const onDelChannel = (row, $index) => {
  console.log(row, $index)
}
</script>
<template>
  <page-containel title="皮卡丘识别详情"
    ><template #extra><el-button>测试按钮</el-button></template>
    <!-- 表单数据 -->
    <el-table v-loading="loading" :data="channelList" style="width: 100%">
      <el-table-column type="index" label="序号" width="100"></el-table-column>
      <el-table-column prop="cls" label="类别"></el-table-column>
      <el-table-column prop="sbjgCount" label="数量"></el-table-column>
      <el-table-column prop="createDate" label="时间"></el-table-column>
      <el-table-column prop="isManualReview" label="审核"></el-table-column>
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
  </page-containel>
</template>
<style lang="scss" scoped></style>
