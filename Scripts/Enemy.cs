using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Personagem {

	[SerializeField]
	protected GameObject deathEffect;

	protected Vector3 spawnLocation;
	protected Quaternion spawnRotation;

	public int contactDamage;

	protected bool colidindo;

	protected override void Hurt () {
		// NADA AQUI POR ENQUANTO.
	}

	// esse método é disparado quando o personagem morre
	protected override void Die () {
		Destroy (gameObject);
	}
}
