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
        for (var y = 0; y < map.Height; ++y) {
          for (var x = 0; x < map.Width; ++x) {
            var cell = map.Cells[y][x];

            var cellSymbol = cell.IsWalkable ? " " : "#";
            if (posx == x && posy == y) {
              cellSymbol = "@";
            }
            this.display.draw(x, y, cellSymbol, "rgb(127, 127, 127)");
          }
        }
      }
      return "";
    }
  }
};
</script>
