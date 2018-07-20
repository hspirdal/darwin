import Vue from "vue";
import Vuex from "vuex";
import gamestatus from "./modules/gamestatus";
import selection from "./modules/selection";

Vue.use(Vuex);


/*eslint prettier/prettier:[off] */
// ^-- keeps whining about tabs over spaces; go away.
export default new Vuex.Store({
	modules: {
		gamestatus,
		selection
	}
});
