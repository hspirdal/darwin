<template>
	<div id="gamescreen">
		<Display/>
		<TabPanel/>
		<GameLog/>
		<StateMachine/>
	</div>
</template>

<script>
import Display from "../Display";
import TabPanel from "../TabPanel";
import GameLog from "../gamelog/GameLog";
import StateMachine from "../SimpleStateMachine";

export default {
	name: "gamescreen",
	components: {
		Display,
		TabPanel,
		GameLog,
		StateMachine
	},
	data() {
		return {};
	}
};
</script>
