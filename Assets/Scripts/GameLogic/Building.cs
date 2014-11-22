using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building {

	private string s_nameBuilding = null;
	private Dictionary<string, float> dic_cost = new Dictionary<string, float>();
	private Dictionary<string, float> dic_incomes = new Dictionary<string, float>();
	private float f_trade = -1;
	private float f_buildTime = -1;
    private float f_storage = -1;
    private BuildingTemplate origin;

	public Building(BuildingTemplate b)
	{
		s_nameBuilding = b.Name;
		dic_cost = new Dictionary<string,float>(b.Cost);
        dic_incomes = new Dictionary<string, float>(b.Incomes);
		f_trade = b.Trade;
        f_buildTime = b.BuildTime;
        f_storage = b.Storage;
        origin = b;
	}

	public string getNameBuilding()
	{
		return s_nameBuilding;
	}

	// Use this for initialization
	void Start () {
		initBuildingEvents ();
	}

	public void initBuildingEvents()
	{
		if(EventManager.BuildingConstructed == null)
		{
			EventManager.BuildingConstructed += new BuildingEventHandler(OnBuildingConstructed);
			while(EventManager.BuildingConstructed.GetInvocationList().Length > 1)
			{
				EventManager.BuildingConstructed -= new BuildingEventHandler(OnBuildingConstructed);
			}
		}
		
		if(EventManager.BuildingDestroyed == null)
		{
			EventManager.BuildingDestroyed += new BuildingEventHandler(OnBuildingDestroyed);
			while(EventManager.BuildingDestroyed.GetInvocationList().Length > 1)
			{
				EventManager.BuildingDestroyed -= new BuildingEventHandler(OnBuildingDestroyed);
			}
		}
	}

	public void OnBuildingConstructed()
	{

	}

	public void OnBuildingDestroyed()
	{

	}
}
