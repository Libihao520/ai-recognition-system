<template>
    <div id="app">
      <el-container style="height: 100vh;">
        <!-- 标题栏 -->
        <el-header>
          <div class="header-content">与GPT对话窗口</div>
        </el-header>
        <!-- 主体内容区，包含对话记录和输入框 -->
        <el-main>
          <div class="chat-history-wrapper" ref="chatHistoryWrapper">
            <div class="chat-history" v-for="(message, index) in chatMessages" :key="index">
              <el-row :class="message.role === 'user'? 'user-message' : 'gpt-message'">
                <el-col :span="24">
                  <span class="message-role">{{ message.role }}</span>
                  <span class="message-content">{{ message.content }}</span>
                </el-col>
              </el-row>
            </div>
          </div>
          <el-row class="input-row" style="margin-top: 10px;">
            <el-input
              v-model="newMessage"
              placeholder="请输入消息"
              clearable
              @keyup.enter="sendMessage"
              style="width: 80%;"
            />
            <el-button type="primary" @click="sendMessage">发送</el-button>
          </el-row>
        </el-main>
      </el-container>
    </div>
  </template>
  
  <script>
  export default {
    data() {
      return {
        chatMessages: [], // 存储对话消息，每条消息包含role（角色，比如 'user'表示用户，'gpt'表示GPT回复）和content（消息内容）
        newMessage: '', // 用于绑定输入框输入的新消息
      };
    },
    methods: {
      sendMessage() {
        if (this.newMessage.trim() === '') return; // 如果输入为空则不发送
        // 将用户输入的消息添加到对话记录中，角色为'user'
        this.chatMessages.push({
          role: 'user',
          content: this.newMessage,
        });
        this.newMessage = '';
  
        // 模拟添加GPT回复，真实情况需调用后端接口获取回复
        setTimeout(() => {
          this.chatMessages.push({
            role: 'gpt',
            content: '这是模拟的GPT回复内容，你需替换为真实接口获取的数据哦',
          });
          // 消息添加后手动触发滚动到底部
          this.$nextTick(() => {
            this.$refs.chatHistoryWrapper.scrollTop = this.$refs.chatHistoryWrapper.scrollHeight;
          });
        }, 1000);
      },
    },
  };
  </script>
  
  <style scoped>
  #app {
    font-family: Arial, sans-serif;
  }
  .el-header {
    background-color: #409EFF;
    color: white;
    text-align: center;
    line-height: 50px;
  }
  .chat-history-wrapper {
    height: calc(80vh - 100px);
    overflow-y: auto;
  }
  .chat-history {
    margin-bottom: 10px;
  }
  .user-message {
    justify-content: flex-end;
  }
  .gpt-message {
    justify-content: flex-start;
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