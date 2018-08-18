<template>
    <div id="game" v-if="this.$parent.authenticated">
        <Network ref="network"/>
        <Input v-on:key-pressed="onKeyPressed" v-if="gameStarted"/>
				<ScreenSelector/>
    </div>
</template>

<script>
import Network from "./Network";
import Input from "./Input";
import ScreenSelector from "./Screens/ScreenSelector";

export default {
	name: "Game",
	components: {
		Network,
		Input,
		ScreenSelector
	},
	data() {
		return {
			gameStarted: false
		};
	},
	created() {
		this.$bus.$on("NewGame.CharacterSelected", $event => {
			this.$store.commit("gamestate/setCurrent", "GamePlay");
			this.$refs.network.newGame($event.selectedTemplate);
			this.gameStarted = true;
		});
	},
	methods: {
		onKeyPressed: function(key) {
			if (this.$store.getters["gamestatus/isCooldown"]) {
				return;
			}

			if (key === "l") {
				this.$refs.network.lootAll();
				return;
			}

			if (key === " ") {
				if (this.$store.getters["selection/isItemSelected"]) {
					this.$refs.network.loot(this.$store.getters["selection/selectedItemId"]);
				} else {
					this.$refs.network.attack(this.$store.getters["selection/selectedCreatureId"]);
				}
			}

			let movementDirection = 0;
			if (key === "w") {
				movementDirection = 2;
			} else if (key === "s") {
				movementDirection = 3;
			} else if (key === "a") {
				movementDirection = 0;
			} else if (key === "d") {
				movementDirection = 1;
			} else {
				return;
			}
			this.$refs.network.move(movementDirection);
		}
	}
};
</script>

<style scoped>
#game {
	background-color: #ffffff;
	padding: 0px;
	margin-top: 10px;
}
</style>
