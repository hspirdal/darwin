"use strict";

export default {
	formatSuccessfulHit(player, attackResult) {
		let name = this.formatName(player, attackResult);
		let damage = this.formatDamage(attackResult);
		let toHit = this.formatToHit(attackResult);
		return `${name}: Attack ${attackResult.DefenderName} for ${damage} ${toHit}`;
	},

	formatFailedHit(player, attackResult) {
		let name = this.formatName(player, attackResult);
		let toHit = this.formatToHit(attackResult);
		return `${name}: Fail to attack ${attackResult.DefenderName} ${toHit}`;
	},

	formatAttacking() {
		return "Attacking target creature";
	},

	formatKilledBy(player, attackResult) {
		return `<span id="other"> ${attackResult.AttackerName} kills ${player.Name}</span> - Game over, buddy!`;
	},

	formatKilledOther(attackResult) {
		return `${attackResult.DefenderName} dies of combat damage!`;
	},

	formatExperienceGain(amount) {
		return `<span id="xpgain">Received ${amount} experience points</span> (in a future build).`;
	},

	formatMovementFailed(message) {
		return `<span id="failedaction">${message}</span>`;
	},

	formatName(player, attackResult) {
		if (player.Name === attackResult.AttackerName) {
			return `<span id="self">${player.Name}</span>`;
		}

		return `<span id="other">${attackResult.AttackerName}</span>`;
	},

	formatDamage(attackResult) {
		return `<span id="damage">${attackResult.DamageTotal}</span> damage!`;
	},

	formatToHit(attackResult) {
		return `<i>(ToHit ${attackResult.ToHitTotal})</i>`;
	}
};
