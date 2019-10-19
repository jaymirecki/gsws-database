using JMSuite;
using GSWS;

class Driver {
    static Database db;
    static string loadDirectory = "Data/Campaigns/";
    static Campaign[] camps;
    static void Main() {
        Testing.CheckExpect("Construct Database", ConstructDatabase, "success");
        Testing.CheckExpect("Load Campaigns", LoadCampaign, "Test (0 ABY)");
        Testing.CheckExpectTimed("Load Database from Campaign", 
                                 LoadCampaignDatabase, "Corellia");
        Testing.CheckExpect("Add Planet", AddPlanet, "success");
        Testing.CheckExpect("Date Test", DateTest, "1:0 ABY");
        Testing.CheckExpect("Date Test 2", DateTest2, "217:13 ABY");
        Testing.CheckExpect("Save Test", SaveTest, "saved");
        Testing.ReportTestResults();
    }
    static string ConstructDatabase() {
        db = new Database();
        return "success";
    }
    static string LoadCampaign() {
        camps = new Serializer<Campaign[]>().Deserialize(loadDirectory + "campaignList.xml");
        return camps[2].Name;
    }
    static string LoadCampaignDatabase() {
        Player player = new Player("Test Player", camps[2].FactionIDs[0]);
        db.LoadDatabase(loadDirectory + camps[2].ID + "/", player);
        return db.GetPlanet("corellia").Name;
    }
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
    static string DateTest() {
        Date d = new Date();
        return d.ToString();
    }
    static string DateTest2() {
        Date d = new Date();
        d.DateInt = d.DateInt + 5000;
        return d.ToString();
    }
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