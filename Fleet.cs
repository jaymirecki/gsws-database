////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                                  Fleet.cs                                  //
//                                 Fleet class                                //
//                 Created by: Jay Mirecki, January 31, 2020                  //
//                  Modified by: Jay Mirecki, March 18, 2020                  //
//                                                                            //
//          The Fleet class is a representation of groups of space            //
//          vessels, allowing them to move as groups.                         //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace GSWS {
[Serializable] public class Fleet : IObject {
    #region Properties
    [XmlAttribute] public string ID;
    public string Name;
    public string kDestination, kNextStop, kOrbiting, kMilitary;
    private Planet _orbiting;
    [XmlIgnore] public Planet Destination;
    [XmlIgnore] public Planet NextStop;
    [XmlIgnore] public Planet Orbiting {
        get { return _orbiting; }
        set { if (value == null)
                  _orbiting = null;
              else {
                  _orbiting = value;
                  Position = Orbiting.Position;
              }
            }
    }
    [XmlIgnore] public Military Military;
    private Coordinate _position;
    public Coordinate Position {
        get { return _position; }
        set { if (Orbiting != null && value != Orbiting.Position)
                  Orbiting = null;
              _position = value; }
    }
    [XmlIgnore] public bool isStationary {
        get { return Destination == null; }
    }
    [XmlIgnore] public List<string> Ships;
    #endregion
    #region Constructing
    public Fleet() {
        ID = "";
        Name = "";
        kDestination = kNextStop = kOrbiting = "";
        Destination = NextStop = Orbiting = null;
        Military = null;
        Position = new Coordinate(0, 0, 0);
        Ships = new List<string>();
    }
    public Fleet(string name):this() {
        Name = name;
    }
    #endregion
    #region Boiler Plate
    public override string ToString() {
        return "{" + ID + ", " + Name + "}";
    }
    public void UpdateValues(Database db) {
        Planet destination, nextStop, orbiting;
        if (kDestination != "" && 
                db.Planets.TryGetValue(kDestination, out destination)) {
            Destination = destination;
        }
        else if (Destination != null) {
            Destination = null;
        }
        if (kNextStop != "" && 
                db.Planets.TryGetValue(kNextStop, out nextStop)) {
            NextStop = nextStop;
        }
        else if (NextStop != null) {
            NextStop = null;
        }
        if (kOrbiting != "" && 
                db.Planets.TryGetValue(kOrbiting, out orbiting)) {
            Orbiting = orbiting;
            Position = Orbiting.Position;
        }
        else if (Orbiting != null) {
            Orbiting = null;
        }
        Military = db.Militaries[kMilitary];
    }
    public void UpdateKeys() {
        if (Destination != null)
            kDestination = Destination.ID;
        else
            kDestination = "";
        if (NextStop != null)
            kNextStop = NextStop.ID;
        else
            kNextStop = "";
        if (Orbiting != null)
            kOrbiting = Orbiting.ID;
        else
            kOrbiting = "";
        kMilitary = Military.ID;
    }
    public void VerifySubGroups() {

    }
    public void UpdateSuperGroups() {
        if (Orbiting != null)
            Orbiting.Fleets.Add(this);
        Military.Fleets.Add(this);
    }
    public string DatapadDescription() {
        string description = 
            Name;
        return description;
    }
    #endregion
    public void Move() {
        if (isStationary) return;
        
    }
}
}