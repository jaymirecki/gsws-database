////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                                  Fleet.cs                                  //
//                                 Fleet class                                //
//             Created by: Jarett (Jay) Mirecki, January 31, 2020             //
//             Modified by: Jarett (Jay) Mirecki, March 15, 2020              //
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

public partial class Database {
    private void UpdateFleetValues() {
        foreach (Fleet f in Fleets.Values) {
            f.UpdateValues(this);
        }
    }
    private void UpdateFleetKeys() {
        foreach (Fleet f in Fleets.Values) {
            f.UpdateKeys();
        }
    }
}

[Serializable] public class Fleet {
    [XmlAttribute] public string ID;
    public string Name;
    public string kDestination, kNextStop, kOrbiting;
    [XmlIgnore] public Planet Destination;
    [XmlIgnore] public Planet NextStop;
    [XmlIgnore] public Planet Orbiting;
    public Coordinate Position;
    public List<string> Ships;

    #region Constructing
    public Fleet() {
        ID = "";
        Name = "";
        kDestination = kNextStop = kOrbiting = "";
        Destination = NextStop = Orbiting = null;
        Position = new Coordinate(0, 0, 0);
        Ships = new List<string>();
    }
    public Fleet(string name):this() {
        Name = name;
    }
    #endregion
    public override string ToString() {
        return "{" + ID + ", " + Name + "}";
    }
    public void UpdateValues(Database db) {
        Planet destination, nextStop, orbiting;
        if (kDestination != "" && 
                db.Planets.TryGetValue(kDestination, out destination)) {
            Destination = destination;
            Destination.Fleets.Add(this);
        }
        else if (Destination != null) {
            Destination.Fleets.Remove(this);
            Destination = null;
        }
        if (kNextStop != "" && 
                db.Planets.TryGetValue(kNextStop, out nextStop)) {
            NextStop = nextStop;
            NextStop.Fleets.Add(this);
        }
        else if (NextStop != null) {
            NextStop.Fleets.Remove(this);
            NextStop = null;
        }
        if (kOrbiting != "" && 
                db.Planets.TryGetValue(kOrbiting, out orbiting)) {
            Orbiting = orbiting;
            Orbiting.Fleets.Add(this);
        }
        else if (Orbiting != null) {
            Orbiting.Fleets.Remove(this);
            Orbiting = null;
        }
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
    }
    public string DatapadDescription() {
        string description = 
            Name;
        return description;
    }
    public bool Stationary() {
        return Destination == null;
    }
    public void Move() {
        if (Stationary()) return;
        
    }
}
}