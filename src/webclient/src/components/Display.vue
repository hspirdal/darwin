<template>
    <div>
        <p>{{ renderMap }}</p>
        <p>Position [{{ this.$store.getters['gamestatus/x'] }}, {{ this.$store.getters['gamestatus/y'] }}]</p>
    </div>
</template>

<script>
import ROT from "rot-js";

export default {
  data() {
    return {
      display: null
    };
  },
  mounted: function() {
    this.display = new ROT.Display({ width: 15, height: 30 });
    document.body.appendChild(this.display.getContainer());
  },
  computed: {
    renderMap: function() {
      var map = this.$store.getters["gamestatus/map"];
      var posx = this.$store.getters["gamestatus/x"];
      var posy = this.$store.getters["gamestatus/y"];
      if (map && this.display) {
        map.VisibleCells.forEach(cell => {
          var cellSymbol = cell.IsWalkable ? " " : "#";
          if (cell.X == posx && cell.Y == posy) {
            cellSymbol = "@";
          }
          this.display.draw(cell.X, cell.Y, cellSymbol, "rgb(127, 127, 127)");
        });
      }
      return "";
    }
  }
};
</script>
