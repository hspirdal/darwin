<template>
<div>
</div>
</template>

<script>
export default {
  mounted: function() {
    window.onkeydown = function() {
      const key = event.key;
      this.$emit("key-pressed", key);
    }.bind(this);
  }
};
</script>
