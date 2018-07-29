/*eslint no-console: [off] */
const state = {
	gameMessages: [],
	lastMessageAdded: []
};

const getters = {
	gameMessages(state) {
		return state.gameMessages;
	},
	lastMessageAdded(state) {
		return state.lastMessageAdded;
	}
};

const mutations = {
	appendMessage(state, message) {
		state.gameMessages.push(message);
		state.lastMessageAdded = message;
	}
};

export default {
	namespaced: true,
	state,
	mutations,
	getters
};
