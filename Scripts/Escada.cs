using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escada : MonoBehaviour {

	private float posX;
	private bool canClimb;

	private Vector3 inicioPos;
	private Vector3 fimPos;

	public Transform inicio;
	public Transform fim;
	public Player player;

	void Start () {
		inicioPos = new Vector3(inicio.position.x, inicio.position.y, inicio.position.z);
		fimPos = new Vector3(fim.position.x, fim.position.y, fim.position.z);
		posX = transform.position.x;
	}

	void Update () {

		canClimb = Physics2D.Linecast(inicioPos, fimPos, 1 << LayerMask.NameToLayer("Player"));
		player.canClimb = canClimb;
		if (canClimb) {
			player.climbPos = posX;
		}
	}
}
