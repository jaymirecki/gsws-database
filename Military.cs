////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                                Military.cs                                 //
//                               Military Class                               //
//                  Created by: Jay Mirecki, March 17, 2020                   //
//                  Modified by: Jay Mirecki, March 17, 2020                  //
//                                                                            //
//          The Military class represents an entire military                  //
//          service. This could be a combined forces or a single              //
//          branch.                                                           //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

using JMSuite.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace GSWS {
[Serializable] public class Military : IObject {
    #region Properties
    [XmlAttribute] public string ID;
    public string Name, Description;
    [XmlIgnore] public HashSet<Military> Branches;
    [XmlIgnore] public HashSet<Fleet> Fleets;
    public string kSuperMilitary, kGovernment;
    private Military _superMilitary;
    private Government _government;
    [XmlIgnore] public Military SuperMilitary {
        get { return _superMilitary; }
        set { if (value != null) {
                  Government = null;
                  _superMilitary = value;
              }
              else if (_government != null)
                  _superMilitary = value; }
    }
    [XmlIgnore] public Government Government {
        get { if (_government == null)
                  return SuperMilitary.Government;
              else
                  return _government; }
        set { if (value != null) {
                  SuperMilitary = null;
                  _government = value;
              }
              else if (SuperMilitary != null)
                  _government = value; }
    }
    [XmlIgnore] public Government Faction {
        get { if (Government != null)
                  return Government.Faction;
              else
                  return SuperMilitary.Faction; }
    }
    [XmlIgnore] public string Color {
        get { if (Government != null)
                  return Government.Color;
              else
                  return SuperMilitary.Color; }
    }
    #endregion
    #region Constructing
    public Military() {
        ID = "";
        Name = "";
        kSuperMilitary = kGovernment = "";
        Branches = new HashSet<Military>();
        Fleets = new HashSet<Fleet>();
        _superMilitary = null;
        _government = null;
    }
    #endregion
    #region Boiler Plate
    public override string ToString() {
        return Name + "(Military)";
    }
    public string DatapadDescription() {
        return 
            Name + "\n\n" +
            Description;
    }
    public void UpdateValues(Database db) {
        SuperMilitary = null;
        Government = null;
        if (kSuperMilitary != "")
            SuperMilitary = db.Militaries[kSuperMilitary];
        else
            Government = db.Governments[kGovernment];
    }
    public void UpdateKeys() {
        kSuperMilitary = "";
        kGovernment = "";
        if (SuperMilitary != null)
            kSuperMilitary = SuperMilitary.ID;
        else if (Government != null)
            kGovernment = Government.ID;
    }
    public void VerifySubGroups() {
        foreach (Military m in Branches) {
            if (m.SuperMilitary != this)
                Branches.Remove(m);
        }
        foreach (Fleet f in Fleets) {
            if (f.Military != this)
                Fleets.Remove(f);
        }
    }
    public void UpdateSuperGroups() {
        if (SuperMilitary != null)
            SuperMilitary.Branches.Add(this);
        else
            Government.Military = this;
    }
    #endregion
}}