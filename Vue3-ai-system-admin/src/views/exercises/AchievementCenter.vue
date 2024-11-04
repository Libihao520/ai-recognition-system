<script setup>
import { ref } from 'vue'
import {
  ElContainer,
  ElHeader,
  ElMain,
  ElFooter,
  ElButton,
  ElRadioGroup,
  ElRadio,
  ElMessage
} from 'element-plus'
import { Download, Delete } from '@element-plus/icons-vue'
import { GetAchievementCenter } from '../../api/exercises'
const channelList = ref([])
// 异步方法获取题目
async function fetchQuestions() {
  const res = await GetAchievementCenter()
  console.log(res.data.data)
  channelList.value = res.data.data
}
// 组件创建时调用 fetchQuestions
fetchQuestions()
</script>  
<template>
  <page-containel title="成绩中心">
    <el-form inline> </el-form>
    <el-table v-loading="loading" :data="channelList" style="width: 100%">
      <el-table-column type="index" label="序号" width="100"></el-table-column>
      <el-table-column prop="subject" label="科目"> </el-table-column>
      <el-table-column prop="correctQuantity" label="答对数量"> </el-table-column>
      <el-table-column prop="numberOfQuestions" label="题目数量"> </el-table-column>
      <el-table-column prop="totalPoints" label="分数"> </el-table-column>
      <el-table-column label="操作" width="150">
        <template #default="{ row, $index }">
          <el-button
            :icon="Download"
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
          ></el-button> </template
      >
    </el-table-column>
    </el-table>
  </page-containel>
</template>

