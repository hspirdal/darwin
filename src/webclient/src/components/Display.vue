<template>
    <div id="display">
        <div id="renderMap">{{ renderMap }}</div>
        <p>Position [{{ this.$store.getters['gamestatus/x'] }}, {{ this.$store.getters['gamestatus/y'] }}]</p>
    </div>
</template>

<script>
import ROT from "rot-js";

export default {
  name: "display",
  data() {
    return {
      display: null
    };
  },
  mounted: function() {
    this.display = new ROT.Display({ width: 100, height: 40 });
    document.body.appendChild(this.display.getContainer());
    this.display.getContainer().style.cssText =
      "padding-left: 0;padding-right: 0;margin-left: auto;margin-right: auto;display: block;width: 1024px;";
  },
  computed: {
    renderMap: function() {
      var map = this.$store.getters["gamestatus/map"];
      var posx = this.$store.getters["gamestatus/x"];
      var posy = this.$store.getters["gamestatus/y"];

      if (map && this.display) {
        map.VisibleCells.forEach(cell => {
          var cellSymbol = cell.IsWalkable ? " " : "#";
          var color = "rgb(127, 127, 127)";
          if (cell.Visitor) {
            cellSymbol = "P";
            color = "rgb(255, 0, 0)";
          }
          if (cell.X == posx && cell.Y == posy) {
            cellSymbol = "@";
            color = "rgb(255, 255, 255)";
          }
          this.display.draw(cell.X, cell.Y, cellSymbol, color);
        });
      }
      return "";
    }
  }
};
</script>
