////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                                  Save .cs                                  //
//               Save and Load functions for the Database Class               //
//             Created by: Jarett (Jay) Mirecki, October 19, 2019             //
//             Modified by: Jarett (Jay) Mirecki, October 19, 2019            //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using JMSuite.Collections;

namespace GSWS {
    public partial class Database {
        public void Save(string directory) {
            Directory.CreateDirectory(directory);

            new Serializer<List<Planet>>().Serialize(directory + "planets.xml", Planets.Values());

            new Serializer<Faction>().SerializeDictionary(directory + "factions.xml", Factions);

            new Serializer<Government>().SerializeDictionary(directory + "governments.xml", Governments);
            
            new Serializer<Character>().SerializeDictionary(directory + "characters.xml", Characters);

            new Serializer<Date>().Serialize(directory + "date.xml", Date);

            new Serializer<Player>().Serialize(directory + "player.xml", Player);
        }
        
        // Loading database from file
        public void LoadDatabase(string directory) {
            Player player = new Serializer<Player>().Deserialize(directory + "player.xml");
            LoadDatabase(directory, player);
        }
        public void LoadDatabase(string directory, Player player) {
            LoadPlanets(directory);
            LoadFactions(directory);
            LoadGovernments(directory);
            LoadFleets(directory);
            this.Player = player;
        }
        private void LoadPlanets(string directory) {
            List<Planet> planetList = new List<Planet>();
            Serializer<List<Planet>> PlanetListSerializer = 
                new Serializer<List<Planet>>();
            planetList = 
                PlanetListSerializer.Deserialize(directory + "planets.xml");
            foreach(Planet aPlanet in planetList) 
                Planets.Add(aPlanet.ID, aPlanet, aPlanet.Neighbors);
        }
        private void LoadFactions(string directory) {
            Faction empire = new Faction("Galactic Empire");
            Faction rebels = new Faction("New Republic");

            empire.ID = "empire";
            empire.AddRelationship(rebels.ID, 0);
            empire.Color = "Red";
            empire.Government = "empire";

            rebels.ID = "newrepublic";
            rebels.AddRelationship(empire.ID, 0);
            rebels.Color = "Blue";
            rebels.Government = "rebels";

            List<Faction> factionList = new List<Faction>();
            factionList.Add(empire);
            factionList.Add(rebels);

            foreach(Faction aFaction in factionList)
                Factions.Add(aFaction.ID, aFaction);
            foreach(Planet aPlanet in Planets.Values()) {
                Faction pFaction = GetFaction(aPlanet);
                if (!GetGovernment(pFaction).MemberPlanets.Contains(aPlanet.ID))
                    GetGovernment(pFaction).MemberPlanets.Add(aPlanet.ID);
            }
        }
        private void LoadGovernments(string directory) {
            Government empire = new Government("Galactic Empire");
            empire.ID = "empire";
            empire.ExecutivePower = 1f;
            
            Government rebels = new Government("New Republic");
            rebels.ID = "rebels";
            rebels.ExecutivePower = 0.375f;
            rebels.LegislativePower = 0.375f;
            rebels.JudicialPower = 0.25f;

            List<Government> governmentList = new List<Government>();
            governmentList.Add(empire);
            governmentList.Add(rebels);

            foreach(Government aGovernment in governmentList)
                Governments.Add(aGovernment.ID, aGovernment);
        }
        private void LoadFleets(string directory) {
            List<Fleet> fleetList = new List<Fleet>();
            Serializer<List<Fleet>> FleetListSerializer = 
                new Serializer<List<Fleet>>();
            fleetList = 
                FleetListSerializer.Deserialize(directory + "fleets.xml");
            foreach(Fleet aFleet in fleetList) 
                Fleets.Add(aFleet.ID, aFleet);
        }
        private void LoadCharacters(string directory) {

        }
    }
}