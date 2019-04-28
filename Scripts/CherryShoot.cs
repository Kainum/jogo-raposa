using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryShoot : MonoBehaviour {

	[SerializeField]
	private float speed;
	[SerializeField]
	private int damage;
	[SerializeField]
	private float maxLifeTime;
	[SerializeField]
	private float gravityTime;
	[SerializeField]
	private float gravityScale;
	private float countTime = 0;
	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		rb.velocity = transform.right * speed;
	}
	
	// Update is called once per frame
	void Update () {
		if (countTime < maxLifeTime) {
			if (rb.gravityScale != gravityScale && countTime > gravityTime) {
				rb.gravityScale = gravityScale;
			}
			countTime += Time.deltaTime;
		} else {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D (Collider2D hitInfo) {
		Enemy enemy = hitInfo.GetComponent<Enemy> ();
		if (enemy != null) {
			enemy.TakeDamage (damage);
		}
		Destroy (gameObject);
	}
}
