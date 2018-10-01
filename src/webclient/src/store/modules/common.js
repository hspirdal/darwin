"use strict";

export default {
	reset(state, initialState) {
		Object.keys(initialState).forEach(key => {
			state[key] = initialState[key];
		});
	}
};
