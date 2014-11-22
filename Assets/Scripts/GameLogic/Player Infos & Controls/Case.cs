using UnityEngine;
using System.Collections;

public class Case {

	/**
	 * The class Case determines the contents of a compartment (wether there is a building, wastes ... in it 
	 **/
	public enum Possibility { Nothing, Building, Waste, BuildAndWaste };
	public Possibility contains;
	public int nbWaste;  // Indicates the quantity of Waste on this compartment
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
	
	public void build(Building build)
	{
		if (build != null)
		{
			switch (contains)
			{
				case Possibility.Nothing : 
					contains = Possibility.Building;
					break;
				case Possibility.Waste : 
					contains = Possibility.BuildAndWaste;
					bat = build;
					break;
				default : 
					// Shouldn't happen
					Debug.Log("You try to build something over a building");
					break;
			}
		}	
	}
	
}
