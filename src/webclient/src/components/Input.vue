<template>
<div>
</div>
</template>

<script>
export default {
  mounted: function() {
    window.addEventListener(
      "keypress",
      function(e) {
        var code = e.charCode;
        var ch = String.fromCharCode(code);
        this.$emit("key-pressed", ch);
      }.bind(this)
    );
  }
};
</script>
