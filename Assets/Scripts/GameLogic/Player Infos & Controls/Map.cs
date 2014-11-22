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
			return (terrain.Count());
		return -1;
	}		
			
			
	public Map(int nbCases)
	{
		Map map = new List<Case>();
		for (i=0; i < nbCases; i++)
		{
			map.Add(Case());
		}
		return map;
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
