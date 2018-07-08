<template>
    <div id="app">
        <div id="nav">
            <router-link v-if="authenticated" to="/login" v-on:click.native="logout()" replace>Logout</router-link>
        </div>
        <router-view @authenticated="setAuthenticated" />
    </div>
</template>

<script>
export default {
  /*eslint no-console: [off] */
  name: "App",
  data() {
    return {
      authenticated: false
    };
  },
  mounted() {
    this.checkAuthentication();
    if (!this.authenticated) {
      this.$router.replace({ name: "login" });
    }
  },
  methods: {
    setAuthenticated(status) {
      this.authenticated = status;
      console.log("setting session to " + status);
    },
    logout() {
      sessionStorage.removeItem("sessionId");
      this.setAuthenticated(false);
    },
    checkAuthentication() {
      var sessionId = sessionStorage.getItem("sessionId");
      console.log("session contents: " + sessionId);
      if (sessionId === null || Object.keys(sessionId) === 0) {
        this.authenticated = false;
      } else {
        this.authenticated = true;
      }
    }
  }
};
</script>

<style>
body {
  background-color: #f0f0f0;
}
h1 {
  padding: 0;
  margin-top: 0;
}
#app {
  width: 1024px;
  margin: auto;
}
</style>
