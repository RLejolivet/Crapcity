using System.Xml.Serialization;
using UnityEngine;

[XmlType("Resources")]
public class Resources {

    [XmlElement("Name")]
    public string Name
    {
        get;
        set;
    }

    [XmlElement("spritePath")]
    public string spritePath
    {
        get;
        set;
    }

    private Sprite icon;
}
