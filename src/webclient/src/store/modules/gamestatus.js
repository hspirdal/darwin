/*eslint no-console: [off] */

import Moment from "moment";

const state = {
	map: null,
	player: { Name: "", Inventory: { Items: [] } },
	gameStarted: false,
	activeCellCreatures: [{}],
	activeCellItems: [{}],
	nextActionAvailableUtc: Moment.utc(Moment.now()),
	isCooldown: false,
	isInCombat: false,
	x: 0,
	y: 0
};

const getters = {
	map(state) {
		return state.map;
	},
	player(state) {
		return state.player;
	},
	x(state) {
		return state.x;
	},
	y(state) {
		return state.y;
	},
	activecellcreatures(state) {
		return state.activeCellCreatures;
	},
	activecellitems(state) {
		return state.activeCellItems;
	},
	gamestarted(state) {
		return state.gameStarted;
	},
	nextActionAvailableUtc(state) {
		return state.nextActionAvailableUtc;
	},
	isCooldown(state) {
		return state.isCooldown;
	},
	isInCombat(state) {
		return state.isInCombat;
	}
};

const mutations = {
	setStatus(state, status) {
		state.map = status.Map;
		state.player = status.Player;
		state.x = status.X;
		state.y = status.Y;
		state.isInCombat = status.IsInCombat;
		state.nextActionAvailableUtc = Moment.utc(status.NextActionAvailableUtc);
		state.isCooldown = Moment.utc() < state.nextActionAvailableUtc;

		if (!state.gameStarted) {
			state.gameStarted = true;
		}

		// Only update if either items or creatures in cell has changed.
		let cell = status.Map.VisibleCells.find(c => c.X == status.X && c.Y == status.Y);
		if (cell == null || (cell.Items == null && cell.Creatures == null)) {
			return;
		}

		let creatures = new Array();
		cell.Creatures.forEach(c => {
			creatures.push(c);
		});

		let items = new Array();
		cell.Items.forEach(i => {
			items.push(i);
		});

		let creaturesChanged = arrayChanged(state.activeCellCreatures, creatures);
		let itemsChanged = arrayChanged(state.activeCellItems, items);

		if (creaturesChanged || itemsChanged) {
			console.log("Refresh entity state");
			state.activeCellCreatures = creatures;
			state.activeCellItems = items;
		}
	}
};

function arrayChanged(oldArray, newArray) {
	if (oldArray.length != newArray.length) {
		return true;
	}

	let found = false;
	oldArray.forEach(o => {
		newArray.forEach(n => {
			if (o.Id == n.Id) {
				found = true;
			}
		});
		if (!found) {
			return true;
		}
	});
	return false;
}

export default {
	namespaced: true,
	state,
	mutations,
	getters
};
