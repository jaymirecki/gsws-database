////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                               Government.cs                                //
//                              Government class                              //
//                  Created by: Jay Mirecki, August 08, 2019                  //
//                  Modified by: Jay Mirecki, March 17, 2020                  //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using JMSuite.Collections;

namespace GSWS {
public enum Relationship { Ally, Neutral, Enemy };
[Serializable] public class Government : IObject {
    #region Properties
    [XmlAttribute] public string ID;
    public string Name, Color, Description;
    public string kSuperGovernment, kMilitary;
    [XmlIgnore] public Government SuperGovernment;
    [XmlIgnore] public HashSet<Government> SubGovernments;
    [XmlIgnore] public Military Military;
    [XmlIgnore] public Government Faction {
        get { if (SuperGovernment == null)
                  return this;
              else
                  return SuperGovernment.Faction; }
    }
    public float ExecutivePower, LegislativePower, JudicialPower, ResidentialTax, CommercialTax;
    public JDictionary<string, Relationship> Relationships;
    [XmlIgnore] public HashSet<Planet> MemberPlanets;
    public Budget Budget;
    #endregion
    #region Constructing
    public Government() {
        ID = "";
        Name = Description = "";
        kSuperGovernment = kMilitary = "";
        ExecutivePower = LegislativePower = JudicialPower = ResidentialTax = CommercialTax = 0f;
        MemberPlanets = new HashSet<Planet>();
        Relationships = new JDictionary<string, Relationship>();
        Budget = new Budget();
        Military = null;
        SuperGovernment = null;
        SubGovernments = new HashSet<Government>();
    }
    public Government(string Name):this() {
        this.Name = Name;
    }
    #endregion
    #region Boiler Plate
    public void UpdateValues(Database db) {
        Government government;
        Military military;
        if (db.Governments.TryGetValue(kSuperGovernment, out government))
            SuperGovernment = government;
        if (db.Militaries.TryGetValue(kMilitary, out military))
            Military = military;
        foreach (string k in Relationships.Keys) {
            if (!db.Governments.ContainsKey(k))
                Relationships.Remove(k);
        }
    }
    public void UpdateKeys() {
        kSuperGovernment = kMilitary = "";
        if (SuperGovernment != null)
            kSuperGovernment = SuperGovernment.ID;
        if (Military != null)
            kMilitary = Military.ID;
        
    }
    public void VerifySubGroups() {
        foreach (Planet p in MemberPlanets) {
            if (p.Government != this)
                MemberPlanets.Remove(p);
        }
        foreach (Government g in SubGovernments) {
            if (g.SuperGovernment != this)
                SubGovernments.Remove(g);
        }
    }
    public void UpdateSuperGroups() {
        if (SuperGovernment != null)
            SuperGovernment.SubGovernments.Add(this);
    }
    public override string ToString() {
        return "{" + ID + ", " + Name + "}";
    }
    public string DatapadDescription() {
        string description = 
            Name + "\n\n" +
            Description;
        return description;
    }
    #endregion
}
}