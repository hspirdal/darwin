/*eslint no-console: [off] */
const state = {
	head: null,
	chest: { Name: "Leather Armor", Id: 9 },
	leftArm: null,
	rightArm: { Name: "Rusty Shortsword", Id: 4 },
	feet: { Name: "Leather Boots", Id: 13 }
};

const getters = {
	headSlot(state) {
		return state.head;
	},
	chestSlot(state) {
		return state.chest;
	},
	leftArmSlot(state) {
		return state.leftArm;
	},
	rightArmSlot(state) {
		return state.rightArm;
	},
	feetSlot(state) {
		return state.feet;
	}
};

const mutations = {};

export default {
	namespaced: true,
	state,
	mutations,
	getters
};
