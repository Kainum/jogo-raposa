using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {

	[HideInInspector]
	public static int qtdVidas = 1;
	private const int maxGems = 99;
	private const int maxCherries = 99;

	[HideInInspector]
	public static int qtdGems = 0;
	[HideInInspector]
	public static int qtdCherries;

    // CANVAS

	private static Text txtCherry;
	private static Text txtGem;
	private static Text txtLife;

    private static Image[] healthBar;
	
	private static Sprite coracao;
	private static Sprite coracao_damage;

    private static void InicializarAtributos()
    {
        txtCherry = GameObject.Find("/Canvas/PanelCerejas/Text").GetComponent<Text>();
        txtGem = GameObject.Find("/Canvas/PanelGemas/Text").GetComponent<Text>();
        txtLife = GameObject.Find("/Canvas/PanelVidas/Text").GetComponent<Text>();

        healthBar = new Image[3];
        healthBar[0] = GameObject.Find("/Canvas/PanelCoracoes/coracao1").GetComponent<Image>();
        healthBar[1] = GameObject.Find("/Canvas/PanelCoracoes/coracao2").GetComponent<Image>();
		healthBar[2] = GameObject.Find("/Canvas/PanelCoracoes/coracao3").GetComponent<Image>();

        coracao = Resources.Load<Sprite>("Grafics/Sprites/Items/coracao");
        coracao_damage = Resources.Load<Sprite>("Grafics/Sprites/Items/coracao_damage");
    }

	// método para modificar a quantidade de cerejas do jogador
	public static void ModCherry (int value) {
		if (value > 0) {
			qtdCherries = qtdCherries + value > maxCherries ? maxCherries : qtdCherries + value;
		} else {
			qtdCherries = qtdCherries + value < 0 ? 0 : qtdCherries + value;
		}
        UpdateCanvasCherry();
	}

	// método para modificar a quantidade de gemas do jogador
	public static void ModGems (int value) {
		int finalValue = qtdGems + value;
		if (value > 0) {
			if (finalValue <= maxGems) {
				qtdGems = finalValue;
			} else {
				qtdGems = finalValue - maxGems - 1;
				qtdVidas++;
				UpdateCanvasLife ();
			}
		} else {
			qtdGems = finalValue < 0 ? 0 : finalValue;
		}
        UpdateCanvasGem();
	}

	public static void UpdateCanvasHealth (int health) {
		int cont = 0;
		foreach (Image item in healthBar) {
			cont++;
			if (cont <= health) {
				item.sprite = coracao;
			} else {
				item.sprite = coracao_damage;
			}
		}
	}

    public static void UpdateCanvasCherry ()
    {
        txtCherry.text = "x" + qtdCherries.ToString("00");
    }

    public static void UpdateCanvasGem()
    {
        txtGem.text = "x" + qtdGems.ToString("00");
    }

    public static void UpdateCanvasLife () {
		txtLife.text = "x" + qtdVidas.ToString("00");
	}

    public static void UpdateCanvas()
    {
        UpdateCanvasCherry();
        UpdateCanvasGem();
        UpdateCanvasLife();
    }
    
	public static void GameStart () {
        InicializarAtributos();
        UpdateCanvas();
    }

	// método que é acionado quando o jogador perde todas as vidas
	public static void GameOver () {
		Debug.Log ("GAME OVER");
		// **AINDA NAO IMPLEMENTADO
	}
}
