using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum Enum_guiShowMode
{
	NON,
	BUILDINGLIST,
	BUILDINGDESCRIPTION,
	WASTED
}

public enum Enum_caseContent
{
	NON,
	WASTED1,
	WASTED2,
	BUILD_RECYCLE,
	BUILD_HACK,
	BUILD_USINE,
	BUILD_TRANSPORT,
	BUILD_DECHETTERIE1,
	BUILD_DECHETTERIE2,
	BUILD_DECHETTERIE3,
}

public class GameManager: MonoBehaviour {

	public GUITexture guitex_buttonSearch;
	public GUITexture guitex_iconMateriaux;
	public GUITexture guitex_iconDechets;
	public GUIText guitex_numMat;
	public GUIText guitex_numDec;
	public GUITexture guitex_map;
	public GUITexture[] guitex_caseContainer;
	public GUITexture[] guitex_buildingContainer;
	public GUIText guitex_buildingDescription;
	public GUIText[] guitex_buildingCost;

	private Enum_guiShowMode enum_guiShowMode = Enum_guiShowMode.NON; 
	private Enum_caseContent[] array_caseContent = new Enum_caseContent[6];
	private Enum_caseContent[] array_caseConentEnemy = new Enum_caseContent[6];

	private int i_wishBuildingCase = -1;

	private int i_maxNumPlayers = 1;

	private string[] sArray_nameOfAllPlayers;
	private int[] iArray_idOfAllPlayers;
	private int[] iArray_sideOfAllPlayers;
	private const int NEW_PLAYER_JOINED = 1;
	private const int EXIST_PLAYER_EXITED = 2;

	private Enum_playMode playMode = Enum_playMode.Non;
//	private GUIManager guiManager;
	private GameInfo gameInfo;
    private List<Resources> listResources;

	private class PlayerInfo
	{
		public int i_idPlayerInGame = -1;
		public int i_idPlayerInNetwork = -1;
		public string s_playerName = null;
		public int i_sidePlayer = -1; // for using in the future: player VS player
		public GameObject go_player = null;
		public Map go_map = null;
	}

    private int frame;
    private bool is_sending = true;

	private PlayerInfo[] array_allPlayersInfos;
	private int i_idMyPlayerInGame = -1;
	private int i_idMyPlayerInNetwork = -1;

	private bool b_quitGame = false;   //player can set this in the inputController when clicking the quitGame button

	public bool B_quitGame
	{
		get{return b_quitGame;}
		set{b_quitGame = value;}
	}

	// Use this for initialization
    void Start()
    {
		for(int i = 0; i < 6; i++)
		{
			array_caseContent[i] = Enum_caseContent.NON;
		}

        initResources();
		initGame ();
		initMap (i_idMyPlayerInGame);
	}

    private void initResources()
    {
        TextAsset resourcesFile;
        resourcesFile = (TextAsset)UnityEngine.Resources.Load("Xml/Resources");
        listResources = XmlHelpers.LoadFromTextAsset<Resources>(resourcesFile);
    }

	private void initGame()
	{
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
			array_allPlayersInfos[i].i_sidePlayer = iArray_sideOfAllPlayers[i];
			array_allPlayersInfos[i].go_player = null;
			array_allPlayersInfos[i].go_map = null;

			if(iArray_idOfAllPlayers[i] == int.Parse(Network.player.ToString()))
			{
				i_idMyPlayerInGame = i;
				//inputController.initMyPlayer();
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
	
	public void initMap(int _i_idMyPlayerInGame)
	{
		networkView.RPC ("initMapRPC", RPCMode.All, _i_idMyPlayerInGame);
	}

	[RPC]
	public void initMapRPC(int _i_idMyPlayerInGame)
	{
		initMapLocal (_i_idMyPlayerInGame);
	}

	public void initMapLocal(int _i_idMyPlayerInGame)
	{
		if(_i_idMyPlayerInGame == i_idMyPlayerInGame)
		{
			createMap (_i_idMyPlayerInGame);
		}
	}

	public void createMap(int _i_idMyPlayerInGame)
	{
		Vector3 v3_posMap = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width * 0.3f, Screen.height * 0.5f, Camera.main.nearClipPlane));
        array_allPlayersInfos[_i_idMyPlayerInGame].go_player.GetComponent<PlayerAvatar>().initMap();
        array_allPlayersInfos[_i_idMyPlayerInGame].go_map = array_allPlayersInfos[_i_idMyPlayerInGame].go_player.GetComponent<PlayerAvatar>().getMap();
	}

	// Update is called once per frame
	void Update () {
        /**
         * Time management
         **/
        frame++;
        if (frame % 60 == 0)
        {
            foreach(PlayerInfo i in array_allPlayersInfos)
            {
                if (i_idMyPlayerInGame  == i.i_idPlayerInGame)
                {
                    i.go_player.GetComponent<PlayerAvatar>().updateResource();
                    i.go_player.GetComponent<PlayerAvatar>().getSendWastes(is_sending);
                }
            }
        }


        /** 
         * Player input management
         **/
		#region inputManager
        if (guitex_buttonSearch.HitTest(Input.mousePosition))
		{
			if(Input.GetMouseButton(0))
			{
				if(Input.GetMouseButtonDown(0))
				{
					guitex_buttonSearch.texture = GlobalVariables.ARRAY_TEXTURE_BUTTON_SEARCH[2];
				}
			}
			else
			{
				guitex_buttonSearch.texture = GlobalVariables.ARRAY_TEXTURE_BUTTON_SEARCH[1];
			}
		}
		else
		{
			guitex_buttonSearch.texture = GlobalVariables.ARRAY_TEXTURE_BUTTON_SEARCH[0];
		}

		for(int i = 0; i < guitex_caseContainer.Length; i++)
		{
			if(Input.GetMouseButtonDown(0) && guitex_caseContainer[i].HitTest(Input.mousePosition))
			{
				if(array_caseContent[i] == Enum_caseContent.NON)
				{
					enum_guiShowMode = Enum_guiShowMode.BUILDINGLIST;
					i_wishBuildingCase = i;

					guitex_buildingContainer[0].texture = GlobalVariables.ATEX_TRANSPORT[2];
					guitex_buildingCost[0].text = "inputTheCost";
					guitex_buildingContainer[1].texture = GlobalVariables.ATEX_RECYCLE[2];
					guitex_buildingCost[1].text = "inputTheCost";
					guitex_buildingContainer[2].texture = GlobalVariables.ATEX_HACK[2];
					guitex_buildingCost[2].text = "inputTheCost";
					guitex_buildingContainer[3].texture = GlobalVariables.ATEX_USINE[2];
					guitex_buildingCost[3].text = "inputTheCost";
				}
				else
				{
					i_wishBuildingCase = -1;

					guitex_buildingContainer[0].texture = null;
					guitex_buildingCost[0].text = "";
					guitex_buildingContainer[1].texture = null;
					guitex_buildingCost[1].text = "";
					guitex_buildingContainer[2].texture = null;
					guitex_buildingCost[2].text = "";
					guitex_buildingContainer[3].texture = null;
					guitex_buildingCost[3].text = "";
				}

				if(array_caseContent[i] == Enum_caseContent.BUILD_DECHETTERIE1
				        || array_caseContent[i] == Enum_caseContent.BUILD_DECHETTERIE2
				        || array_caseContent[i] == Enum_caseContent.BUILD_DECHETTERIE3
				        || array_caseContent[i] == Enum_caseContent.BUILD_HACK
				        || array_caseContent[i] == Enum_caseContent.BUILD_RECYCLE
				        || array_caseContent[i] == Enum_caseContent.BUILD_TRANSPORT
				        || array_caseContent[i] == Enum_caseContent.BUILD_USINE)
				{
					enum_guiShowMode = Enum_guiShowMode.BUILDINGDESCRIPTION;

					guitex_buildingDescription.text = "BuildingDescription";
				}
				else
				{
					guitex_buildingDescription.text = "";
				}

				if(array_caseContent[i] == Enum_caseContent.WASTED1
				        || array_caseContent[i] == Enum_caseContent.WASTED2)
				{
					enum_guiShowMode = Enum_guiShowMode.WASTED;

					guitex_buildingContainer[0].texture = GlobalVariables.ATEX_DECHETTERIE[0];
					guitex_buildingCost[0].text = "inputTheCost";
				}
				else
				{
					if(enum_guiShowMode != Enum_guiShowMode.BUILDINGLIST)
					{
						guitex_buildingContainer[0].texture = null;
						guitex_buildingCost[0].text = "";
					}
				}
			}
		}

		if(enum_guiShowMode == Enum_guiShowMode.BUILDINGLIST 
		   || enum_guiShowMode == Enum_guiShowMode.WASTED)
		{
			for(int i = 0; i < 4; i++)
			{
				if(enum_guiShowMode != Enum_guiShowMode.WASTED)
				{
					if(Input.GetMouseButtonDown(0) && guitex_buildingContainer[i].HitTest(Input.mousePosition))
					{
						if(i_wishBuildingCase != -1 && array_caseContent[i_wishBuildingCase] == Enum_caseContent.NON)
						{
							guitex_caseContainer[i_wishBuildingCase].texture = guitex_buildingContainer[i].texture;
							if(i == 0)
							{
								array_caseContent[i_wishBuildingCase] = Enum_caseContent.BUILD_TRANSPORT;
							}
							else if(i == 1)
							{
								array_caseContent[i_wishBuildingCase] = Enum_caseContent.BUILD_RECYCLE;
							}
							else if(i == 2)
							{
								array_caseContent[i_wishBuildingCase] = Enum_caseContent.BUILD_HACK;
							}
							else if(i == 3)
							{
								array_caseContent[i_wishBuildingCase] = Enum_caseContent.BUILD_USINE;
							}

							i_wishBuildingCase = -1;
							enum_guiShowMode = Enum_guiShowMode.NON;

							guitex_buildingContainer[0].texture = null;
							guitex_buildingCost[0].text = "";
							guitex_buildingContainer[1].texture = null;
							guitex_buildingCost[1].text = "";
							guitex_buildingContainer[2].texture = null;
							guitex_buildingCost[2].text = "";
							guitex_buildingContainer[3].texture = null;
							guitex_buildingCost[3].text = "";
						}
					}
				}
				else
				{
					if(Input.GetMouseButtonDown(0) && guitex_buildingContainer[0].HitTest(Input.mousePosition))
					{
						if(i_wishBuildingCase != -1 && 
						   (array_caseContent[i_wishBuildingCase] == Enum_caseContent.WASTED1
						 || array_caseContent[i_wishBuildingCase] == Enum_caseContent.WASTED2))
						{
							guitex_caseContainer[i_wishBuildingCase].texture = guitex_buildingContainer[0].texture;
							array_caseContent[i_wishBuildingCase] = Enum_caseContent.BUILD_DECHETTERIE1;

							i_wishBuildingCase = -1;
							enum_guiShowMode = Enum_guiShowMode.NON;

							guitex_buildingContainer[0].texture = null;
							guitex_buildingCost[0].text = "";
						}
					}
					i = 4;
				}
			}
		}

		int[] caseContentBuffer = new int[6];
		for(int i = 0; i < 6; i++)
		{
			switch (array_caseContent[i])
			{
			case Enum_caseContent.NON:
				caseContentBuffer[i] = 1;
				break;
			case Enum_caseContent.WASTED1:
				caseContentBuffer[i] = 2;
				break;
			case Enum_caseContent.WASTED2:
				caseContentBuffer[i] = 3;
				break;
			case Enum_caseContent.BUILD_RECYCLE:
				caseContentBuffer[i] = 4;
				break;
			case Enum_caseContent.BUILD_HACK:
				caseContentBuffer[i] = 5;
				break;
			case Enum_caseContent.BUILD_USINE:
				caseContentBuffer[i] = 6;
				break;
			case Enum_caseContent.BUILD_TRANSPORT:
				caseContentBuffer[i] = 7;
				break;
			case Enum_caseContent.BUILD_DECHETTERIE1:
				caseContentBuffer[i] = 8;
				break;
			case Enum_caseContent.BUILD_DECHETTERIE2:
				caseContentBuffer[i] = 9;
				break;
			case Enum_caseContent.BUILD_DECHETTERIE3:
				caseContentBuffer[i] = 10;
				break;
			default:
				break;
			}
		}
		networkView.RPC("updateCaseContentRPC", RPCMode.Others, caseContentBuffer);
		#endregion inputManager
	}

	[RPC]
	public void updateCaseContentRPC(int[] caseContentBuffer)
	{
		updateCaseContentLocal (caseContentBuffer);
	}

	public void updateCaseContentLocal(int[] caseContentBuffer)
	{
		for(int i = 0; i < 6; i++)
		{
			switch (caseContentBuffer[i])
			{
			case 1:
				array_caseConentEnemy[i] = Enum_caseContent.NON;
				break;
			case 2:
				array_caseConentEnemy[i] = Enum_caseContent.WASTED1;
				break;
			case 3:
				array_caseConentEnemy[i] = Enum_caseContent.WASTED2;
				break;
			case 4:
				array_caseConentEnemy[i] = Enum_caseContent.BUILD_RECYCLE;
				break;
			case 5:
				array_caseConentEnemy[i] = Enum_caseContent.BUILD_HACK;
				break;
			case 6:
				array_caseConentEnemy[i] = Enum_caseContent.BUILD_USINE;
				break;
			case 7:
				array_caseConentEnemy[i] = Enum_caseContent.BUILD_TRANSPORT;
				break;
			case 8:
				array_caseConentEnemy[i] = Enum_caseContent.BUILD_DECHETTERIE1;
				break;
			case 9:
				array_caseConentEnemy[i] = Enum_caseContent.BUILD_DECHETTERIE2;
				break;
			case 10:
				array_caseConentEnemy[i] = Enum_caseContent.BUILD_DECHETTERIE3;
				break;
			default:
				break;
			}
		}
	}

	void OnGUI()
	{
		Vector3 v3_posButtonSearch = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width * 0.3f, Screen.height * 0.2f, Camera.main.nearClipPlane));

		if(GUI.Button(new Rect(500, 25, 150, 30), "Back"))
		{
			b_quitGame = true;
		}
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
//			myPlayer = GameObject.Instantiate(GlobalVariables.GO_PLAYER, new Vector3(-25f, 2f, 0f), Quaternion.identity) as GameObject;
			array_allPlayersInfos[i_idMyPlayerInGame].go_player =
				GameObject.Instantiate(GlobalVariables.GO_PLAYER, new Vector3(-25f, 2f, 0f), Quaternion.identity) as GameObject;
		}
	}

	public void server_createPlayer(int _i_idPlayerInGame, int _i_idPlayerInNetwork)
	{
		if(Network.isServer)
		{

			if(array_allPlayersInfos[_i_idPlayerInGame].go_player != null)
			{
				Destroy(array_allPlayersInfos[_i_idPlayerInGame].go_player);
			}

			array_allPlayersInfos[_i_idPlayerInGame].go_player = 
				GameObject.Instantiate (GlobalVariables.GO_PLAYER, new Vector3(-25f, 2f, 0f), Quaternion.identity) as GameObject;
			
			array_allPlayersInfos [_i_idPlayerInGame].go_player.GetComponent<PlayerAvatar> ()
					.initThisPlayer (_i_idPlayerInGame, i_idMyPlayerInGame, iArray_sideOfAllPlayers[_i_idPlayerInGame], sArray_nameOfAllPlayers[_i_idPlayerInGame], listResources);
			
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

			if(array_allPlayersInfos[_i_idPlayerInGame].go_player != null)
			{
				Destroy(array_allPlayersInfos[_i_idPlayerInGame].go_player);
			}

			array_allPlayersInfos[_i_idPlayerInGame].go_player = 
				GameObject.Instantiate (GlobalVariables.GO_PLAYER, new Vector3(-25f, 2f, 0f), Quaternion.identity) as GameObject;

			array_allPlayersInfos [_i_idPlayerInGame].go_player.networkView.viewID = _viewID;

			array_allPlayersInfos [_i_idPlayerInGame].go_player.GetComponent<PlayerAvatar> ()
						.initThisPlayer (_i_idPlayerInGame, i_idMyPlayerInGame, iArray_sideOfAllPlayers[_i_idPlayerInGame],
				                 		sArray_nameOfAllPlayers[_i_idPlayerInGame], listResources);
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
			array_allPlayersInfos[_i_idUpdatePlayerInGame].i_sidePlayer = -1;
			array_allPlayersInfos[_i_idUpdatePlayerInGame].go_map = null;

			if(array_allPlayersInfos[_i_idUpdatePlayerInGame].go_player != null)
			{
				Destroy(array_allPlayersInfos[_i_idUpdatePlayerInGame].go_player);
			}
		}
	}
}

















