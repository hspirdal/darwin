import Vue from "vue";
import Vuex from "vuex";
import gamestatus from "./modules/gamestatus";

Vue.use(Vuex);

export default new Vuex.Store({
  modules: {
    gamestatus
  }
});
