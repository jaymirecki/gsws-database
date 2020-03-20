////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                               DatabaseTest.cs                              //
//                       Testing file for Database class                      //
//                  Created by: Jay Mirecki, October 09, 2019                 //
//                  Modified by: Jay Mirecki, March 19, 2020                  //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////
using JMSuite;
using System;
using System.Collections.Generic;
using GSWS;

class Driver {
    static Database db;
    static string loadDirectory = "Data/Campaigns/";
    static Campaign[] camps;
    static void Main() {
        Testing.CheckExpect("Construct Database", ConstructDatabase, "success");
        Testing.CheckExpect("Load Campaigns", LoadCampaign, 
                            "Post Endor (5 ABY)Second Galactic Civil War (44 ABY)Test (0 ABY)3");
        #region Database.cs
        Testing.CheckExpect("Database.FreshID",
                            FreshIDs,
                            "planet1planet2");
        Testing.CheckExpect("Class Specific Database.FreshID",
                            ClassFreshIDs,
                            "planet1character1fleet1testplanetfleet1government1");
        // Testing.CheckExpect("Database.FreshName",
        //                     FreshNames,
        //                     "");
        // Testing.CheckExpect("Class Specific Database.FreshName",
        //                     ClassFreshnames,
        //                     "");
        // Testing.CheckExpect("Database.AddPlanet",
        //                     AddPlanet,
        //                     "");
        // Testing.CheckExpect("Database.AddCharacter",
        //                     AddCharacter,
        //                     "");
        // Testing.CheckExpect("Database.AddFleet",
        //                     AddFleet,
        //                     "");
        // Testing.CheckExpect("Database.AddGovernment",
        //                     AddGovernment,
        //                     "");
        // Testing.CheckExpect("Nameless Database.NewFleet",
        //                     NamelessNewFleet,
        //                     "");
        // Testing.CheckExpect("Database.NewFleet",
        //                     NewFleet,
        //                     "");
        #endregion
        #region Save.cs
        Testing.CheckExpectTimed("Load Database from Campaign", 
                                 LoadCampaignDatabase, LoadedDatabaseString);
        Testing.CheckExpect("Save Test", SaveTest, "saved");
        Testing.CheckExpect("Save Does Not Exist", FailSaveExist, "False");
        Testing.CheckExpect("Save Does Exist", SuccessSaveExist, "True");
        Testing.CheckExpect("List Save Files", SaveFiles, "Test");
        #endregion
        Testing.CheckExpect("Get Planet's Fleets", TestPlanetFleets, "coruscant");
        Testing.CheckExpect("Date Test", DateTest, "00:00 1:0 ABY");
        Testing.CheckExpect("Date Test 2", DateTest2, "00:00 217:13 ABY");
        Testing.CheckExpect("Construct Fleet", ConstructFleet, "Fleet #10");
        Testing.CheckExpect("Add Fleet", AddFleet, "Test Fleet");
        Testing.CheckExpect("Add Many Fleets", AddMultipleFleets, "Coruscant Defense FleetImperial Corellia FleetNew Republic First FleetTest FleetFleet #1Fleet #2Fleet #3Fleet #4Fleet #5");
        Testing.CheckExpect("Get Fleet's Planet", TestFleetPlanet, "coruscant");
        Testing.CheckExpect("Test Character", TestCharacter, "character1");
        Testing.CheckExpect("Search Test 1", SearchTest, "empirecorelliacoruscantimparmyimpnavyempirecorelliacoruscant");
        Testing.CheckExpect("Coordinate Move", CoordinateMove, "244.9204");
        Testing.ReportTestResults();
    }
    static string LoadedDatabaseString = "[ ][ {coruscant, Coruscant Defense Fleet}, {corellia, Imperial Corellia Fleet}, {commenor, New Republic First Fleet}, ][ {empire, Galactic Empire}, {republic, New Republic}, ][ {corellia, Corellia}, {coruscant, Coruscant}, {commenor, Commenor}, {dac, Dac}, ]{empire}00:00 1:0 ABY";
    static string ConstructDatabase() {
        db = new Database();
        return "success";
    }
    #region Campaigns
    static string LoadCampaign() {
        camps = new Serializer<Campaign[]>().Deserialize(loadDirectory + "campaignList.xml");
        return camps[0].Name + camps[1].Name + camps[2].Name + camps.Length.ToString();
    }
    #endregion
    #region Saves
    static string LoadCampaignDatabase() {
        db.LoadDatabase(loadDirectory + camps[2].ID + "/", "empire");
        Character c = new Character();
        // player.Character = db.Characters["Test Player"];
        return db.ToString();
    }
    static string SaveTest() {
        db.Save("Data/Saves/Test/");
        return "saved";
    }
    static string FailSaveExist() {
        return db.DoesSaveExist("Data/Saves/", "Fail").ToString();
    }
    static string SuccessSaveExist() {
        return db.DoesSaveExist("Data/Saves/", "Test").ToString();
    }
    static string SaveFiles() {
        string saves = "";
        foreach (KeyValuePair<string, DateTime> s in db.GetSaves("Data/Saves/"))
            saves += s.Key;
        return saves;
    }
    #endregion
    #region Database
    static string FreshIDs() {
        string result = "";
        result += db.FreshID(new HashSet<string>(), "", "planet");
        result += db.FreshID(new HashSet<string>(){ "planet1" }, "", "planet");
        return result;
    }
    static string ClassFreshIDs() {
        string result = "";
        result += db.FreshPlanetID(new Planet());
        result += db.FreshCharacterID(new Character());
        result += db.FreshFleetID(new Fleet());
        Planet p = new Planet();
        p.Name = "testplanet";
        Fleet f = new Fleet();
        f.Orbiting = p;
        result += db.FreshFleetID(f);
        result += db.FreshGovernmentID(new Government());
        return result;
    }
    #endregion
    #region Planets
    static string AddPlanet() {
        Planet test = new Planet();

        db.AddPlanet(test);
        if (db.Planets[test.ID] == test)
            return "success";
        return "fail";
    }
    static string TestPlanetFleets() {
        var fs = db.Planets["coruscant"].Fleets.GetEnumerator();
        fs.MoveNext();
        return fs.Current.ID;
    }
    #endregion
    #region Dates
    static string DateTest() {
        Date d = new Date();
        return d.ToString();
    }
    static string DateTest2() {
        Date d = new Date();
        d.DateInt = d.DateInt + (5000 * 24);
        return d.ToString();
    }
    #endregion
    #region Fleets
    static string ConstructFleet() {
        Fleet fleet = db.NewFleet();
        return fleet.Name + fleet.Ships.Count.ToString();
    }
    static string AddFleet() {
        Fleet fleet = db.NewFleet("Test Fleet");
        db.AddFleet(fleet);
        return db.Fleets[fleet.ID].Name;
    }
    static string AddMultipleFleets() {
        db.AddFleet(db.NewFleet());
        db.AddFleet(db.NewFleet());
        db.AddFleet(db.NewFleet());
        db.AddFleet(db.NewFleet());
        db.AddFleet(db.NewFleet());
        string ret = "";
        foreach(Fleet f in db.Fleets.Values)
            ret += f.Name;
        return ret;
    }
    static string TestFleetPlanet() {
        Fleet f = db.Fleets["coruscant"];;
        return f.Orbiting.ID;
    }
    #endregion
    #region Characters
    static string TestCharacter() {
        Character c = new Character();
        db.AddCharacter(c);
        return c.ID;
    }
    #endregion
    #region Search
    static string SearchTest() {
        LoadCampaignDatabase();
        string results = "";
        foreach(KeyValuePair<string, Type> p in db.Search("empire", true, true, true, true, true, true)) {
            results += p.Key;
        }
        return results;
    }
    #endregion
    #region Coordinate
    static string CoordinateMove() {
        Coordinate a = new Coordinate(0, 0, 0);
        Coordinate b = new Coordinate(100, 256, 10);
        Coordinate c = a.MoveTowards(b, 30);
        return c.DistanceTo(b).ToString();
    }
    #endregion
}