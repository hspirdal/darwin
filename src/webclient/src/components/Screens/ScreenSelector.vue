<template>
	<div id="screenselector">
		<slot name="network"></slot>
		<component v-bind:is="currentScreenComponent"></component>
	</div>
</template>

<script>
/*eslint no-console: [off] */

import GameOverScreen from "./GameOverScreen";
import NewGameScreen from "./NewGameScreen";
import GameScreen from "./GameScreen";
import LoadingScreen from "./LoadingScreen";

export default {
	name: "screenselector",
	components: {
		"screen-newgame": NewGameScreen,
		"screen-gameplay": GameScreen,
		"screen-gameover": GameOverScreen,
		"screen-loading": LoadingScreen
	},
	data() {
		return {
			currentScreen: "Loading",
			screens: ["Loading", "NewGame", "GamePlay", "GameOver"]
		};
	},
	created: function() {
		this.$store.watch(
			() => {
				return this.$store.getters["gamestate/current"];
			},
			(previousState, newState) => {
				this.currentScreen = `screen-${newState.toLowerCase()}`;
			}
		);
	},
	computed: {
		currentScreenComponent: function() {
			var gameState = this.$store.getters["gamestate/current"];
			return `screen-${gameState.toLowerCase()}`;
		}
	}
};
</script>
