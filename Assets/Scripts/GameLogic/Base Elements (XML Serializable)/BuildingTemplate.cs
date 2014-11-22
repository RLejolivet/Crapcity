using UnityEngine;
using System.Xml.Serialization;
using System.Collections.Generic;


/**
 * Template used for designers to give us their building ideas easily
 * Imported from BuildingFactory, used to instantiate the Building class
 **/

[XmlRoot("building_template")]
[XmlType("building_template")]
public class BuildingTemplate {

    [XmlElement("name")]
    public string Name
    {
        get;
        set;
    }

    [XmlElement("cost")]
    public List<KeyValuePair<string, float>> ProxyCost
    {
        get
        {
            return new List<KeyValuePair<string, float>>(this.Cost);
        }
        set
        {
            this.Cost = new Dictionary<string, float>();
            foreach (var pair in value)
                this.Cost[pair.Key] = pair.Value;
        }
    }

    private Dictionary<string, float> _cost;

    [XmlIgnore]
    public Dictionary<string, float> Cost
    {
        get
        {
            if (_cost == null)
            {
                _cost = new Dictionary<string, float>();
            }
            return _cost;
        }
        set
        {
            _cost = value;
        }
    }

    [XmlElement("incomes")]
    public List<KeyValuePair<string, float>> ProxyIncomes
    {
        get
        {
            return new List<KeyValuePair<string, float>>(this.Incomes);
        }
        set
        {
            this.Incomes = new Dictionary<string, float>();
            foreach (var pair in value)
                this.Incomes[pair.Key] = pair.Value;
        }
    }

    private Dictionary<string, float> _incomes;

    [XmlIgnore]
    public Dictionary<string, float> Incomes
    {
        get
        {
            if (_incomes == null)
            {
                _incomes = new Dictionary<string, float>();
            }
            return _incomes;
        }
        set
        {
            _incomes = value;
        }
    }

    [XmlElement("trade")]
    public float Trade
    {
        get;
        set;
    }

    [XmlElement("max_trade")]
    public float Max_Trade
    {
        get;
        set;
    }

    [XmlElement("buildTime")]
    public float BuildTime
    {
        get;
        set;
    }

    [XmlElement("storage")]
    public float Storage
    {
        get;
        set;
    }

    [XmlElement("description")]
    public string Description
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