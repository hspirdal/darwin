import Vue from "vue";
import Vuex from "vuex";
import gamestatus from "./modules/gamestatus";
import gamestate from "./modules/gamestate";
import selection from "./modules/selection";
import gamelog from "./modules/gamelog";
import equipment from "./modules/equipment";
import activeTabPanel from "./modules/activeTabPanel";

Vue.use(Vuex);

export default new Vuex.Store({
	modules: {
		gamestatus,
		gamestate,
		selection,
		gamelog,
		equipment,
		activeTabPanel
	},
	actions: {
		clearAll({ commit }) {
			commit("gamestatus/reset");
			commit("gamestate/reset");
			commit("selection/reset");
			commit("gamelog/reset");
			commit("equipment/reset");
			commit("activeTabPanel/reset");
		}
	}
});
