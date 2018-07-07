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

    this.connection.on("authenticate", data => {
      console.log(data);
      var response = JSON.parse(data);
      if (response.Success) {
        console.log(
          "Successfully logged on. Session id: " + response.SessionId
        );
        sessionStorage.setItem("sessionId", response.SessionId);
        var o = {
          RequestName: "lobby.newgame",
          SessionId: response.SessionId
        };
        this.connection.invoke("SendAsync", JSON.stringify(o));
      }
    });

    var sessionId = sessionStorage.getItem("sessionId");
    console.log("session contents: " + sessionId);
    if (sessionId === null || Object.keys(sessionId) === 0) {
      console.log("Performing authentification..");
      this.connection.start().then(() => {
        var o = {
          UserName: "arch",
          Password: "1234"
        };
        this.connection.invoke("AutenticateAsync", JSON.stringify(o));
      });
    } else {
      this.connection.start().then(() => {
        var o = {
          RequestName: "Connection.Refresh",
          SessionId: sessionId
        };
        this.connection.invoke("SendAsync", JSON.stringify(o));
      });
    }
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
    }
  }
};
</script>
<style>
</style>
