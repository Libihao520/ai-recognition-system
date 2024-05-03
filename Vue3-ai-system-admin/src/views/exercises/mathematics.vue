<template>
    <el-container>
      <el-header>
        <h1>本网页包含单选题、多选题和判断题，在完成答题后点击提交按钮！</h1>
      </el-header>
      <el-main>
          <h2>单选题</h2>
        <div v-for="(question, index) in singleChoice" :key="index">
          <p>{{ question.title }}</p>
          <el-radio-group v-model="answers[index]">
            <el-radio
              v-for="(option, optionIndex) in question.options"
              :key="optionIndex"
              :label="option"
              >{{ option }}</el-radio
            >
          </el-radio-group>
        </div>
  
        <!-- 多选题部分 -->
        <div v-if="multipleChoice.length">
          <h2>多选题</h2>
          <div v-for="(question, index) in multipleChoice" :key="index">
            <p>{{ question.title }}</p>
            <el-checkbox-group v-model="multipleAnswers[index]">
              <el-checkbox
                v-for="(option, optionIndex) in question.options"
                :key="optionIndex"
                :label="option"
                >{{ option }}</el-checkbox
              >
            </el-checkbox-group>
          </div>
        </div>
  
        <!-- 判断题部分 -->
        <div v-if="trueFalse.length">
          <h2>判断题</h2>
          <div v-for="(question, index) in trueFalse" :key="index">
            <p>{{ question.title }}</p>
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
  </template>
  
  <script>
  import {
    ElContainer,
    ElHeader,
    ElMain,
    ElFooter,
    ElButton,
    ElRadioGroup,
    ElRadio
  } from 'element-plus'
  
  export default {
    components: {
      ElContainer,
      ElHeader,
      ElMain,
      ElFooter,
      ElButton,
      ElRadioGroup,
      ElRadio
    },
    data() {
      return {
        singleChoice: [
          {
            title: '问题1',
            options: ['选项A', '选项B', '选项C', '选项D']
          },
          {
            title: '问题2',
            options: ['选项A', '选项B', '选项C', '选项D']
          },
          {
            title: '问题3',
            options: ['选项A', '选项B', '选项C', '选项D']
          }
        ],
        multipleChoice: [
          {
            title: '问题1（多选）',
            options: ['选项A', '选项B', '选项C', '选项D'] // 这里可以指定正确答案，但为了简单起见，我们省略了
          }
          // 更多多选题...
        ],
        trueFalse: [
          {
            title: 'lbh是yhj的爹？'
          }
          // 更多判断题...
        ],
        answers: [], // 单选题答案
        multipleAnswers: [], // 多选题答案（数组形式，存储选中的所有选项）
        trueFalseAnswers: [] // 判断题答案（true 或 false）
      }
    },
    methods: {
      submit() {
        console.log('答案：', {  
          singleChoice: this.answers,  
          multipleChoice: this.multipleAnswers,  
          trueFalse: this.trueFalseAnswers,  
        })
      }
    }
  }
  </script>
  