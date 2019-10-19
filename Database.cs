////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                                 Database.cs                                //
//                               Database class                               //
//             Created by: Jarett (Jay) Mirecki, October 09, 2019             //
//             Modified by: Jarett (Jay) Mirecki, October 19, 2019            //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using JMSuite.Collections;

namespace GSWS {
    public partial class Database {
        private Graph <string, Planet> planets;
        private Dictionary<string, Character> characters;
        private Dictionary<string, Government> governments;
        private Dictionary<string, Faction> factions;
        private Player player;
        private Date date;

        private void InitDatabase() {
            planets = new Graph<string, Planet>();
            characters = new Dictionary<string, Character>();
            governments = new Dictionary<string, Government>();
            factions = new Dictionary<string, Faction>();
            player = new Player();
            date = new Date();
        }

        public Database() {
            InitDatabase();
        }

        public Database(Player p, Date d) {
            InitDatabase();
            player = p;
            date = d;
        }

        ////////////////////////////////////////////////////////////////////////
        //                              Planets                               //
        ////////////////////////////////////////////////////////////////////////
        public Planet GetPlanet(string planetId) {
            Planet p;
            if (planets.TryFind(planetId, out p))
                return p;
            return new Planet();
        }
        public Graph<string, Planet> GetPlanets() {
            return planets;
        }
        public void AddPlanet(Planet p) {
            // int[] distances = new int[p.Neighbors.Length];
            // foreach ()
            planets.Add(p.ID, p, p.Neighbors);
        }
        public Planet GetHomeworld(string characterId) {
            return GetHomeworld(GetCharacter(characterId));
        }
        public Planet GetHomeworld(Character c) {
            return GetPlanet(c.Homeworld);
        }

        ////////////////////////////////////////////////////////////////////////
        //                             Characters                             //
        ////////////////////////////////////////////////////////////////////////
        public void AddCharacter(Character c) {
            characters.Add(c.ID, c);
        }
        public Character GetCharacter(string characterId) {
            Character c;
            if (characters.TryGetValue(characterId, out c))
                return c;
            return new Character();
        }
        public Character GetPlayerCharacter() {
            return GetCharacter(player.Character);
        }

        ////////////////////////////////////////////////////////////////////////
        //                              Factions                               /
        ////////////////////////////////////////////////////////////////////////
        public Faction GetFaction(string factionId) {
            Faction f;
            if (factions.TryGetValue(factionId, out f))
                return f;
            return new Faction();
        }
        public Faction GetFaction(Planet p) {
            return GetFaction(p.Faction);
        }
        public Faction GetPlayerFaction() {
            return GetFaction(player.Faction);
        }

        ////////////////////////////////////////////////////////////////////////
        //                             Governments                            //
        ////////////////////////////////////////////////////////////////////////
        public Government GetGovernment(string governmentId) {
            Government g;
            if (governments.TryGetValue(governmentId, out g))
                return g;
            return new Government();
        }
        public Government GetGovernment(Faction f) {
            return GetGovernment(f.Government);
        }
        public Government GetPlayerGovernment() {
            return GetGovernment(GetFaction(player.Faction).Government);
        }

        ////////////////////////////////////////////////////////////////////////
        //                                Date                                //
        ////////////////////////////////////////////////////////////////////////
        public void AdvanceTime() {
            date.AdvanceTime();
        }
        public string GetDateString() {
            return date.ToString();
        }
        public bool IsWeek() {
            return date.IsWeek();
        }
        public bool IsMonth() {
            return date.IsMonth();
        }
        public bool IsYear() {
            return date.IsYear();
        }

        ////////////////////////////////////////////////////////////////////////
        //                               Player                               //
        ////////////////////////////////////////////////////////////////////////
        public void CreatePlayer(string character, string faction) {
            player = new Player(character, faction);
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
            player = new Player(c.ID, faction);
        }
    }
}