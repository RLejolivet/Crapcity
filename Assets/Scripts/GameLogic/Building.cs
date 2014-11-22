using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building {

	private string s_nameBuilding = null;
	private Dictionary<string, float> dic_cost = new Dictionary<string, float>();
	private Dictionary<string, float> dic_incomes = new Dictionary<string, float>();
	private float f_trade = -1;
    private float f_max_trade = -1;
	private float f_buildTime = -1;
    private float f_storage = -1;
    private BuildingTemplate origin;

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

}
