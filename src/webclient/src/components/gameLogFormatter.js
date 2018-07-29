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

	formatKilledBy(player, attackResult) {
		return '<span style="color: red">' + attackResult.AttackerName + " kills " + player.Name + " </span> - Game over, buddy!";
	},

	formatKilledOther(attackResult) {
		return attackResult.DefenderName + " dies of combat damage!";
	},

	formatExperienceGain(amount) {
		return '<span style="color: purple">Received ' + amount + " experience points</span> (in a future build).";
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
