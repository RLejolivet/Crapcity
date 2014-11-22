using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building{

	private string s_nameBuilding = null;
	private Dictionary<string, float> dic_cost = new Dictionary<string, float>();
	private Dictionary<string, float> dic_resources = new Dictionary<string, float>();
	private float f_trade = -1;
	private float f_recycle = -1;
	private float f_buildTime = -1;

	public Building(string _s_nameBuilding, Dictionary<string, float> _dic_cost, Dictionary<string, float> _dic_resources,
	                float _f_trade, float _f_recycle, float _f_buildTime)
	{
		s_nameBuilding = _s_nameBuilding;
		dic_cost = _dic_cost;
		dic_resources = _dic_resources;
		f_trade = _f_trade;
		f_recycle = _f_recycle;
		f_buildTime = _f_buildTime;
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
