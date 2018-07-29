<template>
    <div id="display">
			<div id="container">{{ renderMap }}</div>
			<CharStatus />
			<GameLog />
		</div>
</template>

<script>
/*eslint no-console: [off] */
/*eslint vue/no-side-effects-in-computed-properties: [off] */
// ^-- Turn off temporary until I find a better way than rendering using Computed.

import CharStatus from "./CharStatus";
import GameLog from "./GameLog";
import ROT from "rot-js";

function Create2DArray(rows) {
	var arr = [];

	for (var i = 0; i < rows; i++) {
		arr[i] = [];
	}

	return arr;
}

export default {
	name: "display",
	components: {
		CharStatus,
		GameLog
	},
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
			var map = this.$store.getters["gamestatus/map"];
			var posx = this.$store.getters["gamestatus/x"];
			var posy = this.$store.getters["gamestatus/y"];

			if (!map) {
				return;
			}

			if (!this.mapInitiated) {
				this.initMap(map.Width, map.Height);
				this.initRender(map.Width, map.Height);
				this.mapInitiated = true;
			}

			this.centerPlayer(posx, posy, map.Width, map.Height);
			var cellsToRender = new Array();
			var pre_clear = performance.now();
			this.lastVisibleCells.forEach(cell => {
				this.map[cell.Y][cell.X].IsVisible = false;
				cellsToRender.push({ X: cell.X, Y: cell.Y });
			});

			this.lastVisibleCells = new Array();
			var post_clear = performance.now();

			var pre_update = performance.now();

			map.VisibleCells.forEach(c => {
				var cell = this.map[c.Y][c.X];
				cell.IsVisible = true;
				cell.IsExplored = true;
				cell.IsWalkable = c.IsWalkable;
				cell.Creatures = c.Creatures;
				cell.Items = c.Items;
				cellsToRender.push({ X: cell.X, Y: cell.Y });
				this.lastVisibleCells.push({ X: cell.X, Y: cell.Y });
			});
			var post_update = performance.now();

			var pre_render = performance.now();
			cellsToRender.forEach(c => {
				var cell = this.map[c.Y][c.X];
				var cellSymbol = cell.IsWalkable ? " " : "#";
				var color = "rgb(127, 127, 127)";
				var backgroundColor = cell.IsVisible ? "rgb(255, 255, 140)" : "rgb(65, 65, 65)";
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
						var creature = cell.Creatures[0];
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

			var post_render = performance.now();
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
			for (var y = 0; y < height; ++y) {
				for (var x = 0; x < width; ++x) {
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
			var container = document.getElementById("container");
			container.appendChild(this.display.getContainer());
			this.display.getContainer().style.cssText =
				"padding-left: 0;padding-right: 0;margin-left: auto;margin-right: auto;display: block; width: " +
				this.container.gameResolution.width +
				"px; height: " +
				this.container.gameResolution.height +
				"px";
		},
		centerPlayer(posx, posy, mapWidth, mapHeight) {
			var container = document.getElementById("container");
			var normx = posx / mapWidth;
			var normy = posy / mapHeight;
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
#status {
	height: 600px;
	margin-left: 600px;
	border: 1px solid;
	padding: 0px 20px;
	font-family: Luminary, Fantasy;
}
#status ul {
	list-style-type: none;
}
</style>
