import Vue from "vue";
import Vuex from "vuex";
import gamestatus from "./modules/gamestatus";
import selection from "./modules/selection";
import gamelog from "./modules/gamelog";
import equipment from "./modules/equipment";

Vue.use(Vuex);

export default new Vuex.Store({
	modules: {
		gamestatus,
		selection,
		gamelog,
		equipment
	}
});
