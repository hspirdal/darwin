/*eslint no-console: [off] */
const state = initialState();

function initialState() {
	return {
		gameState: "newgame"
	};
}

const getters = {
	current(state) {
		return state.gameState;
	}
};

const mutations = {
	reset(state) {
		console.log("reset gamestate");
		const s = initialState()
		Object.keys(s).forEach(key => {
			state[key] = s[key]
		});
	},
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
