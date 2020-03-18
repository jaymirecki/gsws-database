////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                                 Planet.cs                                  //
//                                Planet class                                //
//                   Created by: Jay Mirecki, July 27, 2019                   //
//                  Modified by: Jay Mirecki, March 17, 2020                  //
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
[Serializable] public class Planet : IObject {
    #region Properties
    [XmlAttribute] public string ID;
    public string Name, Demonym, Description;
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

    [XmlIgnore] public Coordinate Position {
        get { return Body.Position; }
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
    [XmlIgnore] public Class Class {
        get { return Body.Class; }
    }
    [XmlIgnore] public Atmosphere Atmosphere {
        get { return Body.Atmosphere; }
    }
    [XmlIgnore] public Government Faction {
        get { return Government.Faction; }
    }
    #endregion
    #region Constructing
    public Planet() {
        ID = "";
        Name = Demonym = Description = "";
        kGovernment = "";
        Population = Wealth = 0f;
        Fleets = new HashSet<Fleet>();
        _body = new Body();
    }
    #endregion
    #region Boiler Plate
    public string DatapadDescription() {
        string description = 
            Name + "\n\n" + 
            Position.ToString() + ", " + System + ", " + Sector + ", " + Region + "\n\n" +
            Description;
        return description;
    }
    public override string ToString() {
        return "{" + ID + ", " + Name + "}";
    }
    public void UpdateValues(Database db) {
        Body body;
        if (db.Bodies.TryGetValue(ID, out body))
            Body = body;
        Government government;
        if (db.Governments.TryGetValue(kGovernment, out government))
            Government = government;
    }
    public void UpdateKeys() {
        kGovernment = Government.ID;
    }
    public void VerifySubGroups() {
        foreach (Fleet f in Fleets) {
            if (f.Orbiting != this)
                Fleets.Remove(f);
        }
    }
    public void UpdateSuperGroups() {
        Government.MemberPlanets.Add(this);
    }
    #endregion
    #region Value Calculations
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
    #endregion
}}