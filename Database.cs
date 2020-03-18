////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                                Database.cs                                 //
//                               Database class                               //
//                 Created by: Jay Mirecki, October 09, 2019                  //
//                  Modified by: Jay Mirecki, March 17, 2020                  //
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
//          Testing Coverage: /241
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using JMSuite.Collections;

namespace GSWS {
    public partial class Database {
        #region Members
        public JDictionary<string, Body> Bodies;
        public JDictionary<string, Character> Characters;
        public JDictionary<string, Fleet> Fleets;
        public JDictionary<string, Government> Governments;
        public JDictionary<string, Military> Militaries;
        public JDictionary<string, Planet> Planets;
        public JGraph<string> Map;
        public Player Player;
        public Date Date;
        #endregion
        #region Constructing
        // Testing Coverage: 9/11
        public Database() {
            Bodies = new JDictionary<string, Body>();
            Characters = new JDictionary<string, Character>();
            Fleets = new JDictionary<string, Fleet>();
            Militaries = new JDictionary<string, Military>();
            Governments = new JDictionary<string, Government>();
            Planets = new JDictionary<string, Planet>();
            Map = new JGraph<string>();
            Player = new Player();
            Date = new Date();
        }

        public Database(Player p, Date d): this() {
            Player = p;
            Date = d;
        }
        #endregion
        #region Boiler Plate
        // Testing Coverage: 11/11
        private string DictionaryToString<T>(Dictionary<string, T> dic) {
            string ret = "[ ";
            foreach (T t in dic.Values) {
                ret += t.ToString() + ", ";
            }
            return ret + "]";
        }
        public override string ToString() {
            return DictionaryToString<Character>(Characters) + 
                   DictionaryToString<Fleet>(Fleets) + 
                   DictionaryToString<Government>(Governments) + 
                   DictionaryToString<Planet>(Planets) + 
                   Player.ToString() + 
                   Date.ToString();
        }
        #endregion
        #region IDs and Names
        // Testing Coverage: 19/19
        public string FreshID(HashSet<string> ids, string name, string prefix) {
            bool used = true;
            int counter = 1;
            string id = "";
            while (used) {
                id = name.ToLower().Replace(" ", "").Replace("#", "").Replace("'", "") + prefix + counter++.ToString();
                if (!ids.Contains(id))
                    used = false;
            }
            return id;
        }
        public string FreshName<T>(JDictionary<string, T>.ValueCollection values, string prefix) {
            bool used = true;
            int counter = 1;
            string name = "";
            while (used) {
                used = false;
                name = prefix + " #" + counter++.ToString();
                foreach (T t in values) {
                    if ((string)typeof(T).GetField("Name").GetValue(t) == 
                                                                        name) {
                        used = true;
                        break;
                    }
                }
            }
            return name;
        }
        #endregion
        #region Planets
        // Testing Coverage: 5/5
        public string FreshPlanetID(Planet planet) {
            return FreshID(new HashSet<string>(Planets.Keys), planet.Name, "planet");
        }
        public string FreshPlanetName() {
            return FreshName<Planet>(Planets.Values, "Unnamed Planet");
        }
        public void AddPlanet(Planet p) {
            string ID = FreshPlanetID(p);
            p.ID = ID;
            Planets.Add(p.ID, p);
        }
        #endregion
        #region Characters
        // Testing Coverage: 5/5
        public string FreshCharacterID(Character c) {
            return FreshID(new HashSet<string>(Characters.Keys), c.Name, "character");
        }
        public string FreshCharacterName() {
            return FreshName<Character>(Characters.Values, "Unnamed Character");
        }
        public void AddCharacter(Character c) {
            string ID = FreshCharacterID(c);
            c.ID = ID;
            Characters.Add(c.ID, c);
        }
        #endregion
        #region Fleets
        // Testing Coverage: /14
        public string FreshFleetID(Fleet fleet) {
            if (fleet.Orbiting == null)
            return FreshID(new HashSet<string>(Fleets.Keys), "", "fleet");
            else
                return FreshID(new HashSet<string>(Fleets.Keys), fleet.Orbiting.Name, "fleet");
        }
        public string FreshFleetName(Fleet fleet) {
            if (fleet.Orbiting == null)
                return FreshName<Fleet>(Fleets.Values, "Fleet");
            else
                return FreshName<Fleet>(Fleets.Values, 
                                        fleet.Orbiting.Name + " Fleet");
        }
        public Fleet NewFleet() {
            Fleet fleet = new Fleet();
            fleet.Name = FreshFleetName(fleet);
            return fleet;
        }
        public Fleet NewFleet(string name) {
            Fleet fleet = new Fleet(name);
            return fleet;
        }
        public void AddFleet(Fleet fleet) {
            string ID = FreshFleetID(fleet);
            fleet.ID = ID;
            Fleets.Add(fleet.ID, fleet);
        }
        #endregion
        #region Governments
        // Testing Coverage: 5/5
        public string FreshGovernmentID(Government government) {
            return FreshID(new HashSet<string>(Governments.Keys), government.Name, "government");
        }
        public string FreshGovernmentName() {
            return FreshName<Government>(Governments.Values, "Unnamed Government");
        }
        public void AddGovernment(Government g) {
            string ID = FreshGovernmentID(g);
            g.ID = ID;
            Governments.Add(g.ID, g);
        }
        #endregion
        #region Date
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
        #region Player
        public void CreatePlayer(string character, string faction) {
            Player = new Player();
            // Player.Character = Characters[character];
            Player.Faction = Governments[faction];
            Player.UpdateKeys();
            Player.UpdateValues(this);
        }
        // public void CreatePlayer(string name, string species, string homeworld, string faction) {
        //     Character c = new Character();
        //     c.Name = name;
        //     c.ID = c.Name;
        //     c.Species = species;
        //     c.Homeworld = homeworld;
        //     c.IsPlayer = true;
        //     AddCharacter(c);
        //     Player = new Player(c.ID, faction);
        // }
        public Player GetPlayer() {
            return Player;
        }
        #endregion
    }
}