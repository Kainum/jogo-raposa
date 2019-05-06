using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour {

	[SerializeField]
	private int value;
	[SerializeField]
	private GameObject item_effect;
	public float dropChance;

	protected void OnTriggerEnter2D (Collider2D col) {
		Player player = col.GetComponent<Player>();
		if (player != null) {
			ItemEffect(player, value);
			Instantiate (item_effect, transform.position, transform.rotation);
			Destroy (gameObject);
		}
	}

	// método que dispara quando o player 
	protected abstract void ItemEffect (Player player, int value);

}
