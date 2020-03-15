////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                                 Planet.cs                                  //
//                                Planet class                                //
//              Created by: Jarett (Jay) Mirecki, July 27, 2019               //
//             Modified by: Jarett (Jay) Mirecki, March 15, 2020              //
//                                                                            //
//          The Planet class represents a planet in the galaxy. This          //
//          structure stores information about its location,                  //
//          population, and industry. Additionally, the class                 //
//          implements methods to update the dynamic parameters as            //
//          the simulation runs.                                              //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace GSWS {

public partial class Database {
    private void UpdatePlanetValues() {
        foreach (Planet p in Planets.Values) {
            p.UpdateValues(this);
        }
    }
    private void UpdatePlanetKeys() {
        foreach (Planet p in Planets.Values) {
            p.UpdateKeys();
        }
    }
}

[Serializable] public class Planet {
    #region properties
    [XmlAttribute] public string ID;
    public string Name;
    public string kGovernment;
    public float Population, Wealth;
    [XmlIgnore] public HashSet<Fleet> Fleets { get; private set; }
    private Body _body;
    [XmlIgnore] public Body Body {
        set { if (value.ID == ID)
                  _body = value; }
        private get { return _body; }
    }
    [XmlIgnore] public Government Government;

    [XmlIgnore] public Coordinate Coordinates {
        get { return Body.Coordinates; }
    }
    [XmlIgnore] public string System {
        get { return Body.kSystem; }
    }
    [XmlIgnore] public string Sector {
        get { return Body.kSystem; }
    }
    [XmlIgnore] public Region Region {
        get { return Body.Region; }
    }
    [XmlIgnore] public Government Faction {
        get { return Government.Faction; }
        private set {}
    }
    #endregion
    #region Constructing
    public Planet() {
        ID = "";
        Name = "";
        kGovernment = "";
        Population = Wealth = 0f;
        Fleets = new HashSet<Fleet>();
        _body = new Body();
    }
    #endregion
    #region Boiler Plate
    public string DatapadDescription() {
        string description = 
            Name + "\n" + Coordinates.ToString() + ", " + System + ", " + Sector + ", " + Region;
        return description;
    }
    public override string ToString() {
        return "{" + ID + ", " + Name + "}";
    }
    public void UpdateValues(Database db) {
        Body body;
        if (db.Bodies.TryGetValue(ID, out body))
            Body = body;
        foreach (Fleet f in Fleets) {
            if (f.Orbiting != this)
                Fleets.Remove(f);
        }
        Government government;
        if (db.Governments.TryGetValue(kGovernment, out government)) {
            Government = government;
            Government.MemberPlanets.Add(this);
        }
    }
    public void UpdateKeys() {
        kGovernment = Government.ID;
    }
    #endregion
    public float Value() {
        return ResidentialValue();// + IndustrialValue();
    }
    public float ResidentialValue() {
        return Wealth * Population;
    }
    // public float IndustrialValue() {
    //     return Productivity * Industrialization;
    // }
    // public void GenerateUnknownInfo() {
    //     if (Description == "")
    //         Description = "A planet in the " + System + "system";
    // }
    public string ValueString() {
        float value = Value();
        string valueString;
        if (value > 1000000000000000000000000000f)
            valueString =
                value.ToString("###,###,###,###,###,,,,,,,,,.00") + " octillion";
        else if (value > 1000000000000000000000000f)
            valueString = 
                value.ToString("###,###,###,###,###,,,,,,,,.00") + " septillion";
        else if (value > 1000000000000000000000f)
            valueString = 
                value.ToString("###,###,###,###,###,,,,,,,.00") + " sextillion";
        else if (value > 1000000000000000000f)
            valueString = 
                value.ToString("###,###,###,###,###,,,,,,.00") + " quintillion";
        else if (value > 1000000000000000f)
            valueString = 
                value.ToString("###,###,###,###,###,,,,,.00") + " quadrillion";
        else if (value > 1000000000000f)
            valueString = 
                value.ToString("###,###,###,###,###,,,,.00") + " trillion";
        else if (value > 1000000000f)
            valueString = 
                value.ToString("###,###,###,###,###,,,.00") + " billion";
        else if (value > 1000000f)
            valueString = 
                value.ToString("###,###,###,###,###,,.00") + " million";
        else
            valueString = 
                value.ToString("###,###.00");
        return valueString + " credits per year";
    }
}
}