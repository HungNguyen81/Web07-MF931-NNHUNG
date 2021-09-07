import Vue from 'vue'
import App from './App.vue'
import axios from 'axios'
import VueAxios from 'vue-axios'
import Config from '../config/dev.env'
import ResourceVn from '../resources/resource-vn.js'
import router from './router'

Vue.config.productionTip = false  
Vue.use(VueAxios, axios)

Vue.prototype.$config = Config;
Vue.prototype.$resourceVn = ResourceVn

new Vue({
  router: router,
  render: h => h(App)  
}).$mount('#app')

