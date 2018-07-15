<template>
<div>
</div>
</template>
<script>
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
      var response = JSON.parse(data);
      if (response.Type === "gamestatus") {
        var status = JSON.parse(response.Payload);
        if (status) {
          this.$store.commit("gamestatus/setStatus", status);
        }
      } else if (response.Type === "NotAuthenticated") {
        // Server might have restarted and purged active session list.
        console.log(response.Message + "\nPayload: " + response.Payload);
        sessionStorage.removeItem("sessionId");
        location.reload();
      } else {
        console.log(response.Message + "\nPayload: " + response.Payload);
      }
    });

    this.connection.start().then(() => {
      var sessionId = sessionStorage.getItem("sessionId");
      var o = {
        RequestName: "lobby.newgame",
        SessionId: sessionId
      };
      this.connection.invoke("SendAsync", JSON.stringify(o));
    });
  },
  methods: {
    move(movementDirection) {
      var sessionId = sessionStorage.getItem("sessionId");
      var o = {
        RequestName: "Action.Movement",
        SessionId: sessionId,
        Payload: JSON.stringify({
          OwnerId: 1,
          Name: "Action.Movement",
          MovementDirection: movementDirection
        })
      };
      this.connection.invoke("SendAsync", JSON.stringify(o));
    },
    lootAll() {
      var sessionId = sessionStorage.getItem("sessionId");
      var o = {
        RequestName: "Action.LootAll",
        SessionId: sessionId,
        Payload: JSON.stringify({
          OwnerId: 1,
          Name: "Action.LootAll"
        })
      };
      this.connection.invoke("SendAsync", JSON.stringify(o));
    }
  }
};
</script>
<style>
</style>
