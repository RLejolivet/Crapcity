using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("resource")]
[XmlType("resource")]
public class Resources {

    [XmlElement("name")]
    public string Name
    {
        get;
        set;
    }

    [XmlElement("sprite_path")]
    public string spritePath
    {
        get;
        set;
    }

    private Sprite icon;
}
