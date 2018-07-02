<template>
  <div id="app">
    <GameClient/>
    <Display/>
  </div>
</template>
<script>
import GameClient from "./components/GameClient";
import Display from "./components/Display";
export default {
  name: "App",
  components: {
    GameClient,
    Display
  }
};
</script>
