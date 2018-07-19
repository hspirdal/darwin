<template>
	<div id="status" v-if="gameStarted">
		<h3>Character</h3>
		<p>Name: {{ characterName }}<br />
		Level: {{ characterLevel }}<br />
		Race: {{ characterRace }}<br />
		Class: {{ characterClass }}<br /></p>
		<p>Str: {{ strength }}, Dex: {{ dexterity }}, Con: {{ constitution }}, Int: {{ intelligence }}, Wis: {{ wisdom }}, Cha: {{ charisma }}</p>
		<p>Hit points: {{ characterHitPoints }} / {{ characterHitPoints }}<br/>
		Armor Class: {{ characterArmorClass }}</p>
		<div id="inventory" v-if="gameStarted">
		<h3>Inventory</h3>
		<ul v-if="inventoryItems.length > 0">
			<li v-for="item in inventoryItems">
				{{ item }}
				</li>
		</ul>
		</div>
		<h3>Active Cell [{{ this.$store.getters['gamestatus/x'] }}, {{ this.$store.getters['gamestatus/y'] }}]</h3>
		<p>A dark stone cave.</p>
		<div id="activecell_entities" v-if="activeCellCreatures.length > 0 || activeCellItems.length > 0">
			<b-form-select v-model="selectedItem" class="mb-3" :select-size="activeCellRowCount" v-on:change="selectItem">
				<optgroup label="Creatures" v-if="activeCellCreatures.length > 0">
					<option v-for="(entity, index) in activeCellCreatures" v-bind:value="entity.Id">{{ entity.Name }}</option>
				</optgroup>
				<optgroup label="Items" v-if="activeCellItems.length > 0">
					<option v-for="(entity, index) in activeCellItems" v-bind:value="entity.Id">{{ entity.Name }}</option>
				</optgroup>
			</b-form-select>
			<div>Selected: <strong>{{ selectedItem }}</strong></div>
		</div>
		<h3>Commands</h3>
		WASD to move | Space to attack creature in cell | L to loot all in cell
	</div>
</template>

<script>
/*eslint vue/require-v-for-key: [off] */
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
      return this.$store.getters["gamestatus/player"].Statistics.Race;
    },
    characterClass: function() {
      return this.$store.getters["gamestatus/player"].Statistics.Class;
    },
    characterLevel: function() {
      return this.$store.getters["gamestatus/player"].Statistics.Level;
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
      console.log("Selected: " + item);
    }
  }
};
</script>
<style scoped>
#status {
  width: 340px;
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
</style>
