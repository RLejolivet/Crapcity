using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("BuildingTemplate")]
[XmlType("LevelDescription")]
public class Resources {

    [XmlAttribute]
    public string name
    {
        get;
        set;
    }

    [XmlAttribute]
    public string spritePath
    {
        get;
        set;
    }

    private Sprite icon;
}
