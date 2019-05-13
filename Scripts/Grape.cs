using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grape : Item {

	[SerializeField]
	private float maxVelQueda;
	private Rigidbody2D rb;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		rb.velocity = new Vector2 (rb.velocity.x, 0.7f);
	}

	void Update () {
		CheckMaxVelQueda ();
	}

	protected override void ItemEffect (Player player, int value)
	{
		Game.qtdVidas++;
		Game.UpdateCanvasLife ();
	}

	protected void CheckMaxVelQueda() {
		if (rb.velocity.y < maxVelQueda) {
			rb.velocity = new Vector2 (rb.velocity.x, maxVelQueda);
		}
	}
}
