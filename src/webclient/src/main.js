import Vue from "vue";
import App from "./App.vue";
import Network from "./components/Network.vue";
import router from "./router";
import axios from "axios";
import VueAxios from "vue-axios";
import store from "./store";
const signalR = require("@aspnet/signalr");
Vue.config.productionTip = false;

Vue.prototype.$signalR = signalR;
Vue.prototype.$network = Network;
Vue.use(VueAxios, axios);

axios.defaults.baseURL = "http://localhost:5001";

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount("#app");
