using UnityEngine;
using System.Collections;

public class GlobalVariables : MonoBehaviour {

	public static GameObject GO_PLAYER;
	public GameObject go_player;

	public static float F_FULL_HEALTH_PLAYER;
	public float f_fullHealthPlayer;

	public static GameInfo GO_GAMEINFO;
	public GameInfo go_gameInfo;

	public static Texture[] ARRAY_TEXTURE_BUTTON_SEARCH;
	public Texture[] array_textureButtonSearch;

	public static Texture[] ATEX_DECHET;
	public Texture[] aTex_Dechet;

	public static Texture[] ATEX_DECHETTERIE;
	public Texture[] aTex_Dechetterie;

	public static Texture[] ATEX_TRANSPORT;
	public Texture[] aTex_transport;

	public static Texture[] ATEX_USINE;
	public Texture[] aTex_usine;

	public static Texture[] ATEX_HACK;
	public Texture[] aTex_hack;

	public static Texture[] ATEX_RECYCLE;
	public Texture[] aTex_recycle;

	public static int I_RESOURCEPSITIVE_USINE;
	public int i_resourcePositiveUsine;

	public static int I_RESOURCENEGATIVE_USINE;
	public int i_resourceNegativeUsine;

	public static int I_RESOURCEPSITIVE_HACK;
	public int i_resourcePositiveHack;
	
	public static int I_RESOURCENEGATIVE_HACK;
	public int i_resourceNegativeHack;

	public static int I_RESOURCEPSITIVE_RECYCLE;
	public int i_resourcePositiveRecycle;
	
	public static int I_RESOURCENEGATIVE_HRECYCLE;
	public int i_resourceNegativeRecycle;

	public static int I_RESOURCEPSITIVE_TRANSPORT;
	public int i_resourcePositiveTransport;
	
	public static int I_RESOURCENEGATIVE_TRANSPORT;
	public int i_resourceNegativeTransport;

    void Awake()
    {
        GO_PLAYER = go_player;
		

        F_FULL_HEALTH_PLAYER = f_fullHealthPlayer;

        GO_GAMEINFO = go_gameInfo;

		ARRAY_TEXTURE_BUTTON_SEARCH = new Texture[array_textureButtonSearch.Length];
		for(int i = 0; i < ARRAY_TEXTURE_BUTTON_SEARCH.Length; i++)
		{
			ARRAY_TEXTURE_BUTTON_SEARCH[i] = array_textureButtonSearch[i];
		}
		
		ATEX_DECHET = new Texture[aTex_Dechet.Length];
		for(int i = 0; i < ATEX_DECHET.Length; i++)
		{
			ATEX_DECHET[i] = aTex_Dechet[i];
		}
		
		ATEX_DECHETTERIE = new Texture[aTex_Dechetterie.Length];
		for(int i = 0; i < ATEX_DECHETTERIE.Length; i++)
		{
			ATEX_DECHETTERIE[i] = aTex_Dechetterie[i];
		}

        ARRAY_TEXTURE_BUTTON_SEARCH = new Texture[array_textureButtonSearch.Length];
        for (int i = 0; i < ARRAY_TEXTURE_BUTTON_SEARCH.Length; i++)
        {
            ARRAY_TEXTURE_BUTTON_SEARCH[i] = array_textureButtonSearch[i];
        }

		ARRAY_TEXTURE_BUTTON_SEARCH = new Texture[array_textureButtonSearch.Length];
		for(int i = 0; i < ARRAY_TEXTURE_BUTTON_SEARCH.Length; i++)
		{
			ARRAY_TEXTURE_BUTTON_SEARCH[i] = array_textureButtonSearch[i];
		}
		
		ATEX_DECHET = new Texture[aTex_Dechet.Length];
		for(int i = 0; i < ATEX_DECHET.Length; i++)
		{
			ATEX_DECHET[i] = aTex_Dechet[i];
		}
		
		ATEX_DECHETTERIE = new Texture[aTex_Dechetterie.Length];
		for(int i = 0; i < ATEX_DECHETTERIE.Length; i++)
		{
			ATEX_DECHETTERIE[i] = aTex_Dechetterie[i];
		}

		ATEX_TRANSPORT = new Texture[aTex_transport.Length];
		for(int i = 0; i < ATEX_TRANSPORT.Length; i++)
		{
			ATEX_TRANSPORT[i] = aTex_transport[i];
		}

		ATEX_USINE = new Texture[aTex_usine.Length];
		for(int i = 0; i < ATEX_USINE.Length; i++)
		{
			ATEX_USINE[i] = aTex_usine[i];
		}

		ATEX_HACK = new Texture[aTex_hack.Length];
		for(int i = 0; i < ATEX_HACK.Length; i++)
		{
			ATEX_HACK[i] = aTex_hack[i];
		}

		ATEX_RECYCLE = new Texture[aTex_recycle.Length];
		for(int i = 0; i < ATEX_RECYCLE.Length; i++)
		{
			ATEX_RECYCLE[i] = aTex_recycle[i];
		}

		I_RESOURCEPSITIVE_HACK = i_resourceNegativeHack;
		I_RESOURCEPSITIVE_RECYCLE = i_resourceNegativeRecycle;
		I_RESOURCEPSITIVE_TRANSPORT = i_resourceNegativeTransport;
		I_RESOURCEPSITIVE_USINE = i_resourceNegativeUsine;
	}
}
