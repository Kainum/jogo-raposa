using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : Item {

	protected override void ItemEffect (Player player, int value)
	{
		Game.ModCherry (value);
	}
}
