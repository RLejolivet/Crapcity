﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map {

	/** 
	 * Map contains all the container of a player base
	 **/

	private List<Case> terrain;
    private NetworkView nv;

	public int terrainSize ()
	{
		if (terrain != null)
			return (terrain.Count);
		return -1; 
	}		
			
			
	public Map(int nbCases, NetworkView nv = null)
	{
		terrain = new List<Case>();
		for (int i=0; i < nbCases; i++)
		{
			terrain.Add(new Case());
		}
        this.nv = nv;
	}
	
	
	/**
	 * Return the compartment i
	 **/
	public Case getCase(int i)
	{
		if (terrain.Count > i)
		{
			return terrain[i];
		}
		return null;
	}
	
	public void getResources(Dictionary<string, float> dic_resourcesPlayer)
	{
		if (terrain != null)
		{
			foreach(Case c in terrain)
			{
				c.getResources(dic_resourcesPlayer);
			}
		}
		else 
		{
			Debug.Log("terrain non initialisé");
		}
	}
	
[RPC]	public bool destroyBuildOnCase(int i)
	{
        if (nv.isMine)
        {
            nv.RPC("destroyBuildingOnCase", RPCMode.OthersBuffered, i);
        }

		if (terrain.Count > i)
		{
			return (terrain[i].destroyBuilding());
		}
		else 
		{
			Debug.Log("Fail to destroy building on compartment : " + i);
			return false;
		}
	}

[RPC]    public bool createBuildingOnCase(int i, string name)
    {

        if (nv.isMine)
        {
            nv.RPC("createBuildingOnCase", RPCMode.OthersBuffered, i, name);
        }
        if (terrain.Count > i)
        {
            return (terrain[i].build(name));
        }
        else
        {
            Debug.Log("Fail to create building on compartment : " + i);
            return false;
        }
    }
}
