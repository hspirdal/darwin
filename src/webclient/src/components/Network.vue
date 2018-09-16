<template>
<div>
</div>
</template>
<script>
function createRequest(name, payload) {
	let credentials = JSON.parse(sessionStorage.getItem("credentials"));
	console.log("Creating request: " + credentials.userId + " " + credentials.sessionId);
	return JSON.stringify({
		RequestName: name,
		UserId: credentials.userId,
		SessionId: credentials.sessionId,
		Payload: payload
	});
}

export default {
	/*eslint no-console: [off] */
	data() {
		return {
			connection: null,
			count: 0
		};
	},
	created: function() {
		this.connection = new this.$signalR.HubConnectionBuilder()
			.withUrl("http://127.0.0.1:5000/ws")
			.configureLogging(this.$signalR.LogLevel.Error)
			.build();
	},
	mounted: function() {
		this.connection.on("direct", data => {
			let response = JSON.parse(data);
			if (response.Type === "GameStatus") {
				let status = JSON.parse(response.Payload);
				if (status) {
					this.$store.commit("gamestatus/setStatus", status);
				}
			} else if (response.Type === "GameState") {
				let response = JSON.parse(data);
				let gameState = response.Message;
				this.$store.commit("gamestate/setGameState", gameState);
			} else if (response.Type === "NotAuthenticated") {
				// Server might have restarted and purged active session list.
				console.log(`${response.Message}\nPayload: ${response.Payload}`);
				sessionStorage.removeItem("credentials");
				location.reload();
			} else {
				console.log(`${response.Message}\nPayload: ${response.Payload}`);
			}
		});

		this.connection.on("gamemessage", data => {
			let response = JSON.parse(data);
			let gameMessage = JSON.parse(response.Payload);
			this.$store.commit("gamelog/appendMessage", gameMessage);
		});

		this.connection.start().then(() => {
			this.queryGameState();
		});
	},
	methods: {
		queryGameState() {
			let request = createRequest("Query.GetUserState", "");
			this.connection.invoke("RequestQuery", request);
		},
		queryForfeitGame() {
			let request = createRequest("Query.ForfeitGame", "");
			this.connection.invoke("RequestQuery", request);
		},
		move(movementDirection) {
			let request = createRequest("Action.Movement", JSON.stringify({ MovementDirection: movementDirection }));
			this.connection.invoke("RequestAction", request);
		},
		lootAll() {
			let request = createRequest("Action.LootAll", "");
			this.connection.invoke("RequestAction", request);
		},
		loot(itemId) {
			let request = createRequest("Action.Loot", JSON.stringify({ ItemId: itemId }));
			this.connection.invoke("RequestAction", request);
		},
		attack(targetId) {
			let request = createRequest("Action.Attack", JSON.stringify({ TargetId: targetId }));
			this.connection.invoke("RequestAction", request);
		},
		consume(itemId) {
			let request = createRequest("Action.Consume", JSON.stringify({ ItemId: itemId }));
			this.connection.invoke("RequestAction", request);
		},
		newGame(templateName) {
			let request = createRequest("lobby.newgame", templateName);
			this.connection.invoke("RequestAction", request);
		}
	}
};
</script>
<style>
</style>
