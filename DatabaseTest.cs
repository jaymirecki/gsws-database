////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                               DatabaseTest.cs                              //
//                       Testing file for Database class                      //
//             Created by: Jarett (Jay) Mirecki, October 09, 2019             //
//            Modified by: Jarett (Jay) Mirecki, February 01, 2020            //
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
        Testing.CheckExpectTimed("Load Database from Campaign", 
                                 LoadCampaignDatabase, LoadedDatabaseString);
        Testing.CheckExpect("Add Planet", AddPlanet, "success");
        Testing.CheckExpect("Date Test", DateTest, "00:00 1:0 ABY");
        Testing.CheckExpect("Date Test 2", DateTest2, "00:00 217:13 ABY");
        Testing.CheckExpect("Construct Fleet", ConstructFleet, "Fleet #20");
        Testing.CheckExpect("Add Fleet", AddFleet, "Test Fleet");
        Testing.CheckExpect("Add Many Fleets", AddMultipleFleets, "Fleet #1Test FleetFleet #2Fleet #3Fleet #4Fleet #5Fleet #6");
        Testing.CheckExpect("Test Character", TestCharacter, "success");
        Testing.CheckExpect("Search Test 1", SearchTest, "");
        Testing.CheckExpect("Save Test", SaveTest, "saved");
        Testing.ReportTestResults();
    }
    static string LoadedDatabaseString = "[ ][ {empire, Galactic Empire}, {newrepublic, New Republic}, ][ {Fleet1, Fleet #1}, ][ {empire, Galactic Empire}, {rebels, New Republic}, ]{Test Player, cis}00:00 1:0 ABY";
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
    #region Planets
    static string AddPlanet() {
        Planet test = 
            new Planet("Commenor", 
                       new Coordinate(0, 0, 0), 
                       new string[]{"Corellia"});

        db.AddPlanet(test);
        if (db.GetPlanet(test.ID) == test)
            return "success";
        return "fail";
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
        return db.GetFleet(fleet.ID).Name;
    }
    static string AddMultipleFleets() {
        db.AddFleet(db.NewFleet());
        db.AddFleet(db.NewFleet());
        db.AddFleet(db.NewFleet());
        db.AddFleet(db.NewFleet());
        db.AddFleet(db.NewFleet());
        string ret = "";
        foreach(Fleet f in db.GetFleets())
            ret += f.Name;
        return ret;
    }
    #endregion
    #region Characters
    static string TestCharacter() {
        Character c = new Character();
        db.AddCharacter(c);
        if (db.GetCharacter(c.ID) == c)
            return "success";
        return "failure";
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