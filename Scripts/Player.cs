using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {


	private Rigidbody2D rb;
	private SpriteRenderer spRend;
	private Animator anim;

	// VARIAVEIS DE ACOES

	private float direcaoX;
	private float direcaoY;

	public	float moveSpeed;
	public	float jumpForce;

	public  bool grounded;
	public	Transform groundCheck;

	private bool abaixado;

	private bool isClimb;
	public bool canClimb;
	public float climbSpeed;
	public float climbPosX;

	private float maxVelQueda;

	// VARIAVEIS DE ANIMACAO
	private float climbPosY;
	public int climbDif;
	private int climbCont;


	void Start () {
		maxVelQueda = -3f;
		rb = GetComponent<Rigidbody2D> ();
		spRend = GetComponent<SpriteRenderer> ();
		anim = GetComponent<Animator> ();
	}

	void Update () {
		updateDirecoes ();

		isGrounded ();

		movimento ();
		if (grounded) {
			corrigeY ();
			pulo (0f);
			abaixar ();
		}
		escalar ();

		checkMaxVelQueda ();

		updateAnim ();
	}

	// define as direcoes X e Y do personagem com base nos botões de movimento do teclado
	void updateDirecoes() {
		direcaoX = Input.GetAxisRaw("Horizontal");
		direcaoY = Input.GetAxisRaw("Vertical");
	}

	// verifica se o jogador está no chão
	void isGrounded () {
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
	}

	// corrige a posição y do jogador ( vários bugs envolvidos )
	void corrigeY () {
		float x = transform.position.x;
		float y = Mathf.Round(transform.position.y * 100) / 100;
		float z = transform.position.z;
		transform.position = new Vector3(x, y, z);
	}

	// movimento horizonal do personagem no cenário
	void movimento () {
		if (direcaoX != 0) {
			rb.velocity = new Vector2 (moveSpeed * direcaoX, rb.velocity.y);
			if (direcaoX > 0) {
				spRend.flipX = false;
			} else {
				spRend.flipX = true;
			}
		} else {
			rb.velocity = new Vector2 (0, rb.velocity.y);
		}
	}

	// pulo do personagem
	void pulo(float bonus){
		if (Input.GetButtonDown ("Jump")) {
			isClimb = false;
			rb.velocity = new Vector2 (rb.velocity.x, jumpForce + bonus);
		}
	}

	// metodo para o personagem se abaixar
	void abaixar() {
		if (rb.velocity.x == 0 && direcaoY == -1) {
			abaixado = true;
		} else {
			abaixado = false;
		}
	}

	// metodo para o personagem escalar (ainda em desenvolvimento)
	void escalar() {
		if (canClimb) {
			if (!isClimb) {
				if (direcaoY > 0 || (!grounded && direcaoY < 0)) {
					climbPosY = transform.position.y;
					isClimb = true;
				} else {
					isClimb = false;
				}
			} else {
				rb.isKinematic = true;
				transform.position = new Vector3 (climbPosX, transform.position.y, transform.position.z);
				if (direcaoY != 0) {
					updateClimbAnim ();
					rb.velocity = new Vector2 (0, climbSpeed * direcaoY);
				} else {
					rb.velocity = new Vector2 (0, 0);
				}
				pulo (-1);
			}
		} else {
			rb.isKinematic = false;
			isClimb = false;
		}
	}

	// método para limitar a velocidade máxima de queda do personagem
	void checkMaxVelQueda() {
		if (rb.velocity.y < maxVelQueda) {
			rb.velocity = new Vector2 (rb.velocity.x, maxVelQueda);
		}
	}

	// método para animação do personagem escalando
	void updateClimbAnim () {
		float atual = transform.position.y;
		float difer;
		bool neg;

		if (atual > climbPosY) {
			neg = false;
			difer = (atual - climbPosY) * 100;
		} else {
			neg = true;
			difer = (climbPosY - atual) * 100;
		}


		int cont = (int) difer / climbDif;
		if (neg) {
			if (cont == 3) {
				cont = 1;
			} else if (cont == 1) {
				cont = 3;
			}
		}
		if (cont == 4) {
			cont = 0;
		} else if (cont == -1) {
			cont = 3;
		}
		if (climbCont != cont) {
			if (cont == 0) {
				climbPosY = transform.position.y;
			}
			climbCont = cont;
		}
	}

	// atualiza o animator
	void updateAnim () {
		anim.SetFloat ("velocidadeX", Mathf.Abs(rb.velocity.x));
		anim.SetFloat ("velocidadeY", rb.velocity.y);
		anim.SetBool ("grounded", grounded);
		anim.SetBool ("crouch", abaixado);
		anim.SetBool ("climb", isClimb);
		if (climbCont != 2) {
			anim.SetInteger ("climbVar", climbCont);
		} else {
			anim.SetInteger ("climbVar", 0);
		}
	}
}
