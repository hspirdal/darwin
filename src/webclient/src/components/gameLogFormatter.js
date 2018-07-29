export default {
	formatSuccessfulHit(player, attackResult) {
		var name = this.formatName(player, attackResult);
		var damage = this.formatDamage(attackResult);
		var toHit = this.formatToHit(attackResult);
		return name + ": Attack " + attackResult.DefenderName + " for " + damage + " " + toHit;
	},

	formatFailedHit(player, attackResult) {
		var name = this.formatName(player, attackResult);
		var toHit = this.formatToHit(attackResult);
		return name + ": Fail to attack " + attackResult.DefenderName + " " + toHit;
	},

	formatName(player, attackResult) {
		if (player.Name === attackResult.AttackerName) {
			return '<span style="color: green" id="self">' + player.Name + "</span>";
		}

		return '<span style="color: red" id="other">' + attackResult.AttackerName + "</span>";
	},

	formatDamage(attackResult) {
		return '<span style="color: red">' + attackResult.DamageTotal + "</span> damage!";
	},

	formatToHit(attackResult) {
		return "<i>(ToHit " + attackResult.ToHitTotal + ")</i>";
	}
};
