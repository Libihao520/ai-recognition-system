import request from '@/utils/request'
import { baseURL } from '@/utils/request';
import { useUserStore } from '@/stores';

//查询所有模型
export const getModelService = (params) => request.get('/Aigc/GetModelService', { params })

// 创建模型
export const PutModelService = (file, additionalData = {}) => {
  const formData = new FormData();
  formData.append('Model', file);
  for (const [key, value] of Object.entries(additionalData)) {
    formData.append(key, value);
  }
  return request.post('/Aigc/PutModelService', formData, {
    headers: {
      'Content-Type': 'multipart/form-data'
    }
  });
};
//删除
export const DelModelService = (id) =>
  request.delete('/Aigc/DelModelService', { params: { id } })

export const QuestionsAndAnswers = (q) =>
  request.get('/Aigc/QuestionsAndAnswers', { params: { q } })

export const QuestionsAndAnswersStream = (q, onMessage) => {
  const useStore = useUserStore();
  const token = useStore.token;

  // 构造完整的 API 地址，包含 token 作为查询参数
  const apiUrl = `${baseURL}/Aigc/QuestionsAndAnswersStream?q=${encodeURIComponent(q)}&token=${encodeURIComponent(token)}`;

  // 创建 EventSource 实例
  const eventSource = new EventSource(apiUrl);

  eventSource.onmessage = (event) => {
    onMessage(event.data);
  };

  eventSource.onerror = () => {
    eventSource.close();
  };

  return eventSource;
};
//
export const DelHistory = () =>
  request.delete('/Aigc/DelHistory');