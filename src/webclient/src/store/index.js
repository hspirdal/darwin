import Vue from "vue";
import Vuex from "vuex";
import gamestatus from "./modules/gamestatus";
import selection from "./modules/selection";
import BootstrapVue from "bootstrap-vue";

Vue.use(BootstrapVue);
Vue.use(Vuex);

export default new Vuex.Store({
	modules: {
		gamestatus,
		selection
	}
});
