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
		const s = initialState();
		Object.keys(s).forEach(key => {
			state[key] = s[key];
		});
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
