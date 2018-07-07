import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";
const signalR = require("@aspnet/signalr");
Vue.config.productionTip = false;

Vue.prototype.$signalR = signalR;

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount("#app");
