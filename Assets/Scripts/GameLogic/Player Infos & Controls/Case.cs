using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Possibility { Nothing, Building, Waste, BuildAndWaste };

public class Case {

	/**
	 * The class Case determines the contents of a compartment (wether there is a building, wastes ... in it 
	 **/
	public Possibility contains;
	public float nbWaste;  // Indicates the quantity of Waste on this compartment
	public Building bat; // A reference to the Building on this compartment if there is any
	
	/**
	 * Create an empty Case 
	 **/
	
	public Case()
	{
		contains = Possibility.Nothing;
		nbWaste = 0;
		bat = null;
	}
	
	
	/**
	 * Build a building on a compartment
	 **/
	
	public bool build(string name, Dictionary<string, float> dic_resourcesPlayer)
	{
        Building build = BuildingFactory.Instance.create(name);
		if (build != null)
		{
			switch (contains)
			{
				case Possibility.Nothing : 
					contains = Possibility.Building;
					bat = build;
					bat.getCost(dic_resourcesPlayer);
					break;
				case Possibility.Waste : 
					contains = Possibility.BuildAndWaste;
					bat = build;
					bat.getCost(dic_resourcesPlayer);
					break;
				default : 
					// Shouldn't happen
					Debug.Log("You're trying to build something over a building");
					break;
			}
		}
        return true;
	}
	
	public void buildAuto(string name)
	{
		Building build = BuildingFactory.Instance.create(name);
		if (build != null)
		{
			switch (contains)
			{
				case Possibility.Nothing : 
					contains = Possibility.Building;
					bat = build;
					break;
				case Possibility.Waste : 
					contains = Possibility.BuildAndWaste;
					bat = build;
					break;
				default : 
					// Shouldn't happen
					Debug.Log("You're trying to build something over a building");
					break;
			}
		}
		
	}
	
	public bool destroyBuilding()
	{
		switch (contains)
			{
				case Possibility.Building : 
					contains = Possibility.Nothing;
					// Augmenter le nombre de déchets du côté de player 
					// A FAIRE
					if (BuildingFactory.Instance.release(bat))
						bat = null;
					return true;
				case Possibility.BuildAndWaste : 
					contains = Possibility.Waste;
					// Augmenter le nombre de déchets du côté de player 
					// A FAIRE
					if (BuildingFactory.Instance.release(bat))
					{
						bat = null;
						return true;
					}
					else 
					{
						Debug.Log("Fail to release building");
						return false;
					}
				default : 
					// Shouldn't happen
					Debug.Log("Nothing to be destroyed");
					return false;
			}
	}
	
	
	public void getResources(Dictionary<string, float> total)
	{
		if (bat != null)
		{
			bat.getResources(total);
		}
	}
	
	public float getSendWastes(bool stock)
	{
		if (bat != null)
		{
			if (stock)
			{
				bat.f_current_stock += bat.getF_trade;
				if (bat.f_current_stock >= bat.getF_max_trade)
				{
					bat.f_current_stock = bat.getF_max_trade;
				}
			}
			return bat.f_current_stock;
		}
		return 0;
	}
}
