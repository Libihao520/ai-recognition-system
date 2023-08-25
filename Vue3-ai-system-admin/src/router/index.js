import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/login',
      component: () => import('@/views/login/LoginPage.vue')
    }, //登录页
    {
      path: '/',
      //架子
      component: () => import('@/views/layout/LayoutContainer.vue'),
      redirect: '/sjdp/yolosjdp',
      children: [
        {
          path: '/sjdp/yolosjdp',
          component: () => import('@/views/sjdp/yoloSjdp.vue')
        },
        {
          path: '/yolo/yolocar',
          component: () => import('@/views/yolo/yoloCar.vue')
        },
        {
          path: '/yolo/yolopkq',
          component: () => import('@/views/yolo/yoloPkq.vue')
        },
        {
          path: '/yolo/yolorecognition',
          component: () => import('@/views/yolo/yoloRecognition.vue')
        }
      ]
    }
  ]
})

export default router
