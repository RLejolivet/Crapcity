using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAvatar : MonoBehaviour {
	
	private int i_idThisPlayerInGame = -1;
	private int i_idMyPlayerInGame = -2;  //for avoid bugs in Update() of InputController, use different number with i_idThisPlayerInGame
	private int i_sidePlayer = -1;

	private string s_namePlayer = null;
	private Map map_myPlayer;
	private Dictionary<string, float> dic_resourcesPlayer = new Dictionary<string, float>();

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

	public void initMap ()
	{

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
