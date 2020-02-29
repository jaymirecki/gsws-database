////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                                  Save .cs                                  //
//               Save and Load functions for the Database Class               //
//             Created by: Jarett (Jay) Mirecki, October 19, 2019             //
//            Modified by: Jarett (Jay) Mirecki, February 27, 2020            //
//                                                                            //
//          This extension for the Database class allows for the              //
//          loading and saving of the entire database (and its                //
//          constituent parts) from or to a directory.                        //
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

            Planets.SerializeXml(directory + "planets.xml");

            Factions.SerializeXml(directory + "factions.xml");

            Governments.SerializeXml(directory + "governments.xml");

            Characters.SerializeXml(directory + "characters.xml");

            Fleets.SerializeXml(directory + "fleets.xml");

            new Serializer<Date>().Serialize(directory + "date.xml", Date);

            new Serializer<Player>().Serialize(directory + "player.xml", Player);
        }
        
        // Loading database from file
        public void LoadDatabase(string directory) {
            Player player = new Serializer<Player>().Deserialize(directory + "player.xml");
            LoadDatabase(directory, player);
        }
        public void LoadDatabase(string directory, Player player) {
            Planets = JDictionary<string, Planet>.DeserializeXml(directory + "planets.xml");
            Factions = JDictionary<string, Faction>.DeserializeXml(directory + "factions.xml");
            Governments = JDictionary<string, Government>.DeserializeXml(directory + "governments.xml");
            Fleets = JDictionary<string, Fleet>.DeserializeXml(directory + "fleets.xml");

            Date = new Serializer<Date>().Deserialize(directory + "date.xml");

            this.Player = player;
        }
        private void LoadCharacters(string directory) {

        }
    }
}