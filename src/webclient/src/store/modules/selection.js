/*eslint no-console: [off] */
const state = initialState();

function initialState() {
	return {
		selectedCreatureId: null,
		selectedItemId: null
	};
}

const getters = {
	selectedCreatureId(state) {
		return state.selectedCreatureId;
	},
	selectedItemId(state) {
		return state.selectedItemId;
	},
	isCreatureSelected(state) {
		return state.selectedCreatureId != null && state.selectedCreatureId.length > 0;
	},
	isItemSelected(state) {
		return state.selectedItemId != null && state.selectedItemId.length > 0;
	}
};

const mutations = {
	reset(state) {
		const s = initialState();
		Object.keys(s).forEach(key => {
			state[key] = s[key];
		});
	},
	setSelectedCreatureId(state, selectedCreatureId) {
		state.selectedCreatureId = selectedCreatureId;
		state.selectedItemId = "";
	},
	setSelectedItemId(state, selectedItemId) {
		state.selectedItemId = selectedItemId;
		state.selectedCreatureId = "";
	}
};

export default {
	namespaced: true,
	state,
	mutations,
	getters
};
