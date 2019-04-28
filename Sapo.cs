using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	public class Sapo : Enemy
	{
		[SerializeField]
		private float alturaPulo;
		[SerializeField]
		private float distanciaPulo;
		[SerializeField]
		private float intervaloPulo;
		private float intervaloCount = 0.5f;
		[SerializeField]
		private int MaxQtdPulos;
		private int qtdPulos;

		void Start () {
			health = maxHealth;
			spawnLocation = transform.position;
			spawnRotation = transform.rotation;
			rb = GetComponent<Rigidbody2D> ();
			qtdPulos = MaxQtdPulos;
		}

		void Update () {
			IsGrounded ();
			FicaParado ();
			Pulo ();
		}

		// pulo do sapo
		private void Pulo () {
			if (intervaloCount <= 0) {
				if (grounded) {
					if (qtdPulos == 0) {
						Flip ();
						qtdPulos = MaxQtdPulos;
					}
					rb.velocity = new Vector2(distanciaPulo * (facingRight ? 1 : -1), alturaPulo);
					intervaloCount = intervaloPulo;
					if (MaxQtdPulos != -1) {
						qtdPulos -= 1;
					}
				}
			} else {
				intervaloCount -= Time.deltaTime;
			}
		}

		// método para o sapo parar de ficar fugindo quando aterrissa
		private void FicaParado () {
			if (grounded && intervaloCount < intervaloPulo * 0.9) {
				rb.velocity = new Vector2(0f, rb.velocity.y);
			}
		}
	}
}

