<template>
	<div id="status" v-if="gameStarted">
		<h3>Character</h3>
		<p>Name: {{ this.$store.getters['gamestatus/player'].Name }}<br />
		Level: 1<br />
		Race: Human<br />
		Class: Fighter<br /></p>
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
		<div id="activecell_creatures" v-if="activeCellCreatures.length > 0">
		<h4>Creatures</h4>
		<ul>
			<li v-for="entity in activeCellCreatures">
				{{ entity.Name }}
			</li>
		</ul>
		</div>
		<div id="activecell_items" v-if="activeCellItems.length > 0">
		<h4>Items</h4>
		<ul>
			<li v-for="entity in activeCellItems">
				{{ entity.Name }}
			</li>
		</ul>
		</div>
		<h3>Commands</h3>
		WASD to move | Space to attack creature in cell | L to loot all in cell
	</div>
</template>

<script>
/*eslint vue/require-v-for-key: [off] */

export default {
  name: "charstatus",
  data() {
    return {
      gameStarted: true
    };
  },
  computed: {
    inventoryItems: function() {
      return this.$store.getters["gamestatus/player"].Inventory.Items;
    },
    activeCellItems: function() {
      var map = this.$store.getters["gamestatus/map"];
      var posx = this.$store.getters["gamestatus/x"];
      var posy = this.$store.getters["gamestatus/y"];
      var items = new Array();
      if (!map) {
        return items;
      }

      var cell = map.VisibleCells.find(c => c.X == posx && c.Y == posy);
      cell.Content.forEach(entity => {
        if (entity.Type === "Item") {
          items.push(entity);
        }
      });
      return items;
    },
    activeCellCreatures: function() {
      var map = this.$store.getters["gamestatus/map"];
      var player = this.$store.getters["gamestatus/player"];
      var posx = this.$store.getters["gamestatus/x"];
      var posy = this.$store.getters["gamestatus/y"];
      var creatures = new Array();
      if (!map) {
        return creatures;
      }

      var cell = map.VisibleCells.find(c => c.X == posx && c.Y == posy);
      cell.Content.forEach(entity => {
        if (entity.Type === "Player" && entity.Name !== player.Name) {
          creatures.push(entity);
        }
      });
      return creatures;
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
</style>
