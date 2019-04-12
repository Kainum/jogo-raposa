using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public	float moveSpeed;
	public	int	jumpForce;

	private float direcaoX;
	private float direcaoY;

	private Rigidbody2D rb;
	private SpriteRenderer spRend;
	private Animator anim;

	private float maxVelQueda;

	private bool abaixado;

	private bool isClimb;
	public bool canClimb;
	public float climbSpeed;
	public float climbPos;

	// Grounded
	public  bool grounded;
	public	Transform groundCheck;

	void Start () {
		maxVelQueda = -3f;
		rb = GetComponent<Rigidbody2D> ();
		spRend = GetComponent<SpriteRenderer> ();
		anim = GetComponent<Animator> ();
	}

	void Update () {
		updateDirecoes ();

		isGrounded ();
		corrigeY ();

		movimento ();
		pulo ();
		abaixar ();
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
		if (grounded) {
			float x = transform.position.x;
			float y = Mathf.Round(transform.position.y * 100) / 100;
			float z = transform.position.z;
			transform.position = new Vector3(x, y, z);
		}
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
	void pulo(){
		if (Input.GetButtonDown ("Jump") && grounded) {
			rb.AddForce (new Vector2(rb.velocity.x, jumpForce));
		}
	}

	// metodo para o personagem se abaixar
	void abaixar() {
		if (rb.velocity.x == 0 && grounded && direcaoY == -1) {
			abaixado = true;
		} else {
			abaixado = false;
		}
	}

	// metodo para o personagem escalar (ainda em desenvolvimento)
	void escalar() {
		if (canClimb) {
			if (!isClimb) {
				if (direcaoY > 0) {
					isClimb = true;
				} else {
					isClimb = false;
				}
			} else {
				rb.isKinematic = true;
				transform.position = new Vector3 (climbPos, transform.position.y, transform.position.z);
				if (direcaoY != 0) {
					rb.velocity = new Vector2 (0, climbSpeed * direcaoY);
				} else {
					rb.velocity = new Vector2 (0, 0);
				}
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

	// atualiza o animator
	void updateAnim () {
		anim.SetFloat ("velocidadeX", Mathf.Abs(rb.velocity.x));
		anim.SetFloat ("velocidadeY", rb.velocity.y);
		anim.SetBool ("grounded", grounded);
		anim.SetBool ("crouch", abaixado);
	}
}
