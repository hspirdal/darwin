<template>
	<div id="inventory" v-if="gameStarted">
		<h1>Equipment</h1>
		<ul>
			<li>Head: {{ headSlot }}</li>
			<li>Chest: {{ chestSlot }}</li>
			<li>Left Arm: {{ leftArmSlot }}</li>
			<li>Right Arm: {{ rightArmSlot }}</li>
			<li>Feet: {{ feetSlot }}</li>
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
		},
		headSlot: function() {
			let headSlot = this.$store.getters["equipment/headSlot"];
			return headSlot ? headSlot.Name : "Empty";
		},
		chestSlot: function() {
			let chestSlot = this.$store.getters["equipment/chestSlot"];
			return chestSlot ? chestSlot.Name : "Empty";
		},
		leftArmSlot: function() {
			let leftArmSlot = this.$store.getters["equipment/leftArmSlot"];
			return leftArmSlot ? leftArmSlot.Name : "Empty";
		},
		rightArmSlot: function() {
			let rightArmSlot = this.$store.getters["equipment/rightArmSlot"];
			return rightArmSlot ? rightArmSlot.Name : "Empty";
		},
		feetSlot: function() {
			let feetSlot = this.$store.getters["equipment/feetSlot"];
			return feetSlot ? feetSlot.Name : "Empty";
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
