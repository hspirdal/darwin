import Vue from "vue";
import App from "./App.vue";
import store from "./store";
const signalR = require("@aspnet/signalr");
//const rot = require("rot-js");
//import rot from "./rot-js";
Vue.config.productionTip = false;

Vue.prototype.$signalR = signalR;
//Vue.prototype.$rot = rot;

new Vue({
  store,
  render: h => h(App)
}).$mount("#app");
