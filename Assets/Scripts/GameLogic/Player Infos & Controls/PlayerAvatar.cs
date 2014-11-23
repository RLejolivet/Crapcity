using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAvatar : MonoBehaviour {
	// We can choose the size of a map
	[SerializeField]
	private int sizeMap = 12;
	
	private int i_idThisPlayerInGame = -1;
	private int i_idMyPlayerInGame = -2;  //for avoid bugs in Update() of InputController, use different number with i_idThisPlayerInGame
	private int i_sidePlayer = -1;

	private string s_namePlayer = null;
	public Map map_myPlayer;
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

    public Dictionary<string, float> Resources
    {
        get
        {
            return dic_resourcesPlayer;
        }
    }

	// initialization of the map with sizeMap building emplacement 
	public void initMap ()
	{
        if (i_idThisPlayerInGame == i_idMyPlayerInGame)
        {
            map_myPlayer = new Map(sizeMap, networkView);
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
	public void getSendWastes(bool stock)
	{
		float readyToSend = map_myPlayer.getSendWastes(stock);
		if(dic_resourcesPlayer.ContainsKey("Negatif"))
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
