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
		const s = initialState()
		Object.keys(s).forEach(key => {
			state[key] = s[key]
		});
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
