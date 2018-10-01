import Common from "./common.js";

const state = initialState();

function initialState() {
	return { knownCreatures: new Map() };
}

const getters = {
	knownCreatureById: state => id => {
		return state.knownCreatures.get(id);
	}
};

const mutations = {
	reset(state) {
		Common.reset(state, initialState());
	},
	addOrUpdateKnownCreature(state, creature) {
		state.knownCreatures.set(creature.Id, creature);
	}
};

export default {
	namespaced: true,
	state,
	mutations,
	getters
};
