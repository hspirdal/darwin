import Vue from "vue";
import Vuex from "vuex";
import gamestatus from "./modules/gamestatus";
import BootstrapVue from "bootstrap-vue";

Vue.use(BootstrapVue);
Vue.use(Vuex);

export default new Vuex.Store({
	modules: {
		gamestatus
	}
});
