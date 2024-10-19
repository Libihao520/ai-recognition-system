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

import { getmMthematics, postSubmitExercises } from '../../api/exercises'

// 使用 ref 声明响应式数据
const singleChoice = ref([])
const multipleChoice = ref([])
const trueFalse = ref([])
const answers = ref([]) // 单选题答案
const multipleAnswers = ref([]) // 多选题答案（数组形式，存储选中的所有选项）
const trueFalseAnswers = ref([]) // 判断题答案（true 或 false）

// 异步方法获取题目
async function fetchQuestions() {
  const questions = await getmMthematics()
  console.log(questions.data)
  singleChoice.value = questions.data.data.singleChoice.map((question) => ({
    title: question.title,
    options: question.options,
    topicNumber: question.topicNumber
  }))
  multipleChoice.value = questions.data.data.multipleChoice.map((question) => ({
    title: question.title,
    options: question.options,
    topicNumber: question.topicNumber
  }))
  trueFalse.value = questions.data.data.trueFalse.map((question) => ({
    title: question.title,
    topicNumber: question.topicNumber
  }))

  // 初始化答案数组，确保每个问题都有一个对应的答案位置
  answers.value = singleChoice.value.map(() => null)
  multipleAnswers.value = multipleChoice.value.map(() => [])
  trueFalseAnswers.value = trueFalse.value.map(() => null)
}

// 检查是否有未回答的题目
function hasUnansweredQuestions() {
  return (
    answers.value.includes(null) ||
    multipleAnswers.value.some((answers) => answers.length === 0) ||
    trueFalseAnswers.value.includes(null)
  )
}
// 提交答案的方法
async function submit() {
  if (hasUnansweredQuestions()) {
    ElMessage({
      message: '请仔细检查，确保所有题目都已回答！',
      type: 'warning'
    })

    return
  }

  const questions = await postSubmitExercises({
    singleChoice: answers.value,
    multipleChoice: multipleAnswers.value,
    trueFalse: trueFalseAnswers.value
  })
  ElMessageBox.alert(questions.data.data, '提示', { confirmButtonText: '明白' })
}

// 组件创建时调用 fetchQuestions
fetchQuestions()
</script>  
<template>
  <page-containel title="数学题">
    <el-container>
      <el-header>
        <h1>注意：本网页包含单选题、多选题和判断题，在完成答题后点击提交按钮！</h1>
      </el-header>
      <el-main>
        <h2>单选题</h2>
        <div v-for="(question, index) in singleChoice" :key="index">
          <p>{{ question.topicNumber + '. ' + question.title }}</p>
          <el-radio-group v-model="answers[index]">
            <el-radio
              v-for="(option, optionIndex) in question.options"
              :key="optionIndex"
              :label="optionIndex"
              >{{ option }}</el-radio
            >
          </el-radio-group>
        </div>

        <!-- 多选题部分 -->
        <div v-if="multipleChoice.length">
          <h2>多选题</h2>
          <div v-for="(question, index) in multipleChoice" :key="index">
            <p>{{ question.topicNumber + '. ' + question.title }}</p>
            <el-checkbox-group v-model="multipleAnswers[index]">
              <el-checkbox
                v-for="(option, optionIndex) in question.options"
                :key="optionIndex"
                :label="optionIndex"
                >{{ option }}</el-checkbox
              >
            </el-checkbox-group>
          </div>
        </div>

        <!-- 判断题部分 -->
        <div v-if="trueFalse.length">
          <h2>判断题</h2>
          <div v-for="(question, index) in trueFalse" :key="index">
            <p>{{ question.topicNumber + '. ' + question.title }}</p>
            <el-radio-group v-model="trueFalseAnswers[index]">
              <el-radio label="true">正确</el-radio>
              <el-radio label="false">错误</el-radio>
            </el-radio-group>
          </div>
        </div>
      </el-main>
      <el-footer>
        <el-button type="primary" @click="submit">提交</el-button>
      </el-footer>
    </el-container>
  </page-containel>
</template>

