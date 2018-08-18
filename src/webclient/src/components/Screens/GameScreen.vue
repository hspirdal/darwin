<template>
	<div id="gamescreen">
		<Display/>
		<TabPanel/>
		<GameLog/>
	</div>
</template>

<script>
import Display from "../Display";
import TabPanel from "../TabPanel";
import GameLog from "../gamelog/GameLog";

export default {
	name: "gamescreen",
	components: {
		Display,
		TabPanel,
		GameLog
	},
	data() {
		return {};
	}
};
</script>
