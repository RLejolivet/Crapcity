using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map {

	/** 
	 * Map contains all the container of a player base
	 **/

	private List<Case> terrain;
	public int Terrain 
	{
		get 
		{
			if (terrain != null)
				return (terrain.Count);
			else
				return -1;
		}
		set
		{
			for (int i=0; i<value; i++)
			{
				terrain.Add(new Case());  // create the beginning base and set the number of emplacement
			}
		}
	}
	
	
	/**
	 * Return the compartment i
	 **/
	public Case getCase(int i)
	{
		return terrain[i];
	}
}
