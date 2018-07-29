<template>
	<div id="gamelog" v-if="gameStarted">
	<ul v-if="formattedMessages.length > 0">
		<li v-html="log" v-for="log in formattedMessages">
			{{ log }}
		</li>
	</ul>
	</div>
</template>

<script>
/*eslint vue/require-v-for-key: [off] */
/*eslint no-console: [off] */
/*eslint vue/no-side-effects-in-computed-properties: [off] */
// ^-- Turn off temporary until I find a better way than rendering using Computed.

const MessageTopic = {
	Nothing: 0,
	AttackedBy: 1,
	Attacking: 2,
	KilledBy: 3,
	SuccessfulHitBy: 4,
	FailedHitBy: 5,
	CombatantFlees: 6,
	CombatantDissapears: 7,
	CombatantDies: 8,
	ExperienceGain: 9,
	MovementFailed: 10
};

import GameLogFormatter from "./gameLogFormatter.js";

export default {
	name: "gamelog",
	data() {
		return {
			formattedMessagesHistory: [],
			player: [{}]
		};
	},
	mounted: function() {
		this.player = this.$store.getters["gamestatus/player"];
	},
	computed: {
		gameStarted: function() {
			return this.$store.getters["gamestatus/gamestarted"];
		},
		formattedMessages: function() {
			var message = this.$store.getters["gamelog/lastMessageAdded"];
			var formattedMessage = "";
			if (message.Topic === MessageTopic.SuccessfulHitBy) {
				formattedMessage = GameLogFormatter.formatSuccessfulHit(this.player, message.Payload);
			} else if (message.Topic === MessageTopic.FailedHitBy) {
				formattedMessage = GameLogFormatter.formatFailedHit(this.player, message.Payload);
			} else if (message.Topic === MessageTopic.Attacking) {
				formattedMessage = GameLogFormatter.formatAttacking();
			} else if (message.Topic === MessageTopic.CombatantDies) {
				formattedMessage = GameLogFormatter.formatKilledOther(message.Payload);
			} else if (message.Topic === MessageTopic.KilledBy) {
				formattedMessage = GameLogFormatter.formatKilledBy(this.player, message.Payload);
			} else if (message.Topic === MessageTopic.ExperienceGain) {
				formattedMessage = GameLogFormatter.formatExperienceGain(200);
			} else if (message.Topic === MessageTopic.MovementFailed) {
				formattedMessage = GameLogFormatter.formatMovementFailed(message.Payload);
			}

			if (formattedMessage.length > 0) {
				this.formattedMessagesHistory.push(formattedMessage);
			}

			return this.formattedMessagesHistory.slice().reverse();
		}
	},
	watch: {
		gameStarted() {
			this.player = this.$store.getters["gamestatus/player"];
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
#success {
	color: green;
}
#failure {
	color: red;
}
#self {
	color: green;
}
#other {
	color: red;
}
</style>
