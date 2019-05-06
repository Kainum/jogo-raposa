using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coracao : Item {

	[SerializeField]
	private float maxVelQueda;
	private Rigidbody2D rb;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		rb.velocity = new Vector2 (rb.velocity.x, 0.5f);
	}

	void Update () {
		CheckMaxVelQueda ();
	}

	protected override void ItemEffect (Player player, int value)
	{
		player.Heal (value);
	}

	protected void CheckMaxVelQueda() {
		if (rb.velocity.y < maxVelQueda) {
			rb.velocity = new Vector2 (rb.velocity.x, maxVelQueda);
		}
	}
}
