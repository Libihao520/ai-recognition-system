<template>
  <page-containel>
    <template #extra>
      <div class="stream-switch-container">
        流式返回：
        <el-switch v-model="startStream" inline-prompt active-text="是" inactive-text="否" />
      </div>
    </template>
    <div id="app">
      <el-container style="height: 80%">
        <!-- 标题栏 -->
        <el-header>
          <div class="header-content">与讯飞星火Spark Max对话窗口</div>
        </el-header>
        <!-- 主体内容区，包含对话记录和输入框 -->
        <el-main>
          <div class="chat-history-wrapper" ref="chatHistoryWrapper">
            <div class="chat-history" v-for="(message, index) in chatMessages" :key="index">
              <div :class="message.role === 'user' ? 'user-message' : 'gpt-message'">
                <el-avatar :src="gpt" v-if="message.role === 'gpt'" />
                <div :class="message.role === 'user' ? 'user-content' : 'gpt-content'">
                  <!-- <span class="message-role">{{ message.role }}</span> -->
                  <span class="message-content">{{ message.content }}</span>
                </div>
                <div class="avatar-container" v-if="message.role === 'user'">
                  <el-avatar :src="userStore.user.photo || avatar" />
                </div>
              </div>
            </div>
          </div>
          <el-row class="input-row" style="margin-top: 15px">
            <el-input
              v-model="newMessage"
              placeholder="请输入消息"
              clearable
              @keyup.enter="sendMessage"
              style="width: 80%"
            />

            <el-button type="primary" @click="sendMessage">发送</el-button>
          </el-row>
        </el-main>
      </el-container>
    </div>
  </page-containel>
</template>

<script setup>
import { ref, nextTick, reactive } from 'vue'
import { QuestionsAndAnswers, QuestionsAndAnswersStream } from '../../api/Aigc'
import { useUserStore } from '@/stores'
import { onMounted } from 'vue'
import gpt from '@/assets/gpt.png'

// 定义响应式数据
const chatMessages = ref([]) // 存储对话消息
const newMessage = ref('') // 输入框绑定的新消息
const chatHistoryWrapper = ref(null) // 用于滚动操作的 DOM 引用
const userStore = useUserStore()
const startStream = ref(false)

onMounted(() => {
  userStore.getUser()
})

// 定义方法
const sendMessage = async () => {
  if (newMessage.value.trim() === '') return // 如果输入为空则不发送

  // 添加用户消息
  chatMessages.value.push({
    role: 'user',
    content: newMessage.value
  })

  // 清空输入框
  const userMessage = newMessage.value

  if (startStream.value === false) {
    //非流式
    const res = await QuestionsAndAnswers(newMessage.value)
    chatMessages.value.push({
      role: 'gpt',
      content: res.data.data
    })
    // 滚动到底部
    nextTick(() => {
      if (chatHistoryWrapper.value) {
        chatHistoryWrapper.value.scrollTop = chatHistoryWrapper.value.scrollHeight
      }
    })
  } else {
    //流式
    const gptMessage = reactive({ role: 'gpt', content: '' })
    chatMessages.value.push(gptMessage)

    // 发送 SSE 请求
    const eventSource = QuestionsAndAnswersStream(userMessage, (data) => {
      // 逐字显示消息
      let index = 0
      const intervalId = setInterval(() => {
        if (index < data.length) {
          gptMessage.content += data[index] // 逐个字符添加到内容中
          index++

          // 确保 DOM 更新后滚动到底部
          nextTick(() => {
            if (chatHistoryWrapper.value) {
              chatHistoryWrapper.value.scrollTop = chatHistoryWrapper.value.scrollHeight
            }
          })
        } else {
          clearInterval(intervalId) // 停止定时器
        }
      }, 50) // 每 50 毫秒显示一个字符
    })
    // 当 SSE 连接关闭时，清理资源
    eventSource.onerror = () => {
      eventSource.close()
    }
  }
  newMessage.value = ''
}
</script>

<style scoped>
#app {
  font-family: Arial, sans-serif;
}

.el-header {
  background-color: #409eff;
  color: white;
  text-align: center;
  line-height: 50px;
}

.chat-history-wrapper {
  height: calc(80vh - 240px);
  overflow-y: auto;
}

.chat-history {
  margin-bottom: 10px;
}

.user-message {
  display: flex;
  justify-content: flex-end; /* 用户消息靠右 */
}

.gpt-message {
  display: flex;
  justify-content: flex-start; /* 系统消息靠左 */
}

.user-content {
  background-color: #409eff;
  color: white;
  padding: 10px;
  border-radius: 10px;
  display: inline-block;
  max-width: 70%;
  text-align: left; /* 用户消息内容左对齐 */
}

.gpt-content {
  background-color: #f0f0f0;
  color: black;
  padding: 10px;
  border-radius: 10px;
  display: inline-block;
  max-width: 70%;
  text-align: left; /* 系统消息内容左对齐 */
}

.message-role {
  font-weight: bold;
  margin-right: 5px;
}

.input-row {
  justify-content: center;
  align-items: center;
}
</style>