import Common from "./common.js";

const state = initialState();

function initialState() {
	return {
		activeTabPanelName: ""
	};
}

const getters = {
	activeTabPanelName(state) {
		return state.activeTabPanelName;
	}
};

const mutations = {
	reset(state) {
		Common.reset(state, initialState());
	},
	setActiveTab(state, tabPanelName) {
		state.activeTabPanelName = tabPanelName;
	}
};

export default {
	namespaced: true,
	state,
	mutations,
	getters
};
