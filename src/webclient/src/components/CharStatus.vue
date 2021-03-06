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
import MouseTrap from "mousetrap";

export default {
	data() {
		return {
			name: "Status",
			selectedItem: null,
			creaturesInCell: [],
			itemsInCell: []
		};
	},
	created: function() {
		this.$store.watch(
			() => {
				return this.$store.getters["activeTabPanel/activeTabPanelName"];
			},
			newTabName => {
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
		isCooldown: function() {
			return this.$store.getters["gamestatus/isCooldown"];
		},
		isInCombat: function() {
			return this.$store.getters["gamestatus/isInCombat"];
		},
		gameStarted: function() {
			return this.$store.getters["gamestatus/gameStarted"];
		},
		activeCellItems: function() {
			return this.$store.getters["gamestatus/activeCellItems"];
		},
		activeCellCreatures: function() {
			return this.$store.getters["gamestatus/activeCellCreatures"];
		},
		activeCellRowCount: function() {
			let totalCount = 0;
			let categoryGroupSize = 1;
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
			let creature = this.$store.getters["gamestatus/activeCellCreatures"].find(x => x.Id == id);
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
		cycleSelected: function(relativeIndex) {
			let entityIds = this.$store.getters["gamestatus/activeEntityIds"];
			let index = entityIds.findIndex(id => id == this.selectedItem);
			let newIndex = index + relativeIndex;
			if (newIndex < 0) {
				newIndex = entityIds.length - 1;
			} else if (newIndex >= entityIds.length) {
				newIndex = 0;
			}

			this.preSelect(entityIds[newIndex]);
		},
		inspectHealth: function(creatureId) {
			let creature = this.$store.getters["gamestatus/entities/knownCreatureById"](creatureId);
			if (creature != null) {
				return creature.Healthiness;
			}
			return "Unknown";
		}
	}
};
</script>
<style scoped>
#status ul {
	list-style-type: none;
}
#status select {
	overflow: auto;
}
#status h1 {
	font-size: 24px;
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
