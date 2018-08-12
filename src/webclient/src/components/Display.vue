<template>
    <div id="display">
			<div id="container">{{ renderMap }}</div>
		</div>
</template>

<script>
/*eslint no-console: [off] */
/*eslint vue/no-side-effects-in-computed-properties: [off] */
// ^-- Turn off temporary until I find a better way than rendering using Computed.

import ROT from "rot-js";

function Create2DArray(rows) {
	let arr = [];

	for (let i = 0; i < rows; i++) {
		arr[i] = [];
	}

	return arr;
}

export default {
	name: "display",
	data() {
		return {
			display: null,
			mapInitiated: false,
			map: Create2DArray(0),
			lastVisibleCells: new Array(),
			container: {
				width: 600,
				height: 600,
				gameResolution: {
					width: 1600,
					height: 1600
				},
				halfWidth: 300,
				halfHeight: 300
			},
			averageClear: 0,
			averageUpdate: 0,
			averageRender: 0,
			ticksRunning: 0
		};
	},
	computed: {
		renderMap: function() {
			let map = this.$store.getters["gamestatus/map"];
			let posx = this.$store.getters["gamestatus/x"];
			let posy = this.$store.getters["gamestatus/y"];

			if (!map) {
				return;
			}

			if (!this.mapInitiated) {
				this.initMap(map.Width, map.Height);
				this.initRender(map.Width, map.Height);
				this.mapInitiated = true;
			}

			this.centerPlayer(posx, posy, map.Width, map.Height);
			let cellsToRender = new Array();
			let pre_clear = performance.now();
			this.lastVisibleCells.forEach(cell => {
				this.map[cell.Y][cell.X].IsVisible = false;
				cellsToRender.push({ X: cell.X, Y: cell.Y });
			});

			this.lastVisibleCells = new Array();
			let post_clear = performance.now();

			let pre_update = performance.now();

			map.VisibleCells.forEach(c => {
				let cell = this.map[c.Y][c.X];
				cell.IsVisible = true;
				cell.IsExplored = true;
				cell.IsWalkable = c.IsWalkable;
				cell.Creatures = c.Creatures;
				cell.Items = c.Items;
				cellsToRender.push({ X: cell.X, Y: cell.Y });
				this.lastVisibleCells.push({ X: cell.X, Y: cell.Y });
			});
			let post_update = performance.now();

			let pre_render = performance.now();
			cellsToRender.forEach(c => {
				let cell = this.map[c.Y][c.X];
				let cellSymbol = cell.IsWalkable ? " " : "#";
				let color = "rgb(127, 127, 127)";
				let backgroundColor = cell.IsVisible ? "rgb(255, 255, 140)" : "rgb(65, 65, 65)";
				if (cell.Items != null && cell.Items.length > 0) {
					cellSymbol = "I";
					color = "green";
				}
				if (cell.Creatures != null && cell.Creatures.length > 0) {
					if (cell.Creatures.length > 1) {
						cellSymbol = "&";
						color = "rgb(0, 0, 255)";
					} else if (cell.Creatures[0].Type === "Player") {
						cellSymbol = "@";
						color = "rgb(255, 0, 0)";
					} else {
						let creature = cell.Creatures[0];
						switch (creature.Name) {
							case "Dire Bat":
								cellSymbol = "b";
								color = "gray";
								break;
							case "Grizzly Bear":
								cellSymbol = "B";
								color = "brown";
								break;
							case "Goblin":
								cellSymbol = "G";
								color = "rgb(32, 107, 0)";
								break;
							case "Orc Warrior":
								cellSymbol = "O";
								color = "rgb(32, 107, 0)";
						}
					}
				}
				if (cell.X == posx && cell.Y == posy) {
					cellSymbol = "@";
					color = "rgb(255, 255, 255)";
				}
				this.display.draw(cell.X, cell.Y, cellSymbol, color, backgroundColor);
			});

			let post_render = performance.now();
			this.ticksRunning++;
			this.averageClear = this.averageClear + (post_clear - pre_clear);
			this.averageUpdate = this.averageUpdate + (post_update - pre_update);
			this.averageRender = this.averageRender + (post_render - pre_render);
			// console.log(
			//   "tick: " +
			//     this.ticksRunning +
			//     ".\nclear: " +
			//     this.averageClear / this.ticksRunning +
			//     ".\nupd: " +
			//     this.averageUpdate / this.ticksRunning +
			//     ".\nrender: " +
			//     this.averageRender / this.ticksRunning
			// );
			return "";
		}
	},
	methods: {
		initMap(width, height) {
			this.map = Create2DArray(width);
			for (let y = 0; y < height; ++y) {
				for (let x = 0; x < width; ++x) {
					this.map[y][x] = {
						IsExplored: false,
						IsWalkable: false,
						IsVisible: false,
						Content: null,
						X: x,
						Y: y
					};
				}
			}
		},
		initRender(width, height) {
			this.display = new ROT.Display({
				width: width,
				height: height
			});
			let container = document.getElementById("container");
			container.appendChild(this.display.getContainer());
			this.display.getContainer().style.cssText =
				"padding-left: 0;padding-right: 0;margin-left: auto;margin-right: auto;display: block; width: " +
				this.container.gameResolution.width +
				"px; height: " +
				this.container.gameResolution.height +
				"px";
		},
		centerPlayer(posx, posy, mapWidth, mapHeight) {
			let container = document.getElementById("container");
			let normx = posx / mapWidth;
			let normy = posy / mapHeight;
			container.scrollTo(
				normx * this.container.gameResolution.width - this.container.halfWidth,
				normy * this.container.gameResolution.width - this.container.halfHeight
			);
		}
	}
};
</script>
<style scoped>
#container {
	width: 600px;
	height: 600px;
	overflow: hidden;
	border: 1px solid;
	float: left;
}
</style>
