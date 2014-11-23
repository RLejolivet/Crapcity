using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingFactory {

    private static BuildingFactory _instance;

    public static BuildingFactory Instance
    {
        get
        {
            if (_instance == null)
                new BuildingFactory();
            return _instance;
        }
    }

    public Dictionary<string,BuildingTemplate> buildingTemplates;

    public BuildingFactory()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Debug.Log("BuildingFactory is supposed to be a singleton");
        }

        buildingTemplates = new Dictionary<string, BuildingTemplate>();

        TextAsset buildingFile;
        buildingFile = (TextAsset) UnityEngine.Resources.Load("Xml/Buildings");
        List<BuildingTemplate> buildingTemplatesList = XmlHelpers.LoadFromTextAsset<BuildingTemplate>(buildingFile);

        foreach (BuildingTemplate b in buildingTemplatesList)
        {
            try
            {
                buildingTemplates.Add(b.Name, b);
                //Debug.Log(b.Trade + " " + b.Max_Trade);
            }
            catch (System.ArgumentException)
            {
                Debug.Log("ArgumentAxception dans BuildingFactory");
            }
        }
    }
    
    public Building create(string templateName)
    {
        if (buildingTemplates.ContainsKey(templateName))
        {
            BuildingTemplate b = buildingTemplates[templateName];
            Building retour = new Building(b);
            return retour;
        }
        else
        {
            return null;
        }
    }

    public bool release(Building b)
    {
        return true;
    }
    
}
