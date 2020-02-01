using JMSuite;
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
                                 LoadCampaignDatabase, "Corellia");
        Testing.CheckExpect("Add Planet", AddPlanet, "success");
        Testing.CheckExpect("Date Test", DateTest, "1:0 ABY");
        Testing.CheckExpect("Date Test 2", DateTest2, "217:13 ABY");
        Testing.CheckExpect("Construct Fleet", ConstructFleet, "Fleet #10");
        Testing.CheckExpect("Add Fleet", AddFleet, "Test Fleet");
        Testing.CheckExpect("Add Many Fleets", AddMultipleFleets, "Test FleetFleet #1Fleet #2Fleet #3Fleet #4Fleet #5");
        Testing.CheckExpect("Save Test", SaveTest, "saved");
        Testing.ReportTestResults();
    }
    static string ConstructDatabase() {
        db = new Database();
        return "success";
    }
    #region // Campaigns
    static string LoadCampaign() {
        camps = new Serializer<Campaign[]>().Deserialize(loadDirectory + "campaignList.xml");
        return camps[0].Name + camps[1].Name + camps[2].Name + camps.Length.ToString();
    }
    static string LoadCampaignDatabase() {
        Player player = new Player("Test Player", camps[2].FactionIDs[0]);
        db.LoadDatabase(loadDirectory + camps[2].ID + "/", player);
        return db.GetPlanet("corellia").Name;
    }
    #endregion
    #region // Planets
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
    #region // Dates
    static string DateTest() {
        Date d = new Date();
        return d.ToString();
    }
    static string DateTest2() {
        Date d = new Date();
        d.DateInt = d.DateInt + 5000;
        return d.ToString();
    }
    #endregion
    #region // Fleets
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
    static string TestCharacter() {
        Character c = new Character();
        db.AddCharacter(c);
        if (db.GetCharacter(c.ID) == c)
            return "success";
        return "failure";
    }
    static string SaveTest() {
        db.Save("Data/Saves/Test/");
        return "saved";
    }
}