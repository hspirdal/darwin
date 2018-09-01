<template>
<div>
</div>
</template>

<script>
export default {
	data() {
		return {
			currentState: "idle"
		};
	},
	created: function() {
		this.$store.watch(
			state => {
				return this.$store.getters["gamestatus/isCooldown"];
			},
			(isCooldown, wasCooldown) => {
				if (!isCooldown && wasCooldown && this.currentState == "combat") {
					let id = this.$store.getters["selection/selectedCreatureId"];
					this.$bus.$emit("Action.Attack", { targetId: id });
				}
			}
		);

		this.$store.watch(
			state => {
				return this.$store.getters["gamestatus/isInCombat"];
			},
			(isInCombat, wasInCombat) => {
				if (isInCombat) {
					this.currentState = "combat";
				} else {
					this.currentState = "idle";
				}
			}
		);
	}
};
</script>
