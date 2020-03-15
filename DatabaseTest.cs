////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                               DatabaseTest.cs                              //
//                       Testing file for Database class                      //
//             Created by: Jarett (Jay) Mirecki, October 09, 2019             //
//             Modified by: Jarett (Jay) Mirecki, March 15, 2020              //
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
                            "planet1character1fleet1testplanetfleet1nullgovernment1");
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
        Testing.CheckExpectTimed("Load Database from Campaign", 
                                 LoadCampaignDatabase, LoadedDatabaseString);
        Testing.CheckExpect("Get Planet's Fleets", TestPlanetFleets, "coruscant");
        Testing.CheckExpect("Date Test", DateTest, "00:00 1:0 ABY");
        Testing.CheckExpect("Date Test 2", DateTest2, "00:00 217:13 ABY");
        Testing.CheckExpect("Construct Fleet", ConstructFleet, "Fleet #20");
        Testing.CheckExpect("Add Fleet", AddFleet, "Test Fleet");
        Testing.CheckExpect("Add Many Fleets", AddMultipleFleets, "Fleet #1Coruscant Defense FleetTest FleetFleet #2Fleet #3Fleet #4Fleet #5Fleet #6");
        Testing.CheckExpect("Get Fleet's Planet", TestFleetPlanet, "coruscant");
        Testing.CheckExpect("Test Character", TestCharacter, "character1");
        Testing.CheckExpect("Search Test 1", SearchTest, "");
        Testing.CheckExpect("Save Test", SaveTest, "saved");
        Testing.ReportTestResults();
    }
    static string LoadedDatabaseString = "[ ][ {Fleet1, Fleet #1}, {coruscant, Coruscant Defense Fleet}, ][ {empire, Galactic Empire}, {rebels, New Republic}, ][ {corellia, Corellia}, {coruscant, Coruscant}, {commenor, Commenor}, ]{Test Player, cis}00:00 1:0 ABY";
    static string ConstructDatabase() {
        db = new Database();
        return "success";
    }
    #region Campaigns
    static string LoadCampaign() {
        camps = new Serializer<Campaign[]>().Deserialize(loadDirectory + "campaignList.xml");
        return camps[0].Name + camps[1].Name + camps[2].Name + camps.Length.ToString();
    }
    static string LoadCampaignDatabase() {
        Player player = new Player("Test Player", camps[2].FactionIDs[0]);
        db.LoadDatabase(loadDirectory + camps[2].ID + "/", player);
        return db.ToString();
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
        string results = "";
        foreach(KeyValuePair<string, Type> p in db.Search("empireempirecorelliabilbringicoruscantfondor", true, true, true, true, true)) {
            results += p.Key;
        }
        return results;
    }
    #endregion
    static string SaveTest() {
        db.Save("Data/Saves/Test/");
        return "saved";
    }
}