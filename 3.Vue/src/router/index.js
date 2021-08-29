import Vue from 'vue'
import VueRouter from 'vue-router'
import EmployeePage from '../components/pages/PageEmployee'
// import CustomerPage from '../components/pages/PageCustomer'

Vue.use(VueRouter)

const routes = [
  {path: '/', component: EmployeePage}
];

const router = new VueRouter({
  routes: routes,
  mode: 'history'
}); 

export default router