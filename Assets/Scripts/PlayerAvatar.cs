using UnityEngine;
using System.Collections;

public class PlayerAvatar : MonoBehaviour {

	private float f_health = -1f;
	private int i_idThisPlayerInGame = -1;
	private int i_idMyPlayerInGame = -2;  //for avoid bugs in Update() of InputController, use different number with i_idThisPlayerInGame

	public void initThisPlayer(int _i_idThisPlayerInGame, int _i_idMyPlayerInGame)
	{
		i_idThisPlayerInGame = _i_idThisPlayerInGame;
		i_idMyPlayerInGame = _i_idMyPlayerInGame;
	}

	public int getIdThisPlayerInGame()
	{
		return i_idThisPlayerInGame;
	}

	public int getIdMyPlayerInGame()
	{
		return i_idMyPlayerInGame;
	}

	// Use this for initialization
	void Start () {
		f_health = GlobalVariables.F_FULL_HEALTH_PLAYER;
	}
}
