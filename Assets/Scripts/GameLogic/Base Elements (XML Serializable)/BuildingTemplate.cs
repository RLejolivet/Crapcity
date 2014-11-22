using System.Xml.Serialization;
using System.Collections.Generic;


/**
 * Template used for designers to give us their building ideas easily
 * Imported from BuildingFactory, used to instantiate the Building class
 **/

[XmlRoot("BuildingTemplate")]
[XmlType("LevelDescription")]
public class BuildingTemplate {

    [XmlElement("Name")]
    public string name
    {
        get;
        set;
    }

    [XmlElement("Cost")]
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

    [XmlElement("Incomes")]
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

    [XmlElement("Trade")]
    public float Trade
    {
        get;
        set;
    }

    [XmlElement("BuildTime")]
    public float BuildTime
    {
        get;
        set;
    }

    [XmlElement("Storage")]
    public float Storage
    {
        get;
        set;
    }

    [XmlElement("Description")]
    public string Description
    {
        get;
        set;
    }

}