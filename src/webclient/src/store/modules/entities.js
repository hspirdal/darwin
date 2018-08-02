const state = {
	knownCreatures: new Map()
};

const getters = {
	knownCreatureById: state => id => {
		var c = state.knownCreatures.get(id);
		return c;
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
