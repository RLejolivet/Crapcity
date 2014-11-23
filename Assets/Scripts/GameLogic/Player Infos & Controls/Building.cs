using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable()]
public class Building {

    public string s_nameBuilding = null;
    public Dictionary<string, float> dic_cost = new Dictionary<string, float>();
    public Dictionary<string, float> dic_incomes = new Dictionary<string, float>();
    public float f_trade = -1;
    public float f_max_trade = -1;
    public float f_buildTime = -1;
    public float f_storage = -1;
    public BuildingTemplate origin;
    public bool is_hacked = false;
	public float f_current_stock = 0;

	public Building(BuildingTemplate b)
	{
		s_nameBuilding = b.Name;
		dic_cost = new Dictionary<string,float>(b.Cost);
        dic_incomes = new Dictionary<string, float>(b.Incomes);
		f_trade = b.Trade;
        f_max_trade = b.Max_Trade;
        f_buildTime = b.BuildTime;
        f_storage = b.Storage;
        origin = b;
	}

	public string getNameBuilding()
	{
		return s_nameBuilding;
	}

	private bool canBuild(Dictionary<string, float> dic_resourcesPlayer)
	{
		foreach(KeyValuePair<string, float> inc in this.dic_cost)
		{
			if (dic_resourcesPlayer[inc.Key] < inc.Value)
			{
				return false;
			}
		}
		return true;
	}
	
	public bool getCost(Dictionary<string, float> dic_resourcesPlayer)
	{
		bool res = true;
		if (canBuild(dic_resourcesPlayer)) //canbuild return true if there are enough resources to build
		{
			foreach(KeyValuePair<string, float> inc in this.dic_cost)
			{
				dic_resourcesPlayer[inc.Key] -= inc.Value;
			}
			return true;
		}
		return false;
	}
	
	
	public void getResources(Dictionary<string, float> total)
	{
		foreach(KeyValuePair<string, float> inc in this.dic_incomes)
			{
				if (total.ContainsKey(inc.Key))
				{
					total[inc.Key] += inc.Value;
				}
				else 
				{
					total.Add(inc.Key, inc.Value);
				}
			}
	}
	
	public float getF_max_trade
	{
		get
		{
			return f_max_trade;
		}
	}
	
	public float getF_trade
	{
		get
		{
			return f_trade;
		}
	}
	
}
