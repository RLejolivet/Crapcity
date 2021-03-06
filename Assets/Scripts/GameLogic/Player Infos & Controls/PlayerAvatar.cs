﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAvatar : MonoBehaviour {
	// We can choose the size of a map
	[SerializeField]
	private int sizeMap;
	
	private int i_idThisPlayerInGame = -1;
	private int i_idMyPlayerInGame = -2;  //for avoid bugs in Update() of InputController, use different number with i_idThisPlayerInGame
	private int i_sidePlayer = -1;

	private string s_namePlayer = null;
	private Map map_myPlayer;
	private Map map_opponent;
	private Dictionary<string, float> dic_resourcesPlayer = new Dictionary<string, float>();
	private float readyToSend = 0;
	
	public void initThisPlayer(int _i_idThisPlayerInGame, int _i_idMyPlayerInGame, int _i_sidePlayer, string _s_namePlayer, List<Resources> listResources)
	{
		i_idThisPlayerInGame = _i_idThisPlayerInGame;
		i_idMyPlayerInGame = _i_idMyPlayerInGame;
		i_sidePlayer = _i_sidePlayer;
		_s_namePlayer = s_namePlayer;

        foreach (Resources r in listResources)
        {
            dic_resourcesPlayer.Add(r.Name, 0);
        }
		initMap ();
	}

	// initialization of the map with sizeMap building emplacement 
	public void initMap ()
	{
        if (i_idThisPlayerInGame == i_idMyPlayerInGame)
        {
            map_myPlayer = new Map(sizeMap, networkView);
			map_opponent = new Map(sizeMap);
        }
        else
            map_myPlayer = new Map(sizeMap);
	}

    public Map getMap()
    {
        return map_myPlayer;
    }

	// Upgrade the number of resources that this players owner
	public void updateResource()
	{
		map_myPlayer.getResources(dic_resourcesPlayer);
	}
	
	
	// Count the n° of waste items ready to be sent
	public void getSendWastes(bool issending)
	{
		float readyToSend = map_myPlayer.getSendWastes(!issending);
		if (dic_resourcesPlayer.ContainsKey("Negatif"))
		{
			if (readyToSend > dic_resourcesPlayer["Negatif"])
			{
				readyToSend = dic_resourcesPlayer["Negatif"];
			}	
			if (this.networkView.isMine)
			{
				networkView.RPC("sendWaste", RPCMode.OthersBuffered, readyToSend);	
				dic_resourcesPlayer["Negatif"] -= readyToSend;
			}
			else
			{
				// shouldn't happen
				Debug.Log("Try to sendWaste when not allowed to");
			}
		}
	}
	
		// Send all the waste available to be sent
[RPC]	public void sendWaste(float qty)
	{
		dic_resourcesPlayer["Negatif"] += qty;
	}
	
	public void destroyBuildingOnCase(int ncase)
	{
		if(map_myPlayer.destroyBuildOnCase(ncase))
			networkView.RPC("sendDestroyBuilding", RPCMode.OthersBuffered, ncase);
	}
	
	
[RPC] public void sendDestroyBuilding(int ncase)
	{
		map_opponent.destroyBuildOnCase(ncase);
	}
	
	public void createBuildingOnCase(int ncase, string buildingname)
	{
		if (map_myPlayer.createBuildingOnCase(ncase, buildingname, dic_resourcesPlayer))
		{
			if (this.networkView.isMine)
			{
				networkView.RPC("sendCreateBuilding", RPCMode.OthersBuffered, ncase, buildingname);	
			}
			else
			{
				// shouldn't happen
				Debug.Log("Try to sendWaste when not allowed to");
			}
		}
	}
	
	
[RPC]	public void sendCreateBuilding(int ncase, string buildingname)
	{
		map_opponent.build(ncase, buildingname);
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

	}
}
