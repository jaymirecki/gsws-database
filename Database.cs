////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                                Database.cs                                 //
//                               Database class                               //
//             Created by: Jarett (Jay) Mirecki, October 09, 2019             //
//            Modified by: Jarett (Jay) Mirecki, February 07, 2020            //
//                                                                            //
//          The Database class implements all of the functions                //
//          needed to manipulate the data structures in the GSWS              //
//          simulations. Ideally, this allows for abstraction                 //
//          between the simulation data and the simulation                    //
//          visualization. This interface also provides shortcut              //
//          functions for accessing multiple data structures(i.e.             //
//          for getting the Planet object representing a character's          //
//          home planet).                                                     //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using JMSuite.Collections;

namespace GSWS {
    public partial class Database {
        #region Members
        private Dictionary<string, Character> Characters;
        private Dictionary<string, Faction> Factions;
        private Dictionary<string, Fleet> Fleets;
        private Dictionary<string, Government> Governments;
        private Graph <string, Planet> Planets;
        private Player Player;
        private Date Date;
        #endregion
        #region Constructing
        private void InitDatabase() {
            Characters = new Dictionary<string, Character>();
            Factions = new Dictionary<string, Faction>();
            Fleets = new Dictionary<string, Fleet>();
            Governments = new Dictionary<string, Government>();
            Planets = new Graph<string, Planet>();
            Player = new Player();
            Date = new Date();
        }

        public Database() {
            InitDatabase();
        }

        public Database(Player p, Date d) {
            InitDatabase();
            Player = p;
            Date = d;
        }
        #endregion
        public static string GetFreshID() {
            return Guid.NewGuid().ToString();
        }
        private string DictionaryToString<T>(Dictionary<string, T> dic) {
            string ret = "[ ";
            foreach (T t in dic.Values) {
                ret += t.ToString() + ", ";
            }
            return ret + "]";
        }
        public override string ToString() {
            return DictionaryToString<Character>(Characters) + 
                   DictionaryToString<Faction>(Factions) + 
                   DictionaryToString<Fleet>(Fleets) + 
                   DictionaryToString<Government>(Governments) + 
                   //DictionaryToString<Planet>(Planets) + 
                   Player.ToString() + 
                   Date.ToString();
        }
        ////////////////////////////////////////////////////////////////////////
        //                              Planets                               //
        ////////////////////////////////////////////////////////////////////////
        #region
        public Planet GetPlanet(string planetId) {
            Planet p;
            if (Planets.TryFind(planetId, out p))
                return p;
            return new Planet();
        }
        public Graph<string, Planet> GetPlanets() {
            return Planets;
        }
        public void AddPlanet(Planet p) {
            // int[] distances = new int[p.Neighbors.Length];
            // foreach ()
            Planets.Add(p.ID, p, p.Neighbors);
        }
        public List<Fleet> GetPlanetFleets(string planet) {
            Planet p = GetPlanet(planet);
            return GetPlanetFleets(p);
        }
        public List<Fleet> GetPlanetFleets(Planet planet) {
            List<Fleet> fleets = new List<Fleet>();
            foreach (string f in planet.Fleets) {
                fleets.Add(GetFleet(f));
            }
            return fleets;
        }
        #endregion
        ////////////////////////////////////////////////////////////////////////
        //                             Characters                             //
        ////////////////////////////////////////////////////////////////////////
        #region
        public void AddCharacter(Character c) {
            Characters.Add(c.ID, c);
        }
        public Character GetCharacter(string characterId) {
            Character c;
            if (Characters.TryGetValue(characterId, out c))
                return c;
            return new Character();
        }
        public Character GetPlayerCharacter() {
            return GetCharacter(Player.Character);
        }
        public Planet GetHomeworld(string characterId) {
            return GetHomeworld(GetCharacter(characterId));
        }
        public Planet GetHomeworld(Character c) {
            return GetPlanet(c.Homeworld);
        }
        #endregion
        ////////////////////////////////////////////////////////////////////////
        //                              Factions                               /
        ////////////////////////////////////////////////////////////////////////
        #region
        public Faction GetFaction(string factionId) {
            Faction f;
            if (Factions.TryGetValue(factionId, out f))
                return f;
            return new Faction();
        }
        public Faction GetFaction(Planet p) {
            return GetFaction(p.Faction);
        }
        public Faction GetPlayerFaction() {
            return GetFaction(Player.Faction);
        }
        #endregion
        ////////////////////////////////////////////////////////////////////////
        //                              Fleets                                //
        ////////////////////////////////////////////////////////////////////////
        #region
        public Fleet NewFleet() {
            Fleet fleet = new Fleet(GetNewFleetName());
            return fleet;
        }
        public Fleet NewFleet(string name) {
            Fleet fleet = new Fleet(name);
            return fleet;
        }
        public void AddFleet(Fleet fleet) {
            Fleets.Add(fleet.ID, fleet);
        }
        public Fleet GetFleet(string fleetID) {
            Fleet fleet;
            if (Fleets.TryGetValue(fleetID, out fleet))
                return fleet;
            return new Fleet();
        }
        private string GetNewFleetName() {
            int fleetCounter = 1;
            string fleetName = "UNASSIGNED";
            bool nameExists = true;
            while(nameExists) {
                fleetName = "Fleet #" + fleetCounter++.ToString();
                nameExists = false;
                foreach(Fleet f in Fleets.Values) {
                    if (fleetName == f.Name) {
                        nameExists = true;
                        break;
                    }
                }
            }
            return fleetName;
        }
        public List<Fleet> GetFleets() {
            return new List<Fleet>(Fleets.Values);
        }
        public bool GetFleetPlanet(string fleet, out Planet planet) {
            return GetFleetPlanet(GetFleet(fleet), out planet);
        }
        public bool GetFleetPlanet(Fleet fleet, out Planet planet) {
            if (fleet.Orbiting == null) {
                planet = new Planet();
                return false;
            }
            else {
                planet =  GetPlanet(fleet.Orbiting);
                return true;
            }
        }
        #endregion
        ////////////////////////////////////////////////////////////////////////
        //                             Governments                            //
        ////////////////////////////////////////////////////////////////////////
        #region
        public Government GetGovernment(string governmentId) {
            Government g;
            if (Governments.TryGetValue(governmentId, out g))
                return g;
            return new Government();
        }
        public Government GetGovernment(Faction f) {
            return GetGovernment(f.Government);
        }
        public Government GetPlayerGovernment() {
            return GetGovernment(GetFaction(Player.Faction).Government);
        }
        #endregion
        ////////////////////////////////////////////////////////////////////////
        //                                Date                                //
        ////////////////////////////////////////////////////////////////////////
        #region
        public void AdvanceTime() {
            Date.AdvanceTime();
        }
        public string GetDateString() {
            return Date.ToString();
        }
        public bool IsWeek() {
            return Date.IsWeek();
        }
        public bool IsMonth() {
            return Date.IsMonth();
        }
        public bool IsYear() {
            return Date.IsYear();
        }
        #endregion
        ////////////////////////////////////////////////////////////////////////
        //                               Player                               //
        ////////////////////////////////////////////////////////////////////////
        #region
        public void CreatePlayer(string character, string faction) {
            Player = new Player(character, faction);
            GetCharacter(character).IsPlayer = true;
        }
        public void CreatePlayer(string name, string species, string homeworld, string faction) {
            Character c = new Character();
            c.Name = name;
            c.ID = c.Name;
            c.Species = species;
            c.Homeworld = homeworld;
            c.IsPlayer = true;
            AddCharacter(c);
            Player = new Player(c.ID, faction);
        }
        public Player GetPlayer() {
            return Player;
        }
        #endregion
    }
}