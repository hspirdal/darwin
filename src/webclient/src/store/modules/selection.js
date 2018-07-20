/*eslint no-console: [off] */
/*eslint prettier/prettier:[off] */
// ^-- keeps whining about tabs over spaces; go away.
const state = {
	selectedEntityId: null,
};

const getters = {
	selectedEntityId(state) {
		return state.selectedEntityId;
	}
};

const mutations = {
	setSelectedEntityId(state, selectedEntityId) {
		state.selectedEntityId = selectedEntityId;
	}
};

export default {
	namespaced: true,
	state,
	mutations,
	getters
};
