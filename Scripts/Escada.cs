using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escada : MonoBehaviour {

	private float posX;
	private bool canClimb;

	public Transform inicio;
	public Transform fim;
	public Player player;

	void Start () {
		posX = transform.position.x;
	}

	void Update () {

		canClimb = Physics2D.Linecast(inicio.position, fim.position, 1 << LayerMask.NameToLayer("Player"));
		player.canClimb = canClimb;
		if (canClimb) {
			player.climbPos = posX;
		}
	}
}
