import Vue from "vue";
import Router from "vue-router";
import LoginComponent from "./components/Login.vue";
import GameClient from "./components/GameClient.vue";

Vue.use(Router);

export default new Router({
	routes: [
		{
			path: "/",
			redirect: {
				name: "login"
			}
		},
		{
			path: "/login",
			name: "login",
			component: LoginComponent
		},
		{
			path: "/game",
			name: "game",
			component: GameClient
		}
	]
});
