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

export default {
	name: "screenselector",
	components: {
		"screen-newgame": NewGameScreen,
		"screen-gameplay": GameScreen,
		"screen-gameover": GameOverScreen
	},
	data() {
		return {
			currentScreen: "NewGame",
			screens: ["NewGame", "GamePlay", "GameOver"]
		};
	},
	computed: {
		currentScreenComponent: function() {
			var gameState = this.$store.getters["gamestate/current"];
			return `screen-${gameState.toLowerCase()}`;
		}
	}
};
</script>
