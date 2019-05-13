using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Personagem {

	// VARIAVEIS DE COMBATE

	[SerializeField]
	private float invincibilityTime;
	private float invincibilitytimeCount;
	private bool invincible;

	[SerializeField]
	private Transform firePoint;
	[SerializeField]
	private GameObject cherryShoot;
	[SerializeField]
	private float intervalo; // TEMPO ENTRE UM DISPARO E OUTRO
	private float intervaloCount;

	// VARIAVEIS DE ACOES

	private float initialGravity;

	private float direcaoX;
	private float direcaoY;

	[SerializeField]
	private float moveSpeed;
	[SerializeField]
	private float jumpForce;

	private bool abaixado;

	private bool isClimbing;
	[SerializeField]
	private float climbSpeed;
	[SerializeField]
	private LayerMask whatIsLadder;

	// VARIAVEIS DE ANIMACAO

	private float climbPosY;
	[SerializeField]
	private int climbDif;
	private int climbCont;


	void Start () {
		Game.GameStart ();

        rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		initialGravity = rb.gravityScale;
		maxVelQueda = -3f;
		health = maxHealth;
	}

	void FixedUpdate() {
		UpdateDirecoes ();
		IsGrounded ();

		Movimento ();
		if (grounded) {
			CorrigeY ();
			Pulo (0f);
		}
		Atirar ();
		Abaixar ();
		Escalar ();
		if (invincible) {
			Invencibilidade ();
		}

		CheckMaxVelQueda ();

		UpdateAnim ();
	}

	// define as direcoes X e Y do personagem com base nos botões de movimento do teclado
	private void UpdateDirecoes() {
		direcaoX = Input.GetAxisRaw("Horizontal");
		direcaoY = Input.GetAxisRaw("Vertical");
	}

	// corrige a posição y do jogador ( vários bugs envolvidos )
	private void CorrigeY () {
		float x = transform.position.x;
		float y = Mathf.Round(transform.position.y * 100) / 100;
		float z = transform.position.z;
		transform.position = new Vector3(x, y, z);
	}

	// movimento horizontal do personagem no cenário
	private void Movimento () {
		if (direcaoX != 0) {
			rb.velocity = new Vector2 (moveSpeed * direcaoX, rb.velocity.y);
			if ((direcaoX > 0 && !facingRight) || (direcaoX < 0 && facingRight)) {
				Flip ();
			}
		} else {
			rb.velocity = new Vector2 (0, rb.velocity.y);
		}
	}

	// Pulo do personagem
	private void Pulo(float bonus) {
		if (Input.GetButtonDown ("Jump")) {
			if (!isClimbing || (isClimbing && direcaoX != 0)) {
				isClimbing = false;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce + bonus);
            }
		}
	}

	// metodo para o player arremessar cerejas
	private void Atirar() {
		if (Input.GetButtonDown("Fire1")) {
			if (Game.qtdCherries == 0) {
				//	SOM DE IMPOSSIVEL
			}
		}
		if (intervaloCount <= 0) {
			if (Input.GetButton ("Fire1")) {
				if (Game.qtdCherries > 0) {
					Game.ModCherry (-1);
					Instantiate (cherryShoot, firePoint.position, firePoint.rotation);
					intervaloCount = intervalo;
				}
			}
		} else {
			intervaloCount -= Time.deltaTime;
		}
	}

	// metodo para o personagem se abaixar
	private void Abaixar() {
		if (rb.velocity.x == 0 && direcaoY == -1 && grounded) {
			abaixado = true;
		} else {
			abaixado = false;
		}
	}

	// metodo para o personagem escalar (ainda em desenvolvimento)
	private void Escalar () {
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
			Pulo (-1f);
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

	private void Invencibilidade () {
		if (invincibilitytimeCount > 0) {
			invincibilitytimeCount -= Time.deltaTime;
		} else {
			invincible = false;
			int enemyLayer = LayerMask.NameToLayer ("Enemy");
			int playerLayer = LayerMask.NameToLayer ("Player");
			Physics2D.IgnoreLayerCollision (enemyLayer, playerLayer, false);
			anim.SetLayerWeight (1, 0);
		}
	}

	// método para recuperar a vida do personagem
	public override void Heal (int value) {

		health = health + value > maxHealth ? maxHealth : health + value;
		Game.UpdateCanvasHealth (health);
	}

	// método que ocorre quando o player recebe dano
	protected override void Hurt () {
		Game.UpdateCanvasHealth (health);
		if (health > 0) {
			invincible = true;
			invincibilitytimeCount = invincibilityTime;
			int enemyLayer = LayerMask.NameToLayer ("Enemy");
			int playerLayer = LayerMask.NameToLayer ("Player");
			Physics2D.IgnoreLayerCollision (enemyLayer, playerLayer);
			anim.SetLayerWeight (1, 1);
		}
	}

	protected override void Die () {
		if (Game.qtdVidas == 0) {
			Game.GameOver ();
		} else {
			Game.qtdVidas--;
			Game.UpdateCanvasLife ();
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}

	// atualiza o animator
	protected override void UpdateAnim () {
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
