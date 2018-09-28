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
				{{ item.Name }}
				</li>
		</ul>
	</div>
</template>

<script>
/*eslint no-console: [off] */
import MouseTrap from "mousetrap";

export default {
	data() {
		return {
			name: "Inventory"
		};
	},
	created: function() {
		this.$store.watch(
			() => {
				return this.$store.getters["activeTabPanel/activeTabPanelName"];
			},
			(newTabName, oldTabName) => {
				if (this.name === newTabName) {
					MouseTrap.bind(
						"down",
						function() {
							this.cycleSelected(1);
						}.bind(this)
					);
					MouseTrap.bind(
						"up",
						function() {
							this.cycleSelected(-1);
						}.bind(this)
					);
				}
			}
		);
	},
	computed: {
		inventoryItems: function() {
			return this.$store.getters["gamestatus/player"].Inventory.Items;
		},
		gameStarted: function() {
			return this.$store.getters["gamestatus/gameStarted"];
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
	},
	methods: {
		cycleSelected: function(relativeIndex) {
			console.log("inv cycle " + relativeIndex);
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
