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
				return (terrain.Count());
		}
		set
		{
			for (i=0; i<value; i++)
			{
				terrain.Add(Case());  // create the beginning base and set the number of emplacement
			}
		}
	}
	
	
	/**
	 * Return the compartment i
	 **/
	public Case getCase(int i)
	{
		return terrain.Item(i);
	}
}
