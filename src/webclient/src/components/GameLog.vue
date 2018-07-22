<template>
	<div id="gamelog" v-if="gameStarted">
	<ul v-if="gamelog.length > 0">
		<li v-for="log in gamelog">
		{{ log.Message }}
		</li>
	</ul>
	</div>
</template>

<script>
/*eslint vue/require-v-for-key: [off] */
/*eslint no-console: [off] */
/*eslint vue/no-side-effects-in-computed-properties: [off] */
// ^-- Turn off temporary until I find a better way than rendering using Computed.

export default {
  name: "gamelog",
  data() {
    return {
      feedbackHistory: []
    };
  },
  computed: {
    gameStarted: function() {
      return this.$store.getters["gamestatus/gamestarted"];
    },
    gamelog: function() {
      var feedback = this.$store.getters["gamestatus/feedback"];
      feedback.forEach(f => {
        this.feedbackHistory.push(f);
      });
      return this.feedbackHistory.slice().reverse();
    }
  }
};
</script>
<style scoped>
#gamelog {
  height: 100px;
  margin-left: 0px;
  overflow: auto;
  border: 1px solid;
  padding: 0px 20px;
  font-family: Luminary, Fantasy;
}
#gamelog ul {
  list-style-type: none;
  margin: 0;
  padding: 0;
}
</style>
