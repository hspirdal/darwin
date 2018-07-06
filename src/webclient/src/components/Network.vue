<template>
<div>
</div>
</template>
<script>
export default {
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
      } else {
        console.log(response.Message + "\nPayload: " + response.Payload);
      }
    });

    this.connection
      .start()
      .then(() =>
        this.connection.invoke(
          "SendAsync",
          '{ RequestName: "Authenticate", Payload: "arch;1234" }'
        )
      )
      .then(() =>
        this.connection.invoke("SendAsync", '{ RequestName: "lobby.newgame" }')
      );
  },
  methods: {
    move(movementDirection) {
      var o = {
        RequestName: "Action.Movement",
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
