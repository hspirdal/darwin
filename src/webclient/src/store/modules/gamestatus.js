const state = {
	map: null,
	name: "",
	x: 0,
	y: 0
};

const getters = {
	map(state) {
		return state.map;
	},
	name(state) {
		return state.name;
	},
	x(state) {
		return state.x;
	},
	y(state) {
		return state.y;
	}
};

const mutations = {
	setStatus(state, status) {
		state.map = status.Map;
		state.name = status.Name;
		state.x = status.X;
		state.y = status.Y;
	}
};

export default {
	namespaced: true,
	state,
	mutations,
	getters
};
