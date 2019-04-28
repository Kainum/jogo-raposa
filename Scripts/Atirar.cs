using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atirar : MonoBehaviour {

	[SerializeField]
	private Transform firePoint;
	[SerializeField]
	private GameObject cherryShoot;
	[SerializeField]
	private float intervalo; // TEMPO ENTRE UM DISPARO E OUTRO
	private float intervaloCount;
	
	// Update is called once per frame
	void Update () {
		if (intervaloCount <= 0) {
			if (Input.GetButton ("Fire1")) {
				Shoot ();
				intervaloCount = intervalo;
			}
		} else {
			intervaloCount -= Time.deltaTime;
		}
	}

	void Shoot () {
		Instantiate (cherryShoot, firePoint.position, firePoint.rotation);
	}
}
