using UnityEngine;
using System;
using System.Collections;

[Serializable]
public enum Enum_moveMode
{
	Non,
	NormalMove,
	DoubleClique
}

[Serializable]
public enum Enum_shotMode
{
	Non,
	Normal,
	DeuxLine,
	TestMode,
	Spiral
}

public class InputController : MonoBehaviour
{
	PlayerAvatar player = null;
	private int i_sidePlayer = -1;

	public void setSidePlayer(int _i_sidePlayer)
	{
		i_sidePlayer = _i_sidePlayer;
	}

	// Use this for initialization
	void Start ()
	{
		player = this.gameObject.GetComponent<PlayerAvatar>();
	}

	// Update is called once per frame
	void Update ()
	{
		if(player.getIdMyPlayerInGame() == player.getIdThisPlayerInGame()
		   || (!Network.isClient && !Network.isServer))
		{

		}
	}
}












