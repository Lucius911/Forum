import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import vuetify from './plugins/vuetify'


import 'vuetify/styles'
import '@mdi/font/css/materialdesignicons.css'

createApp(App).use(vuetify).use(store).use(router).mount('#app')
