<template>
	<div id="inventory" v-if="gameStarted">
		<h1>Inventory</h1>
		<ul v-if="inventoryItems.length > 0">
			<li v-for="(item, index) in inventoryItems" v-bind:key="index">
				{{ item }}
				</li>
		</ul>
	</div>
</template>

<script>
/*eslint no-console: [off] */

export default {
	name: "inventory",
	data() {
		return {};
	},
	computed: {
		inventoryItems: function() {
			return this.$store.getters["gamestatus/player"].Inventory.Items;
		},
		gameStarted: function() {
			return this.$store.getters["gamestatus/gamestarted"];
		}
	}
};
</script>
<style scoped>
#inventory ul {
	list-style-type: none;
}
#inventory select {
	overflow: auto;
}
#inventory h1 {
	font-size: 24px;
}
#inventory ul {
	height: 200px;
	overflow: auto;
}
</style>
