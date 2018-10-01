/*eslint no-console: [off] */
import Common from "./common.js";

const state = initialState();

function initialState() {
	return {
		gameMessages: [],
		lastMessageAdded: []
	};
}

const getters = {
	gameMessages(state) {
		return state.gameMessages;
	},
	lastMessageAdded(state) {
		return state.lastMessageAdded;
	}
};

const mutations = {
	reset(state) {
		Common.reset(state, initialState());
	},
	appendMessage(state, message) {
		state.gameMessages.push(message);
		state.lastMessageAdded = message;
		console.log(`${message.SequenceNumber}: ${message.Topic}`);
	}
};

export default {
	namespaced: true,
	state,
	mutations,
	getters
};
