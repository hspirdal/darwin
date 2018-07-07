<template>
    <div>
        <h1>Mah Vue Client</h1>
        <Network ref="network"/>
        <Input v-on:key-pressed="onKeyPressed"/>
    </div>
</template>

<script>
import Network from "./Network";
import Input from "./Input";

export default {
  components: {
    Network,
    Input
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
