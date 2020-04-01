////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                                  Fleet.cs                                  //
//                                 Fleet class                                //
//                 Created by: Jay Mirecki, January 31, 2020                  //
//                  Modified by: Jay Mirecki, March 26, 2020                  //
//                                                                            //
//          The Fleet class is a representation of groups of space            //
//          vessels, allowing them to move as groups.                         //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using JMSuite.Collections;

namespace GSWS {
[Serializable] public class Fleet : IObject {
    #region Properties
    [XmlAttribute] public string ID;
    private const float hourlyDistance = 20f;
    public string Name;
    public string kDestination, kNextStop, kOrbiting, kMilitary;
    public float Speed;
    private Planet _orbiting;
    [XmlIgnore] public Planet Destination { get; private set; }
    [XmlIgnore] public Planet NextStop { get; private set; }
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
        Speed = 1f;
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
    public bool SetDestination(Planet destination, JGraph<string> map, JDictionary<string, Planet> planets) {
        List<string> path;
        if (Orbiting != null && map.TryPathTo(Orbiting.ID, destination.ID, out path, p => {return Military.Faction.Relationships[planets[p].Faction.ID] != Relationship.Enemy;}, true)) {
            Destination = destination;
            NextStop = planets[path[1]];
            return true;
        }
        return false;
    }
    public bool Move(JGraph<string> map, JDictionary<string, Planet> planets) {
        if (isStationary) return false;
        Position = Position.MoveTowards(NextStop.Position, 20);
        if (Position == NextStop.Position) {
            Orbiting = NextStop;
            if (NextStop == Destination) {
                NextStop = Destination = null;
                return true;
            }
            SetDestination(Destination, map, planets);
            return false;
        }
        else {
            Orbiting = null;
            return false;
        }
    }
    public JDictionary<Planet, int> SafeDestinations(JGraph<string> map, JDictionary<string, Planet> planets) {
        return Destinations(map, planets, false, false);
    }
    public JDictionary<Planet, int> HostileDestinations(JGraph<string> map, JDictionary<string, Planet> planets) {
        return Destinations(map, planets, true, true);
    }
    private JDictionary<Planet, int> Destinations(JGraph<string> map, JDictionary<string, Planet> planets, bool includeHostile, bool onlyHostile) {
        JDictionary<Planet, int> planetDestinations = 
            new JDictionary<Planet, int>();
        Planet start;
        if (Orbiting != null)
            start = Orbiting;
        else if (NextStop != null)
            start = NextStop;
        else
            return planetDestinations;
        JDictionary<string, int> destinations;
        if (map.TryReachableVertices(start.ID, out destinations, p => Military.Faction.Relationships[planets[p].Faction.ID] != Relationship.Enemy, includeHostile)) {
            destinations.Remove(start.ID);
            foreach (string s in destinations.Keys) {
                Relationship relationship = 
                    Military.Faction.Relationships[planets[s].Faction.ID];
                if (!onlyHostile || (onlyHostile && relationship == Relationship.Enemy))
                    planetDestinations.Add(planets[s], destinations[s] / (int)(hourlyDistance / Speed));
            }
        }
        return planetDestinations;
    }
}
}