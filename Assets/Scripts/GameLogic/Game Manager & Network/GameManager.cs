﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManager: MonoBehaviour {

	private int i_maxNumPlayers = 1;

	private string[] sArray_nameOfAllPlayers;
	private int[] iArray_idOfAllPlayers;
	private int[] iArray_sideOfAllPlayers;
	private const int NEW_PLAYER_JOINED = 1;
	private const int EXIST_PLAYER_EXITED = 2;

	private Enum_playMode playMode = Enum_playMode.Non;
//	private GUIManager guiManager;
	private GameInfo gameInfo;

	private class PlayerInfo
	{
		public int i_idPlayerInGame = -1;
		public int i_idPlayerInNetwork = -1;
		public string s_playerName = null;
//		public float f_healthCurrentLife = -1;
		public int i_sidePlayer = -1; // for using in the future: player VS player
		public GameObject go_player = null;
	}

	private PlayerInfo[] array_allPlayersInfos;
	private int i_idMyPlayerInGame = -1;
	private int i_idMyPlayerInNetwork = -1;

	private InputController inputController;

	private bool b_quitGame = false;   //player can set this in the inputController when clicking the quitGame button

	public bool B_quitGame
	{
		get{return b_quitGame;}
		set{b_quitGame = value;}
	}

	// Use this for initialization
	void Start () {
		initGame ();
	}

	private void initGame()
	{
		inputController = FindObjectOfType (typeof(InputController)) as InputController;

		gameInfo = FindObjectOfType (typeof(GameInfo)) as GameInfo;
		i_maxNumPlayers = gameInfo.I_maxNumPlayers;
		playMode = gameInfo.Enum_playMode;

		sArray_nameOfAllPlayers = new string[i_maxNumPlayers];
		sArray_nameOfAllPlayers = gameInfo.getAllPlayersName();
		
		iArray_idOfAllPlayers = new int[i_maxNumPlayers];
		iArray_idOfAllPlayers = gameInfo.getAllPlayersId ();;

		iArray_sideOfAllPlayers = new int[i_maxNumPlayers];
		iArray_sideOfAllPlayers = gameInfo.getAllPlayersSides ();

		array_allPlayersInfos = new PlayerInfo[i_maxNumPlayers];
		for(int i = 0; i < i_maxNumPlayers; ++i)    //index 0 is the server, others are the clients
		{
			array_allPlayersInfos[i] = new PlayerInfo();
			
			array_allPlayersInfos[i].i_idPlayerInGame = i;
			array_allPlayersInfos[i].i_idPlayerInNetwork = iArray_idOfAllPlayers[i];
			array_allPlayersInfos[i].s_playerName = sArray_nameOfAllPlayers[i];
//			array_allPlayersInfos[i].f_healthCurrentLife = GlobalVariables.F_FULL_HEALTH_PLAYER;   //need assign value after
			array_allPlayersInfos[i].i_sidePlayer = iArray_sideOfAllPlayers[i];
			array_allPlayersInfos[i].go_player = null;

			if(iArray_idOfAllPlayers[i] == int.Parse(Network.player.ToString()))
			{
				i_idMyPlayerInGame = i;
				inputController.setSidePlayer (array_allPlayersInfos[i_idMyPlayerInGame].i_sidePlayer);
			}
		}

		i_idMyPlayerInNetwork = int.Parse (Network.player.ToString());

		if(!Network.isServer && !Network.isClient)
		{
			i_idMyPlayerInGame = 0;
			i_idMyPlayerInNetwork = -1;
		}

		createPlayer ();
	}

	// Update is called once per frame
	void Update () {

	}

	
	void OnPlayerDisconnected(NetworkPlayer player)
	{
		Network.DestroyPlayerObjects (player);

		int i_idExitPlayerInNetwork = int.Parse (player.ToString());

		for(int i = 0; i < i_maxNumPlayers; ++i)
		{
			if(gameInfo.getPlayerIdInNetworkByIndex(i) == i_idExitPlayerInNetwork)
			{
				networkView.RPC("updateGameInfoRPC", RPCMode.All, i, i_idExitPlayerInNetwork, EXIST_PLAYER_EXITED);
			}
		}
	}
	
	void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		gameInfo.Enum_quitGameMode = Enum_quitGameMode.QuitDuringGame;
		Application.LoadLevel ("Menu");
	}

	public void quitGame ()
	{

		if(b_quitGame == true)
		{
			gameInfo.Enum_quitGameMode = Enum_quitGameMode.QuitDuringGame;

		}
		else
		{
			gameInfo.Enum_quitGameMode = Enum_quitGameMode.FinishGame;
		}

		Application.LoadLevel ("Menu");
	}

	public void createPlayer()
	{
		if(Network.isServer)
		{
			server_createPlayer(i_idMyPlayerInGame, i_idMyPlayerInNetwork);
		}
		else if(Network.isClient)
		{
			networkView.RPC("server_requestNewPlayerRPC", RPCMode.Server, i_idMyPlayerInGame, i_idMyPlayerInNetwork);
		}
		else
		{
//			array_allPlayersInfos[i_idMyPlayerInGame].f_healthCurrentLife = GlobalVariables.F_FULL_HEALTH_PLAYER;
//			myPlayer = GameObject.Instantiate(GlobalVariables.GO_PLAYER, new Vector3(-25f, 2f, 0f), Quaternion.identity) as GameObject;
			array_allPlayersInfos[i_idMyPlayerInGame].go_player =
				GameObject.Instantiate(GlobalVariables.GO_PLAYER, new Vector3(-25f, 2f, 0f), Quaternion.identity) as GameObject;
		}
	}

	public void server_createPlayer(int _i_idPlayerInGame, int _i_idPlayerInNetwork)
	{
		if(Network.isServer)
		{
//			array_allPlayersInfos[_i_idPlayerInGame].f_healthCurrentLife = GlobalVariables.F_FULL_HEALTH_PLAYER;

			if(array_allPlayersInfos[_i_idPlayerInGame].go_player != null)
			{
				Destroy(array_allPlayersInfos[_i_idPlayerInGame].go_player);
			}

			array_allPlayersInfos[_i_idPlayerInGame].go_player = 
				GameObject.Instantiate (GlobalVariables.GO_PLAYER, new Vector3(-25f, 2f, 0f), Quaternion.identity) as GameObject;
			
			array_allPlayersInfos [_i_idPlayerInGame].go_player.GetComponent<PlayerAvatar> ().initThisPlayer (_i_idPlayerInGame, i_idMyPlayerInGame);
			
			NetworkViewID viewID = Network.AllocateViewID ();
			array_allPlayersInfos [_i_idPlayerInGame].go_player.networkView.viewID = viewID;
			
			networkView.RPC("client_createPlayerInGameRPC", RPCMode.Others, _i_idPlayerInGame, _i_idPlayerInNetwork, viewID);
		}
	}

	[RPC]
	public void server_requestNewPlayerRPC (int _i_idPlayerInGame, int _i_idPlayerInNetwork)
	{
		server_requestNewPlayerLocal (_i_idPlayerInGame, _i_idPlayerInNetwork);
	}

	public void server_requestNewPlayerLocal(int _i_idPlayerInGame, int _i_idPlayerInNetwork)
	{
		if(Network.isServer)
		{
			for(int i = 0; i < i_maxNumPlayers; ++i)
			{
				if(array_allPlayersInfos[i].i_idPlayerInNetwork == _i_idPlayerInNetwork)
				{
					if(array_allPlayersInfos[i].go_player == null)
					{
						server_createPlayer(_i_idPlayerInGame, _i_idPlayerInNetwork);
					}
					i = i_maxNumPlayers;
				}
			}
		}
	}

	[RPC]
	public void client_createPlayerInGameRPC(int _i_idPlayerInGame, int _i_idPlayerInNetwork, NetworkViewID _viewID)
	{
		client_createPlayerInGameLocal (_i_idPlayerInGame, _i_idPlayerInNetwork, _viewID);
	}
	
	public void client_createPlayerInGameLocal(int _i_idPlayerInGame, int _i_idPlayerInNetwork, NetworkViewID _viewID)
	{
		if(Network.isClient)
		{
//			array_allPlayersInfos[_i_idPlayerInGame].f_healthCurrentLife = GlobalVariables.F_FULL_HEALTH_PLAYER;

			if(array_allPlayersInfos[_i_idPlayerInGame].go_player != null)
			{
				Destroy(array_allPlayersInfos[_i_idPlayerInGame].go_player);
			}

			array_allPlayersInfos[_i_idPlayerInGame].go_player = 
				GameObject.Instantiate (GlobalVariables.GO_PLAYER, new Vector3(-25f, 2f, 0f), Quaternion.identity) as GameObject;

			array_allPlayersInfos [_i_idPlayerInGame].go_player.networkView.viewID = _viewID;

			array_allPlayersInfos [_i_idPlayerInGame].go_player.GetComponent<PlayerAvatar> ().initThisPlayer (_i_idPlayerInGame, i_idMyPlayerInGame);
		}
	}

	[RPC]
	public void updateGameInfoRPC(int _i_idUpdatePlayerInGame, int _i_idUpdatePlayerInNetwork, int _i_updateMode)
	{
		updateGameInfoLocal (_i_idUpdatePlayerInGame, _i_idUpdatePlayerInNetwork, _i_updateMode);

	}

	public void updateGameInfoLocal(int _i_idUpdatePlayerInGame, int _i_idUpdatePlayerInNetwork, int _i_updateMode)
	{
		if(_i_updateMode == NEW_PLAYER_JOINED)
		{
			//not need now
		}
		else if(_i_updateMode == EXIST_PLAYER_EXITED)
		{
			gameInfo.setPlayerIdInNetworkByIndex(_i_idUpdatePlayerInGame, -1);
			gameInfo.setPlayerNameByIndex(_i_idUpdatePlayerInGame, null);
			
			iArray_idOfAllPlayers[_i_idUpdatePlayerInGame] = -1;
			sArray_nameOfAllPlayers[_i_idUpdatePlayerInGame] = null;
			
			array_allPlayersInfos[_i_idUpdatePlayerInGame].i_idPlayerInGame = -1;
			array_allPlayersInfos[_i_idUpdatePlayerInGame].i_idPlayerInNetwork = -1;
			array_allPlayersInfos[_i_idUpdatePlayerInGame].s_playerName = null;
//			array_allPlayersInfos[_i_idUpdatePlayerInGame].f_healthCurrentLife = GlobalVariables.F_FULL_HEALTH_PLAYER;   //need assign value after
			array_allPlayersInfos[_i_idUpdatePlayerInGame].i_sidePlayer = -1;

			if(array_allPlayersInfos[_i_idUpdatePlayerInGame].go_player != null)
			{
				Destroy(array_allPlayersInfos[_i_idUpdatePlayerInGame].go_player);
			}
		}
	}

	void OnGUI()
	{
		if(GUI.Button(new Rect(500, 25, 150, 30), "Back"))
		{
			b_quitGame = true;
		}
	}


}
















