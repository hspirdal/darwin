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
			<li v-for="item in inventoryItems">
				{{ item }}
				</li>
		</ul>
		</div>
		<h1>Active Cell [{{ this.$store.getters['gamestatus/x'] }}, {{ this.$store.getters['gamestatus/y'] }}]</h1>
		<p>A dark stone cave.</p>
		<div id="activecell_entities" v-if="activeCellCreatures.length > 0 || activeCellItems.length > 0">
			<b-form-select v-model="selectedItem" class="mb-3" :select-size="activeCellRowCount" v-on:change="selectItem">
				<optgroup label="Creatures" v-if="activeCellCreatures.length > 0">
					<option v-for="(entity) in activeCellCreatures" v-bind:value="entity.Id">{{ entity.Name }}</option>
				</optgroup>
				<optgroup label="Items" v-if="activeCellItems.length > 0">
					<option v-for="(entity) in activeCellItems" v-bind:value="entity.Id">{{ entity.Name }}</option>
				</optgroup>
			</b-form-select>
		</div>
	</div>
</template>

<script>
/*eslint vue/require-v-for-key: [off] */
/*eslint no-console: [off] */
import "bootstrap/dist/css/bootstrap.css";
import "bootstrap-vue/dist/bootstrap-vue.css";

export default {
  name: "charstatus",
  data() {
    return {
      selectedItem: null
    };
  },
  computed: {
    gameStarted: function() {
      return this.$store.getters["gamestatus/gamestarted"];
    },
    inventoryItems: function() {
      return this.$store.getters["gamestatus/player"].Inventory.Items;
    },
    activeCellItems: function() {
      console.log("Active cell item refresh");
      return this.$store.getters["gamestatus/activecellitems"];
    },
    activeCellCreatures: function() {
      console.log("Active cell creature refresh");
      return this.$store.getters["gamestatus/activecellcreatures"];
    },
    activeCellRowCount: function() {
      var totalCount = 0;
      var categoryGroupSize = 1;
      if (this.activeCellCreatures.length > 0) {
        totalCount += this.activeCellCreatures.length + categoryGroupSize;
      }
      if (this.activeCellItems.length > 0) {
        totalCount += this.activeCellItems.length + categoryGroupSize;
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
      return this.$store.getters["gamestatus/player"].Statistics.DefenseScores
        .ArmorClass;
    },
    characterHitPoints: function() {
      return this.$store.getters["gamestatus/player"].Statistics.DefenseScores
        .HitPoints;
    },
    strength: function() {
      return this.$store.getters["gamestatus/player"].Statistics.AbilityScores
        .Strength;
    },
    dexterity: function() {
      return this.$store.getters["gamestatus/player"].Statistics.AbilityScores
        .Dexterity;
    },
    constitution: function() {
      return this.$store.getters["gamestatus/player"].Statistics.AbilityScores
        .Constitution;
    },
    intelligence: function() {
      return this.$store.getters["gamestatus/player"].Statistics.AbilityScores
        .Intelligence;
    },
    wisdom: function() {
      return this.$store.getters["gamestatus/player"].Statistics.AbilityScores
        .Wisdom;
    },
    charisma: function() {
      return this.$store.getters["gamestatus/player"].Statistics.AbilityScores
        .Charisma;
    }
  },
  methods: {
    selectItem: function(item) {
      this.$store.commit("selection/setSelectedEntityId", item);
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
</style>
