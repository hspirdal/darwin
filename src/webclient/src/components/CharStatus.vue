<template>
	<div id="status" v-if="gameStarted">
		<h1>Character</h1>
		<p>Name: {{ characterName }}<br />
		Level: {{ characterLevel }}<br />
		Race: {{ characterRace }}<br />
		Class: {{ characterClass }}<br /></p>
		<p>Str: {{ strength }}, Dex: {{ dexterity }}, Con: {{ constitution }}, Int: {{ intelligence }}, Wis: {{ wisdom }}, Cha: {{ charisma }}<br/>
		Hit points: {{ characterHitPoints }} / {{ characterHitPoints }}<br/>
		Armor Class: {{ characterArmorClass }}</p>
		<div id="inventory" v-if="gameStarted">
		<h1>Inventory</h1>
		<ul v-if="inventoryItems.length > 0">
			<li v-for="(item, index) in inventoryItems" v-bind:key="index">
				{{ item }}
				</li>
		</ul>
		</div>
		<div id="gamestate">
		Game state: <span id="gamestateCombat" v-if="isInCombat">COMBAT</span><span id="gamestateExplore" v-else>Exploring</span>
		</div>
		<div id="actionCooldown">
		Actions: <span id="actionUnavailable" v-if="isCooldown">Unavailable</span><span id="actionReady" v-else>Ready</span>
		</div>
		<h1>Active Cell [{{ this.$store.getters['gamestatus/x'] }}, {{ this.$store.getters['gamestatus/y'] }}]</h1>
		<p>A dark stone cave.</p>
		<div id="activecell_entities" v-if="creaturesInCell.length > 0 || itemsInCell.length > 0">
			<b-form-select v-model="selectedItem" class="mb-3" :select-size="activeCellRowCount" v-on:change="selectEntity">
				<optgroup label="Creatures" v-if="creaturesInCell.length > 0">
					<option v-for="(entity, index) in creaturesInCell" v-bind:value="entity.Id" v-bind:key="index">{{ entity.Name }} ({{ inspectHealth(entity.Id) }})</option>
				</optgroup>
				<optgroup label="Items" v-if="itemsInCell.length > 0">
					<option v-for="(entity, index) in itemsInCell" v-bind:value="entity.Id" v-bind:key="index">{{ entity.Name }}</option>
				</optgroup>
			</b-form-select>
		</div>
	</div>
</template>

<script>
/*eslint no-console: [off] */
import "bootstrap/dist/css/bootstrap.css";
import "bootstrap-vue/dist/bootstrap-vue.css";

export default {
	name: "charstatus",
	data() {
		return {
			selectedItem: null,
			creaturesInCell: [],
			itemsInCell: []
		};
	},
	computed: {
		isCooldown: function() {
			return this.$store.getters["gamestatus/isCooldown"];
		},
		isInCombat: function() {
			return this.$store.getters["gamestatus/isInCombat"];
		},
		gameStarted: function() {
			return this.$store.getters["gamestatus/gamestarted"];
		},
		inventoryItems: function() {
			return this.$store.getters["gamestatus/player"].Inventory.Items;
		},
		activeCellItems: function() {
			return this.$store.getters["gamestatus/activecellitems"];
		},
		activeCellCreatures: function() {
			return this.$store.getters["gamestatus/activecellcreatures"];
		},
		activeCellRowCount: function() {
			var totalCount = 0;
			var categoryGroupSize = 1;
			if (this.creaturesInCell.length > 0) {
				totalCount += this.creaturesInCell.length + categoryGroupSize;
			}
			if (this.itemsInCell.length > 0) {
				totalCount += this.itemsInCell.length + categoryGroupSize;
			}
			return totalCount;
		},
		characterName: function() {
			return this.$store.getters["gamestatus/player"].Name;
		},
		characterRace: function() {
			return this.$store.getters["gamestatus/player"].Race;
		},
		characterClass: function() {
			return this.$store.getters["gamestatus/player"].Class;
		},
		characterLevel: function() {
			return this.$store.getters["gamestatus/player"].Level;
		},
		characterArmorClass: function() {
			return this.$store.getters["gamestatus/player"].Statistics.DefenseScores.ArmorClass;
		},
		characterHitPoints: function() {
			return this.$store.getters["gamestatus/player"].Statistics.DefenseScores.HitPoints;
		},
		strength: function() {
			return this.$store.getters["gamestatus/player"].Statistics.AbilityScores.Strength;
		},
		dexterity: function() {
			return this.$store.getters["gamestatus/player"].Statistics.AbilityScores.Dexterity;
		},
		constitution: function() {
			return this.$store.getters["gamestatus/player"].Statistics.AbilityScores.Constitution;
		},
		intelligence: function() {
			return this.$store.getters["gamestatus/player"].Statistics.AbilityScores.Intelligence;
		},
		wisdom: function() {
			return this.$store.getters["gamestatus/player"].Statistics.AbilityScores.Wisdom;
		},
		charisma: function() {
			return this.$store.getters["gamestatus/player"].Statistics.AbilityScores.Charisma;
		}
	},
	watch: {
		activeCellCreatures(creatures) {
			this.selectedItem = "";
			this.$nextTick(function() {
				this.creaturesInCell = creatures;
				if (creatures.length > 0) {
					this.preSelect(creatures[0].Id);
				}
			});
		},
		activeCellItems(items) {
			this.selectedItem = "";
			this.$nextTick(function() {
				this.itemsInCell = items;
				if (this.creaturesInCell.length == 0 && items.length > 0) {
					this.preSelect(items[0].Id);
				}
			});
		}
	},
	methods: {
		selectEntity: function(id) {
			var creature = this.$store.getters["gamestatus/activecellcreatures"].find(x => x.Id == id);
			if (creature != null) {
				this.$store.commit("selection/setSelectedCreatureId", id);
			} else {
				this.$store.commit("selection/setSelectedItemId", id);
			}
		},
		preSelect: function(entityId) {
			if (entityId) {
				this.selectedItem = entityId;
				this.selectEntity(entityId);
			}
		},

		inspectHealth: function(creatureId) {
			var creature = this.$store.getters["gamestatus/entities/knownCreatureById"](creatureId);
			if (creature != null) {
				return creature.Healthiness;
			}
			return "Unknown";
		}
	}
};
</script>
<style scoped>
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
#status select {
	overflow: auto;
}
#status h1 {
	font-size: 24px;
}
#inventory ul {
	height: 80px;
	overflow: auto;
}
#actionUnavailable {
	color: red;
}
#actionReady {
	color: green;
}
#gamestateCombat {
	color: red;
}
#gamestateExplore {
	color: green;
}
</style>
