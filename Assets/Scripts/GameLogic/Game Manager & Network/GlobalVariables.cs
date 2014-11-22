using UnityEngine;
using System.Collections;

public class GlobalVariables : MonoBehaviour {

	public static GameObject GO_PLAYER;
	public GameObject go_player;

	public static float F_FULL_HEALTH_PLAYER;
	public float f_fullHealthPlayer;

	public static GameInfo GO_GAMEINFO;
	public GameInfo go_gameInfo;

	public static GameObject GO_MAP;
	public GameObject go_map;

	public static Texture[] ARRAY_TEXTURE_BUTTON_SEARCH;
	public Texture[] array_textureButtonSearch;

	// Use this for initialization
	void Start () {

		GO_PLAYER = go_player;

		F_FULL_HEALTH_PLAYER = f_fullHealthPlayer;

		GO_GAMEINFO = go_gameInfo;

		GO_MAP = go_map;

		ARRAY_TEXTURE_BUTTON_SEARCH = new Texture[array_textureButtonSearch.Length];
		for(int i = 0; i < ARRAY_TEXTURE_BUTTON_SEARCH.Length; i++)
		{
			ARRAY_TEXTURE_BUTTON_SEARCH[i] = array_textureButtonSearch[i];
		}
	}
}
