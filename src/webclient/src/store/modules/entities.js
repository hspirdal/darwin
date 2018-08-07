const state = {
	knownCreatures: new Map()
};

const getters = {
	knownCreatureById: state => id => {
		return state.knownCreatures.get(id);
	}
};

const mutations = {
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
