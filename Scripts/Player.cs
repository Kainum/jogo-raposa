using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public	float moveSpeed;
	public	int	jumpForce;

	private float direcao;
	private Transform transf;
	private Rigidbody2D rb;
	private SpriteRenderer spRend;
	private Animator anim;

	private float maxVelQueda;

	// Grounded
	public  bool grounded;
	public	Transform groundCheck;

	void Start () {
		maxVelQueda = -3f;
		transf = GetComponent<Transform> ();
		rb = GetComponent<Rigidbody2D> ();
		spRend = GetComponent<SpriteRenderer> ();
		anim = GetComponent<Animator> ();
	}

	void Update () {
		isGrounded ();

		corrigeY ();
		movimento ();
		pulo ();
		checkMaxVelQueda ();

		updateAnim ();
	}

	// verifica se o jogador está no chão
	void isGrounded () {
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
	}

	// corrige a posição y do jogador ( vários bugs envolvidos )
	void corrigeY () {
		if (grounded) {
			float x = transf.position.x;
			float y = Mathf.Round(transf.position.y * 100) / 100;
			float z = transf.position.z;
			transf.SetPositionAndRotation (new Vector3(x, y, z), transf.rotation);
		}
	}

	// movimento horizonal do personagem no cenário
	void movimento () {
		direcao = Input.GetAxisRaw("Horizontal");
		if (direcao != 0) {
			rb.velocity = new Vector2 (moveSpeed * direcao, rb.velocity.y);
			if (direcao > 0) {
				spRend.flipX = false;
			} else {
				spRend.flipX = true;
			}
		} else {
			rb.velocity = new Vector2 (0, rb.velocity.y);
		}
	}

	// pulo do personagem
	void pulo(){
		if (Input.GetButtonDown ("Jump") && grounded) {
			rb.AddForce (new Vector2(rb.velocity.x, jumpForce));
		}
	}

	// método para limitar a velocidade máxima de queda do personagem
	void checkMaxVelQueda() {
		if (rb.velocity.y < maxVelQueda) {
			rb.velocity = new Vector2 (rb.velocity.x, maxVelQueda);
		}
	}

	// atualiza o animator
	void updateAnim () {
		anim.SetFloat ("velocidadeX", Mathf.Abs(rb.velocity.x));
		anim.SetFloat ("velocidadeY", rb.velocity.y);
		anim.SetBool ("grounded", grounded);
	}
}
