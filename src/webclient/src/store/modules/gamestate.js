/*eslint no-console: [off] */
const state = initialState();

const NewGameMenuState = "newgame";
const InGameState = "gameplay";
const GameOverState = "gameover";

function initialState() {
	return {
		gameState: "loading"
	};
}

const getters = {
	current(state) {
		return state.gameState;
	}
};

const mutations = {
	reset(state) {
		const s = initialState();
		Object.keys(s).forEach(key => {
			state[key] = s[key];
		});
	},
	setGameState(state, gameState) {
		if (gameState === "GameLobby") {
			changeGameState(state, state.gameState, NewGameMenuState);
		} else if (gameState === "InGame") {
			changeGameState(state, state.gameState, InGameState);
		} else if (gameState === "PlayerDeath") {
			changeGameState(state, state.gameState, GameOverState);
		}
	}
};

function changeGameState(state, oldState, newState) {
	if (oldState !== newState) {
		state.gameState = newState;
	}
}

export default {
	namespaced: true,
	state,
	mutations,
	getters
};
