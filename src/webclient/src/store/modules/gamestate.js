/*eslint no-console: [off] */
const state = {
	gameState: "newgame"
};

const getters = {
	current(state) {
		return state.gameState;
	}
};

const mutations = {
	setCurrent(state, gameState) {
		state.gameState = gameState;
	}
};

export default {
	namespaced: true,
	state,
	mutations,
	getters
};
