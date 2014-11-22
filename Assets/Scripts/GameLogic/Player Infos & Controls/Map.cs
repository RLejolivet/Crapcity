using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map {

	/** 
	 * Map contains all the container of a player base
	 **/

	private List<Case> terrain;
	public int terrainSize ()
	{
		if (terrain != null)
			return (terrain.Count);
		return -1; 
	}		
			
			
	public Map(int nbCases)
	{
		terrain = new List<Case>();
		for (int i=0; i < nbCases; i++)
		{
			terrain.Add(new Case());
		}
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
}
