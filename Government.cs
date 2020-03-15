////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                               Government.cs                                //
//                              Government class                              //
//             Created by: Jarett (Jay) Mirecki, August 08, 2019              //
//             Modified by: Jarett (Jay) Mirecki, March 15, 2020              //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using JMSuite.Collections;

namespace GSWS {

public partial class Database {
    private void UpdateGovernmentValues() {
        foreach (Government g in Governments.Values)
            g.UpdateValues(this);
    }
    private void UpdateGovernmentKeys() {
        foreach (Government g in Governments.Values)
            g.UpdateKeys();
    }
}
public enum Relationship { Ally, Neutral, Enemy };
[Serializable] public class Government {
    [XmlAttribute] public string ID;
    public string Name, Color;
    public string kSuperGovernment, kMilitary;
    [XmlIgnore] public Government SuperGovernment;
    [XmlIgnore] public Government Faction {
        get { if (SuperGovernment == null)
                  return this;
              else
                  return SuperGovernment.Faction; }
        private set { }
    }
    public float ExecutivePower, LegislativePower, JudicialPower, ResidentialTax, CommercialTax;
    public JDictionary<string, Relationship> Relationships;
    [XmlIgnore] public HashSet<Planet> MemberPlanets;
    public Budget Budget;
    public Government() {
        ID = Name = "null";
        kSuperGovernment = "";
        ExecutivePower = LegislativePower = JudicialPower = ResidentialTax = CommercialTax = 0f;
        MemberPlanets = new HashSet<Planet>();
        Relationships = new JDictionary<string, Relationship>();
        Budget = new Budget();
    }
    public Government(string Name):this() {
        this.Name = Name;
        ID = Name.ToLower().Replace("'", "").Replace(' ', '_');
    }
    public void UpdateValues(Database db) {
        foreach (Planet p in MemberPlanets) {
            if (p.Government != this)
                MemberPlanets.Remove(p);
        }
        foreach (string k in Relationships.Keys) {
            if (!db.Governments.ContainsKey(k))
                Relationships.Remove(k);
        }
    }
    public void UpdateKeys() {
    }
    public override string ToString() {
        return "{" + ID + ", " + Name + "}";
    }
    public string DatapadDescription() {
        string description = 
            Name;
        return description;
    }
}
}