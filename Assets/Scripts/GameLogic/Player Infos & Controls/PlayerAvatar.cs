using UnityEngine;
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
	private Dictionary<string, float> dic_resourcesPlayer = new Dictionary<string, float>();

	public void initThisPlayer(int _i_idThisPlayerInGame, int _i_idMyPlayerInGame, int _i_sidePlayer, string _s_namePlayer)
	{
		i_idThisPlayerInGame = _i_idThisPlayerInGame;
		i_idMyPlayerInGame = _i_idMyPlayerInGame;
		i_sidePlayer = _i_sidePlayer;
		_s_namePlayer = s_namePlayer;
		//dic_resourcesPlayer = _dic_resourcesPlayer;
		initMap ();
	}

	// initialization of the map with sizeMap building emplacement 
	public void initMap ()
	{
		map_myPlayer = new Map(sizeMap);
	}

	// Upgrade the number of resources that this players owner
	public void updateResource()
	{
		map_myPlayer.getResources(dic_resourcesPlayer);
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
