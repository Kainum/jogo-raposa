using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Personagem {

	[SerializeField]
	protected GameObject deathEffect;

	protected Vector3 spawnLocation;
	protected Quaternion spawnRotation;

	[SerializeField]
	protected int contactDamage;

	protected override void Hurt () {
		// NADA AQUI POR ENQUANTO.
	}

	// esse método é disparado quando o personagem morre
	protected override void Die () {
		Instantiate (deathEffect, transform.position, transform.rotation);
		Destroy (gameObject);
	}

	protected void OnTriggerEnter2D (Collider2D col) {
		Player player = col.GetComponent<Player>();
		if (player != null) {
			player.TakeDamage(contactDamage);
		}
	}
}
