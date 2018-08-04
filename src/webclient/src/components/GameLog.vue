<template>
	<div id="gamelog" v-if="gameStarted">
	<ul v-if="formattedMessagesHistory.length > 0">
		<li v-html="log.text" v-for="(log, index) in formattedMessagesHistory" v-bind:key="index">
			{{ log.text }}
		</li>
	</ul>
	</div>
</template>

<script>
/*eslint no-console: [off] */

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
			} else if (message.Topic === MessageTopic.AttackedBy) {
				formattedMessage = GameLogFormatter.formatAttackedBy();
			} else if (message.Topic === MessageTopic.CombatantDies) {
				formattedMessage = GameLogFormatter.formatKilledOther(message.Payload);
			} else if (message.Topic === MessageTopic.KilledBy) {
				formattedMessage = GameLogFormatter.formatKilledBy(this.player, message.Payload);
			} else if (message.Topic === MessageTopic.ExperienceGain) {
				formattedMessage = GameLogFormatter.formatExperienceGain(200);
			} else if (message.Topic === MessageTopic.MovementFailed) {
				formattedMessage = GameLogFormatter.formatMovementFailed(message.Payload);
			}

			return { text: formattedMessage, sequenceNumber: message.SequenceNumber };
		}
	},
	watch: {
		gameStarted() {
			this.player = this.$store.getters["gamestatus/player"];
		},
		formattedMessages(formattedMessage) {
			if (formattedMessage.text.length > 0) {
				formattedMessage.text += " seq: " + formattedMessage.sequenceNumber;
				this.formattedMessagesHistory.push(formattedMessage);

				this.ensureSortingOrder(formattedMessage);

				this.$nextTick(function() {
					this.scrollToEnd();
				});
			}
		}
	},
	methods: {
		scrollToEnd: function() {
			var container = document.getElementById("gamelog");
			container.scrollTop = container.scrollHeight;
		},
		ensureSortingOrder(lastAddedMessage) {
			let currentLength = this.formattedMessagesHistory.length;
			if (currentLength > 1) {
				let previousElement = this.formattedMessagesHistory[currentLength - 2];
				if (previousElement.sequenceNumber > lastAddedMessage.sequenceNumber) {
					this.formattedMessagesHistory.sort((a, b) => a.sequenceNumber - b.sequenceNumber);
				}
			}
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
#gamelog >>> #success {
	color: green;
}
#gamelog >>> #failure {
	color: red;
}
#gamelog >>> #self {
	color: green;
}
#gamelog >>> #other {
	color: red;
}
#gamelog >>> #damage {
	color: red;
}
#gamelog >>> #xpgain {
	color: purple;
}
#gamelog >>> #failedaction {
	color: blue;
}
</style>
