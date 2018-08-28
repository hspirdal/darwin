<template>
    <div id="newgame">
        <h1>New Game</h1>
				<p>Template selection:</p>
				<b-form-select v-model="selectedTemplate" class="mb-3" v-on:change="selectTemplate">
					<option v-for="(template, index) in characterTemplates" v-bind:value="template.value" v-bind:key="index">{{ template.text }}</option>
				</b-form-select>
        <button type="button" v-on:click="startGame()" v-focus>Start</button>
    </div>
</template>

<script>
/*eslint no-console: [off] */
import "bootstrap/dist/css/bootstrap.css";
import "bootstrap-vue/dist/bootstrap-vue.css";

export default {
	name: "newgame",
	data() {
		return {
			selectedTemplate: "Jools",
			characterTemplates: [
				{ value: "Jools", text: "Jools - Level 1 Human Fighter" },
				{ value: "Jops", text: "Jops - Level 1 Human Wizard" },
				{ value: "Death Bunny", text: "Death Bunny - Level 1 Human Fighter" }
			]
		};
	},
	methods: {
		startGame() {
			this.$bus.$emit("NewGame.CharacterSelected", { selectedTemplate: this.selectedTemplate });
		},
		selectTemplate: function(templateName) {
			console.log(`Selecting ${templateName}..`);
		}
	}
};
</script>
