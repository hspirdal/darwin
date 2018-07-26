/*eslint no-console: [off] */
/*eslint prettier/prettier:[off] */
// ^-- keeps whining about tabs over spaces; go away.
const state = {
	selectedCreatureId: null,
	selectedItemId: null,
};

const getters = {
	selectedCreatureId(state) {
		return state.selectedCreatureId;
	},
	selectedItemId(state) {
		return state.selectedItemId;
	},
	isCreatureSelected(state) {
		return state.selectedCreatureId != null && state.selectedCreatureId.length > 0
	},
	isItemSelected(state) {
		return state.selectedItemId != null && state.selectedItemId.length > 0
	}
};

const mutations = {
	setSelectedCreatureId(state, selectedCreatureId) {
		state.selectedCreatureId = selectedCreatureId;
	},
	setSelectedItemId(state, selectedItemId) {
		state.selectedItemId = selectedItemId;
	}
};

export default {
	namespaced: true,
	state,
	mutations,
	getters
};
