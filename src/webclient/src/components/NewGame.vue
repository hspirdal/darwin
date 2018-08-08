<template>
    <div id="newgame">
        <h1>New Game</h1>
				<p>Template selection:</p>
				<b-form-select v-model="selectedTemplate" class="mb-3" v-on:change="selectTemplate">
					<option v-for="(template, index) in characterTemplates" v-bind:value="template.value" v-bind:key="index">{{ template.text }}</option>
				</b-form-select>
        <button type="button" v-on:click="startGame()">Start</button>
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
			selectedTemplate: "jools",
			characterTemplates: [{ value: "jools", text: "Jools - Level 1 Human Fighter" }, { value: "jops", text: "Jops - Level 1 Human Wizard" }]
		};
	},
	methods: {
		startGame() {
			console.log(`Starting game with ${this.selectedTemplate} `);
		},
		selectTemplate: function(templateName) {
			console.log(`Selecting ${templateName}..`);
		}
	}
};
</script>
