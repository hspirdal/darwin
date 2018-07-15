const state = {
	map: null,
	player: { Name: "", Inventory: { Items: [] } },
	x: 0,
	y: 0
};

const getters = {
	map(state) {
		return state.map;
	},
	player(state) {
		return state.player;
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
		state.player = status.Player;
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
