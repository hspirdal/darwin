<template>
	<div id="gameoverscreen">
		<h1>Game over!</h1>
		<button type="button" v-on:click="goToMenu()">OK</button>
	</div>
</template>
<script>
export default {
	methods: {
		goToMenu() {
			this.$bus.$emit("GameOver.GoToMenu", {});
		}
	}
};
</script>
