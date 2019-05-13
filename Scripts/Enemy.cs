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

	[SerializeField]
	protected GameObject[] drops;

	// método para recuperar a vida do personagem
	public override void Heal (int value) {

		health = health + value > maxHealth ? maxHealth : health + value;

	}

	protected override void Hurt () {
		// NADA AQUI POR ENQUANTO.
	}

	// esse método é disparado quando o personagem morre
	protected override void Die () {
		Instantiate (deathEffect, transform.position, transform.rotation);
		Drop ();
		Destroy (gameObject);
	}

	private void Drop () {
		foreach (GameObject go in drops) {
			Item item = go.GetComponent<Item> ();
			if (item != null) {
				float number = Random.Range(0f, 1f);
				if (number < item.dropChance) {
					Instantiate (go, transform.position, transform.rotation);
					break;
				}
			}
		}
	}

	protected void OnTriggerEnter2D (Collider2D col) {
		Player player = col.GetComponent<Player>();
		if (player != null) {
			player.TakeDamage(contactDamage);
		}
	}
}
