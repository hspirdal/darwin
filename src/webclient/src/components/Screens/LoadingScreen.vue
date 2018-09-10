<template>
	<div id="loadingscreen" class="container">
		<div class="item">
		<h1>Connecting to server..</h1>
		<scaling-squares-spinner class="spinner"
			:animation-duration="1250"
			:size="165"
			color="#6B97FF"
		/>
		</div>
	</div>
</template>
<script>
import { ScalingSquaresSpinner } from "epic-spinners";

export default {
	components: {
		ScalingSquaresSpinner
	}
};
</script>
<style scoped>
.container {
	display: flex;
	align-items: center;
	justify-content: center;
	height: 68vh;
}
.spinner {
	margin: auto;
	width: 50%;
}
</style>