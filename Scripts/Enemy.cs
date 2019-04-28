using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {


	[SerializeField]
	protected int maxHealth;
	protected int health;

	[SerializeField]
	protected GameObject deathEffect;

	protected Vector3 spawnLocation;
	protected Quaternion spawnRotation;
	protected Rigidbody2D rb;

	protected bool grounded;
	[SerializeField]
	private Transform groundCheck;

	protected bool facingRight = false;

	protected void IsGrounded () {
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground"));
	}

	// método para infligir dano na vida do personagem
	public void TakeDamage (int damage) {
		
		health -= damage;

		if (health <= 0) {
			Die ();
		}
	}

	// esse método é disparado quando o inimigo morre
	protected void Die () {
		//Instantiate (deathEffect, transform.position, Quaternion.identity);
		Destroy (gameObject);
	}

	// método para mudar a direção do personagem
	protected void Flip() {
		facingRight = !facingRight;
		transform.Rotate (0f, 180f, 0f);
	}

}
