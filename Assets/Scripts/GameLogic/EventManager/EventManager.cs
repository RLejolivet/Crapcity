using System;
using System.Collections;

//public enum Enum_eventType
//{
//	NON,
//	ON_BUILDING_CONSTRUCTED,
//	ON_BUILDING_DESTROYED
//}

public delegate void BuildingEventHandler();

public class EventManager{

	public static event BuildingEventHandler BuildingConstructed;
	public static event BuildingEventHandler BuildingDestroyed;

	// Use this for initialization
	void Start () {
	
	}

	public static void onBuildingConstructed()
	{
		if(BuildingConstructed != null)
		{
			BuildingConstructed();
		}
	}

	public static void onBuildingDestroyed()
	{
		if(BuildingDestroyed != null)
		{
			BuildingDestroyed();
		}
	}
}