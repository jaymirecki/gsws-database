////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                                  Save .cs                                  //
//               Save and Load functions for the Database Class               //
//                  Created by: Jay Mirecki, October 19, 2019                 //
//                  Modified by: Jay Mirecki, March 26, 2020                  //
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
using System.Diagnostics;

namespace GSWS {
public partial class Database {
    #region Save
    public bool DoesSaveExist(string directory, string saveName) {
        HashSet<string> saves = new HashSet<string>(Directory.GetDirectories(directory));
        return saves.Contains(directory + saveName);
    }
    public void Save(string directory) {
        Directory.CreateDirectory(directory);
        Bodies.SerializeXml(directory + "bodies.xml");
        Planets.SerializeXml(directory + "planets.xml");
        Governments.SerializeXml(directory + "governments.xml");
        Characters.SerializeXml(directory + "characters.xml");
        Fleets.SerializeXml(directory + "fleets.xml");
        Map.SerializeXml(directory + "map.xml");
        Militaries.SerializeXml(directory + "militaries.xml");

        new Serializer<Date>().Serialize(directory + "date.xml", Date);

        new Serializer<Player>().Serialize(directory + "player.xml", Player);
    }
    #endregion
    #region Load
    public List<KeyValuePair<string, DateTime>> GetSaves(string directory) {
        List<KeyValuePair<string, DateTime>> saves = new List<KeyValuePair<string, DateTime>>();
        foreach (string s in Directory.GetDirectories(directory)) {
            saves.Add(new KeyValuePair<string, DateTime>(s.Substring(directory.Length), Directory.GetLastWriteTime(s)));
        }
        return saves;
    }
    // Loading database from file
    public void LoadDatabase(string directory, bool loadPlayer) {
        Bodies = JDictionary<string, Body>.DeserializeXml(directory + "bodies.xml");
        Planets = JDictionary<string, Planet>.DeserializeXml(directory + "planets.xml");
        Governments = JDictionary<string, Government>.DeserializeXml(directory + "governments.xml");
        Fleets = JDictionary<string, Fleet>.DeserializeXml(directory + "fleets.xml");
        Militaries = JDictionary<string, Military>.DeserializeXml(directory + "militaries.xml");
        Map = JUndirectedGraph<string>.DeserializeXml(directory + "map.xml");
        Date = new Serializer<Date>().Deserialize(directory + "date.xml");
        if (loadPlayer)
            Player = new Serializer<Player>().Deserialize(directory + "player.xml");
        else
            Player = new Player();
        UpdateValues(loadPlayer);
        UpdateSuperGroups();
        VerifySubGroups();
    }
    public void LoadDatabase(string directory, string playerFaction) {
        LoadDatabase(directory, false);
        Player.kFaction = playerFaction;
        Player.UpdateValues(this);
        // Player.Character
    }
    #endregion
    #region Ensure Value Integrity
    private void UpdateValues(bool updatePlayer) {
        foreach (JUndirectedGraph<string>.Edge e in Map.Edges) {
            Map.AddEdge(e.Origin, e.Destination, (int)Planets[e.Origin].Position.DistanceTo(Planets[e.Destination].Position));
        }
        UpdatePlanetValues();
        UpdateFleetValues();
        UpdateGovernmentValues();
        UpdateMilitaryValues();
        if (updatePlayer)
            Player.UpdateValues(this);
    }
    private void UpdateFleetValues() {
        foreach (Fleet f in Fleets.Values) {
            f.UpdateValues(this);
        }
    }
    private void UpdateGovernmentValues() {
        foreach (Government g in Governments.Values)
            g.UpdateValues(this);
    }
    private void UpdateMilitaryValues() {
        foreach (Military m in Militaries.Values) {
            m.UpdateValues(this);
        }
    }
    private void UpdatePlanetValues() {
        foreach (Planet p in Planets.Values) {
            p.UpdateValues(this);
        }
    }
    #endregion
    #region Ensure Key Integrity
    private void UpdateKeys() {
        UpdateFleetKeys();
        UpdateGovernmentKeys();
        UpdateMilitaryKeys();
        UpdatePlanetKeys();
        Player.UpdateKeys();
    }
    private void UpdateFleetKeys() {
        foreach (Fleet f in Fleets.Values)
            f.UpdateKeys();
    }
    private void UpdateGovernmentKeys() {
        foreach (Government g in Governments.Values)
            g.UpdateKeys();
    }
    private void UpdateMilitaryKeys() {
        foreach (Military m in Militaries.Values)
            m.UpdateKeys();
    }
    private void UpdatePlanetKeys() {
        foreach (Planet p in Planets.Values) {
            p.UpdateKeys();
        }
    }
    #endregion
    #region Update SuperGroups
    private void UpdateSuperGroups() {
        UpdateFleetSuperGroups();
        UpdateGovernmentSuperGroups();
        UpdateMilitarySuperGroups();
        UpdatePlanetSuperGroups();
    }
    private void UpdateFleetSuperGroups() {
        foreach (Fleet f in Fleets.Values) {
            f.UpdateSuperGroups();
        }
    }
    private void UpdateGovernmentSuperGroups() {
        foreach (Government g in Governments.Values)
            g.UpdateSuperGroups();
    }
    private void UpdateMilitarySuperGroups() {
        foreach (Military m in Militaries.Values) {
            m.UpdateSuperGroups();
        }
    }
    private void UpdatePlanetSuperGroups() {
        foreach (Planet p in Planets.Values)
            p.UpdateSuperGroups();
    }
    #endregion
    #region Verify Sub Groups
    private void VerifySubGroups() {
        VerifyFleetSubGroups();
        VerifyGovernmentSubGroups();
        VerifyMilitarySubGroups();
        VerifyPlanetSubGroups();
    }
    private void VerifyFleetSubGroups() {
        foreach (Fleet f in Fleets.Values) {
            f.VerifySubGroups();
        }
    }
    private void VerifyGovernmentSubGroups() {
        foreach (Government g in Governments.Values) {
            g.VerifySubGroups();
        }
    }
    private void VerifyMilitarySubGroups() {
        foreach (Military m in Militaries.Values) {
            m.VerifySubGroups();
        }
    }
    private void VerifyPlanetSubGroups() {
        foreach (Planet p in Planets.Values) {
            p.VerifySubGroups();
        }
    }
    #endregion
}}