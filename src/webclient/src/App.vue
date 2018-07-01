<template>
  <div id="app">
    <GameClient/>
    <Network/>
    <Display/>
  </div>
</template>
<script>
import GameClient from "./components/GameClient";
import Network from "./components/Network";
import Display from "./components/Display";
export default {
  name: "App",
  components: {
    GameClient,
    Network,
    Display
  }
};
</script>
