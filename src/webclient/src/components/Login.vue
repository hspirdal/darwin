<template>
    <div id="login">
        <h1>Login</h1>
        <input type="text" name="username" v-model="input.username" placeholder="Username" />
        <input type="password" name="password" v-model="input.password" placeholder="Password" />
        <button type="button" v-on:click="login()">Login</button>
    </div>
</template>

<script>
export default {
  /*eslint no-console: [off] */
  name: "Login",
  data() {
    return {
      input: {
        username: "",
        password: ""
      }
    };
  },
  methods: {
    login() {
      if (this.input.username != "" && this.input.password != "") {
        var formData = new FormData();
        formData.append("UserName", this.input.username);
        formData.append("Password", this.input.password);

        this.$http
          .post("/api/account", formData)
          .then(response => {
            if (response.data.success) {
              console.log(
                "Successfully logged on. Session id: " + response.data.sessionId
              );
              sessionStorage.setItem("sessionId", response.data.sessionId);
              this.$emit("authenticated", true);
              this.$router.replace({ name: "game" });
              console.log(response);
            } else {
              console.log("Failed to log in!");
            }
          })
          .catch(e => {
            this.errors.push(e);
            console.log(this.errors);
          });
      } else {
        console.log("A username and password must be present");
      }
    }
  }
};
</script>

<style scoped>
#login {
  width: 500px;
  border: 1px solid #cccccc;
  background-color: #ffffff;
  margin: auto;
  margin-top: 200px;
  padding: 20px;
}
</style>
