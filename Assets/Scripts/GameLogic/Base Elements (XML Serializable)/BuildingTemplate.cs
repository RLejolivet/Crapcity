using System.Xml.Serialization;
using System.Collections.Generic;

[XmlRoot("BuildingTemplate")]
[XmlType("LevelDescription")]
public class BuildingTemplate {

    [XmlAttribute]
    public string name
    {
        get;
        set;
    }

    [XmlAttribute]
    public float Cost
    {
        get;
        set;
    }

    /**
     * Il manque pour l'instant la List<resource, int>
     **/

    [XmlAttribute]
    public float Trade
    {
        get;
        set;
    }

    [XmlAttribute]
    public float Recycle
    {
        get;
        set;
    }

    [XmlAttribute]
    public float BuildTime
    {
        get;
        set;
    }

}