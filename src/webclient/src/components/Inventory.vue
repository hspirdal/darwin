<template>
	<div id="inventory" v-if="gameStarted">
		<h1>Equipment</h1>
		<ul>
			<li>Head: Empty</li>
			<li>Chest: Leather Armor</li>
			<li>Left Arm: Empty</li>
			<li>Right Arm: Rusty Shortsword</li>
			<li>Feet: Leather Boots</li>
		</ul>
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
	height: 160px;
	overflow: auto;
}
</style>
