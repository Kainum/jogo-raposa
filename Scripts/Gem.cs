using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : Item {

	protected override void ItemEffect (Player player, int value)
	{
		Game.ModGems (value);
	}
}
