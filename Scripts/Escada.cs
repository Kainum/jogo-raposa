using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escada : MonoBehaviour {

	[SerializeField]
	private float waitTime;
	private float waitTime2;
	private PlatformEffector2D effector;

	void Start () {
		effector = GetComponent<PlatformEffector2D> ();
	}

	void Update () {

		if (Input.GetKey (KeyCode.DownArrow)) {
			effector.rotationalOffset = 180f;
			waitTime2 = waitTime;
		}
		if (waitTime2 > 0) {
			waitTime2 -= Time.deltaTime;
		} else {
			effector.rotationalOffset = 0f;
		}
	}
}
