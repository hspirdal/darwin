<template>
	<div id="tabpanel">
		<button
			v-for="tab in tabs"
			v-bind:key="tab"
			v-bind:class="['tab-button', { active: currentTab === tab }]"
			v-on:click="currentTab = tab"
		>{{ tab }}</button>

		<keep-alive>
			<component
				v-bind:is="currentTabComponent"
				class="tab"
			></component>
		</keep-alive>
	</div>
</template>

<script>
/*eslint no-console: [off] */

import CharStatus from "./CharStatus";
import Inventory from "./Inventory";
import MouseTrap from "mousetrap";

export default {
	name: "tabpanel",
	components: {
		"tab-status": CharStatus,
		"tab-inventory": Inventory
	},
	data() {
		return {
			currentTab: "Status",
			tabs: ["Status", "Inventory"]
		};
	},
	created: function() {
		MouseTrap.bind(
			"i",
			function() {
				this.currentTab = "Inventory";
			}.bind(this)
		);

		MouseTrap.bind(
			"c",
			function() {
				this.currentTab = "Status";
			}.bind(this)
		);
	},
	computed: {
		currentTabComponent: function() {
			return `tab-${this.currentTab.toLowerCase()}`;
		}
	}
};
</script>
<style scoped>
#tabpanel {
	height: 600px;
	margin-left: 600px;
	border: 1px solid;
	padding: 0px 0px;
	font-family: Luminary, Fantasy;
}
.tab-button {
	padding: 6px 10px;
	border-top-left-radius: 3px;
	border-top-right-radius: 3px;
	border: 1px solid #ccc;
	cursor: pointer;
	background: #f0f0f0;
	margin-bottom: -1px;
}
.tab-button:hover {
	background: #e0e0e0;
}
.tab-button.active {
	background: #e0e0e0;
}
.tab {
	padding: 10px;
}
</style>
