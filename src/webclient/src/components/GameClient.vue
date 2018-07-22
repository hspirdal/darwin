<template>
    <div id="game" v-if="this.$parent.authenticated">
        <Network ref="network"/>
        <Input v-on:key-pressed="onKeyPressed"/>
        <Display/>
    </div>
</template>

<script>
import Network from "./Network";
import Input from "./Input";
import Display from "./Display";

export default {
  name: "Game",
  components: {
    Network,
    Input,
    Display
  },
  methods: {
    onKeyPressed: function(key) {
      if (key === "l") {
        this.$refs.network.lootAll();
        return;
      }

      if (key === " ") {
        var selectedId = this.$store.getters["selection/selectedEntityId"];
        if (selectedId != null && selectedId.length > 0) {
          this.$refs.network.loot(selectedId);
        }
      }

      var movementDirection = 0;
      if (key === "w") {
        movementDirection = 2;
      } else if (key === "s") {
        movementDirection = 3;
      } else if (key === "a") {
        movementDirection = 0;
      } else if (key === "d") {
        movementDirection = 1;
      } else {
        return;
      }
      this.$refs.network.move(movementDirection);
    }
  }
};
</script>

<style scoped>
#game {
  background-color: #ffffff;
  padding: 0px;
  margin-top: 10px;
}
</style>
