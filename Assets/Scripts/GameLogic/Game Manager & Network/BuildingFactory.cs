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

    private Dictionary<string,BuildingTemplate> buildingTemplates;

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

        TextAsset buildingFile;
        buildingFile = (TextAsset) UnityEngine.Resources.Load("Xml/Buildings");
        List<BuildingTemplate> buildingTemplatesList = XmlHelpers.LoadFromTextAsset<BuildingTemplate>(buildingFile);

        foreach (BuildingTemplate b in buildingTemplatesList)
        {
            try
            {
                buildingTemplates.Add(b.Name, b);
            }
            catch (System.ArgumentException)
            {

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
