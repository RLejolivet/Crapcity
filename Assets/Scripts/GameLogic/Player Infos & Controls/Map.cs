using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map {

	/** 
	 * Map contains all the container of a player base
	 **/

	public List<Case> terrain;
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
	
	public float getSendWastes(bool stock)
	{
		float ret = 0f;
		if (terrain != null)
		{
			foreach(Case c in terrain)
			{
				ret += c.getSendWastes(stock);
			}
		}
		else 
		{
			Debug.Log("terrain non initialisé");
		}
		return (ret);
	}

	public bool destroyBuildOnCase(int i)
	{
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

	public bool createBuildingOnCase(int i, string name, Dictionary<string, float> dic_resourcesPlayer)
    {
        return (terrain[i].build(name, dic_resourcesPlayer));
    }
	
	public void build(int ncase, string buildingname)
	{
		terrain[ncase].buildAuto(buildingname);
	}
}
