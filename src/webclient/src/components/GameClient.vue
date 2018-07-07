<template>
    <div id="game">
        <h1>Mah Vue Client</h1>
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
      var movementDirection = 0;
      if (key === "w" || key === "ArrowUp") {
        movementDirection = 2;
      } else if (key === "s" || key === "ArrowDown") {
        movementDirection = 3;
      } else if (key === "a" || key === "ArrowLeft") {
        movementDirection = 0;
      } else if (key === "d" || key === "ArrowRight") {
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
  border: 1px solid #cccccc;
  padding: 20px;
  margin-top: 10px;
}
</style>