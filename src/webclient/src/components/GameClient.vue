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
    onKeyPressed: function(ch) {
      var movementDirection = 0;
      if (ch === "w") {
        movementDirection = 2;
      } else if (ch === "s") {
        movementDirection = 3;
      } else if (ch === "a") {
        movementDirection = 0;
      } else if (ch === "d") {
        movementDirection = 1;
      } else {
        return;
      }
      this.$refs.network.move(movementDirection);
    }
  }
};
</script>
