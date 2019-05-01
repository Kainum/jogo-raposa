using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Personagem : MonoBehaviour {

    [SerializeField]
    protected int maxHealth;
    protected int health;

	protected Rigidbody2D rb;
	protected Animator anim;

	// VARIAVEIS DE ACOES

	protected bool facingRight = true;

	[SerializeField]
	protected bool grounded;
	[SerializeField]
	protected Transform groundCheck;

	protected float maxVelQueda = -3f;

	// verifica se o jogador está no chão
	protected void IsGrounded () {
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
	}

	// metodo para inverter direção que o personagem está olhando
	protected void Flip() {
		facingRight = !facingRight;
		transform.Rotate (0f, 180f, 0f);
	}

	// método para limitar a velocidade máxima de queda do personagem
	protected void CheckMaxVelQueda() {
		if (rb.velocity.y < maxVelQueda) {
			rb.velocity = new Vector2 (rb.velocity.x, maxVelQueda);
		}
	}

    // método para infligir dano na vida do personagem
	public void TakeDamage (int value) {
		
		health -= value;

		if (health <= 0) {
			Die ();
		} else {
            Hurt ();
        }
	}

	// método para recuperar a vida do personagem
	public void Heal (int value) {

		health = health + value > maxHealth ? maxHealth : health + value;

	}

	// esse método é disparado quando o personagem morre
	protected abstract void Die ();

    // esse método é disparado quando o personagem recebe dano
    protected abstract void Hurt ();

	// atualiza o animator
	protected abstract void UpdateAnim ();
}
