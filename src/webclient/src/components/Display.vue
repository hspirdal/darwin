<template>
    <div id="display">
        <div id="renderMap">{{ renderMap }}</div>
        <p>Position [{{ this.$store.getters['gamestatus/x'] }}, {{ this.$store.getters['gamestatus/y'] }}]</p>
    </div>
</template>

<script>
import ROT from "rot-js";

function Create2DArray(rows) {
  var arr = [];

  for (var i = 0; i < rows; i++) {
    arr[i] = [];
  }

  return arr;
}

export default {
  name: "display",
  data() {
    return {
      display: null,
      mapInitiated: false,
      map: Create2DArray(0),
      lastVisibleCells: new Array(),
      averageClear: 0,
      averageUpdate: 0,
      averageRender: 0,
      ticksRunning: 0
    };
  },
  computed: {
    renderMap: function() {
      var map = this.$store.getters["gamestatus/map"];
      var posx = this.$store.getters["gamestatus/x"];
      var posy = this.$store.getters["gamestatus/y"];

      if (!map) {
        return;
      }

      if (!this.mapInitiated) {
        this.initMap(map.Width, map.Height);
        this.initRender(map.Width, map.Height);
        this.mapInitiated = true;
      }

      var pre_clear = performance.now();
      var cellsToRender = new Array();
      this.lastVisibleCells.forEach(cell => {
        this.map[cell.Y][cell.X].IsVisible = false;
        cellsToRender.push({ X: cell.X, Y: cell.Y });
      });

      this.lastVisibleCells = new Array();
      var post_clear = performance.now();

      var pre_update = performance.now();

      map.VisibleCells.forEach(c => {
        var cell = this.map[c.Y][c.X];
        cell.IsVisible = true;
        cell.IsExplored = true;
        cell.IsWalkable = c.IsWalkable;
        cell.Visitor = c.Visitor;
        cellsToRender.push({ X: cell.X, Y: cell.Y });

        this.lastVisibleCells.push({ X: cell.X, Y: cell.Y });
      });
      var post_update = performance.now();

      var pre_render = performance.now();
      cellsToRender.forEach(c => {
        var cell = this.map[c.Y][c.X];
        var cellSymbol = cell.IsWalkable ? " " : "#";
        var color = "rgb(127, 127, 127)";
        var backgroundColor = cell.IsVisible
          ? "rgb(255, 255, 140)"
          : "rgb(65, 65, 65)";
        if (cell.Visitor) {
          cellSymbol = "P";
          color = "rgb(255, 0, 0)";
        }
        if (cell.X == posx && cell.Y == posy) {
          cellSymbol = "@";
          color = "rgb(255, 255, 255)";
        }
        this.display.draw(cell.X, cell.Y, cellSymbol, color, backgroundColor);
      });

      var post_render = performance.now();
      this.ticksRunning++;
      this.averageClear = this.averageClear + (post_clear - pre_clear);
      this.averageUpdate = this.averageUpdate + (post_update - pre_update);
      this.averageRender = this.averageRender + (post_render - pre_render);
      console.log(
        "tick: " +
          this.ticksRunning +
          ".\nclear: " +
          this.averageClear / this.ticksRunning +
          ".\nupd: " +
          this.averageUpdate / this.ticksRunning +
          ".\nrender: " +
          this.averageRender / this.ticksRunning
      );
      return "";
    }
  },
  methods: {
    initMap(width, height) {
      this.map = Create2DArray(width);
      for (var y = 0; y < height; ++y) {
        for (var x = 0; x < width; ++x) {
          this.map[y][x] = {
            IsExplored: false,
            IsWalkable: false,
            IsVisible: false,
            Inhabitant: null,
            X: x,
            Y: y
          };
        }
      }
    },
    initRender(width, height) {
      this.display = new ROT.Display({
        width: width,
        height: height
      });
      document.body.appendChild(this.display.getContainer());
      this.display.getContainer().style.cssText =
        "padding-left: 0;padding-right: 0;margin-left: auto;margin-right: auto;display: block;width: 1024px;";
    }
  }
};
</script>
