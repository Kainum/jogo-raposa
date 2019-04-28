using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {


	private Rigidbody2D rb;
	private Animator anim;

	// VARIAVEIS DE ACOES

	private float initialGravity;

	private bool facingRight = true;

	private float direcaoX;
	private float direcaoY;

	[SerializeField]
	private float moveSpeed;
	[SerializeField]
	private float jumpForce;

	[SerializeField]
	private bool grounded;
	[SerializeField]
	private Transform groundCheck;

	private bool abaixado;

	private bool isClimbing;
	[SerializeField]
	private float climbSpeed;
	[SerializeField]
	private LayerMask whatIsLadder;

	private float maxVelQueda;

	// VARIAVEIS DE ANIMACAO
	private float climbPosY;
	[SerializeField]
	private int climbDif;
	private int climbCont;


	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		initialGravity = rb.gravityScale;
		maxVelQueda = -3f;
	}

	void Update () {
		updateDirecoes ();
		isGrounded ();

		movimento ();
		if (grounded) {
			corrigeY ();
			pulo (0f);
		}
		abaixar ();
		escalar ();

		checkMaxVelQueda ();

		updateAnim ();
	}

	// define as direcoes X e Y do personagem com base nos botões de movimento do teclado
	private void updateDirecoes() {
		direcaoX = Input.GetAxisRaw("Horizontal");
		direcaoY = Input.GetAxisRaw("Vertical");
	}

	// verifica se o jogador está no chão
	private void isGrounded () {
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
	}

	// corrige a posição y do jogador ( vários bugs envolvidos )
	private void corrigeY () {
		float x = transform.position.x;
		float y = Mathf.Round(transform.position.y * 100) / 100;
		float z = transform.position.z;
		transform.position = new Vector3(x, y, z);
	}

	// movimento horizonal do personagem no cenário
	private void movimento () {
		if (direcaoX != 0) {
			rb.velocity = new Vector2 (moveSpeed * direcaoX, rb.velocity.y);
			if ((direcaoX > 0 && !facingRight) || (direcaoX < 0 && facingRight)) {
				Flip ();
			}
		} else {
			rb.velocity = new Vector2 (0, rb.velocity.y);
		}
	}

	// metodo para inverter direção que o personagem está olhando
	private void Flip() {
		facingRight = !facingRight;
		transform.Rotate (0f, 180f, 0f);
	}

	// pulo do personagem
	private void pulo(float bonus) {
		if (Input.GetButtonDown ("Jump")) {
			if (!isClimbing || (isClimbing && direcaoX != 0)) {
				isClimbing = false;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce + bonus);
            }
		}
	}

	// metodo para o personagem se abaixar
	private void abaixar() {
		if (rb.velocity.x == 0 && direcaoY == -1 && grounded) {
			abaixado = true;
		} else {
			abaixado = false;
		}
	}

	// metodo para o personagem escalar (ainda em desenvolvimento)
	private void escalar () {
		Vector3 posicao = new Vector3(transform.position.x, transform.position.y-0.16f, transform.position.z);
		RaycastHit2D hitInfo = Physics2D.Raycast (posicao, Vector2.up, 0.12f, whatIsLadder);

		if (hitInfo.collider != null) {
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
                if (!isClimbing) {
                    corrigeX();
                }
                isClimbing = true;
				climbPosY = transform.position.y;
			}

		} else {
			isClimbing = false;
		}
		if (isClimbing) {
			if (grounded && Input.GetKey (KeyCode.DownArrow)) {
				isClimbing = false;
				return;
			}
			rb.gravityScale = 0;
			rb.velocity = new Vector2 (0, direcaoY * climbSpeed);
			pulo (-1f);
			updateClimbAnim ();
		} else {
			rb.gravityScale = initialGravity;
		}
	}

    private void corrigeX () {
        int atualPosX = (int) (transform.position.x * 100);
        int resto = atualPosX % 16 - 8;
        float objetivoX = (atualPosX - resto);
        float x = objetivoX / 100;
        float y = transform.position.y;
        float z = transform.position.z;
        transform.position = new Vector3(x, y, z);
    }

	// método para limitar a velocidade máxima de queda do personagem
	private void checkMaxVelQueda() {
		if (rb.velocity.y < maxVelQueda) {
			rb.velocity = new Vector2 (rb.velocity.x, maxVelQueda);
		}
	}

	// método para animação do personagem escalando
	private void updateClimbAnim () {
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
	private void updateAnim () {
		anim.SetFloat ("velocidadeX", Mathf.Abs(rb.velocity.x));
		anim.SetFloat ("velocidadeY", rb.velocity.y);
		anim.SetBool ("grounded", grounded);
		anim.SetBool ("crouch", abaixado);
		anim.SetBool ("climb", isClimbing);
		if (climbCont != 2) {
			anim.SetInteger ("climbVar", climbCont);
		} else {
			anim.SetInteger ("climbVar", 0);
		}
	}
}
