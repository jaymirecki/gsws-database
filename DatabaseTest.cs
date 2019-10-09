using JMSuite;

class Driver {
    static Database db;
    static void Main() {
        Testing.CheckExpect("Construct Planet", ConstructPlanet, "");
        Testing.CheckExpect("Planet Value", PlanetValue, "0");
        Testing.CheckExpect("Construct Database", ConstructDatabase, "success");
        Testing.CheckExpect("Add Planet", AddPlanet, "success");
        Testing.CheckExpect("Date Test", DateTest, "1:0 ABY");
        Testing.CheckExpect("Date Test 2", DateTest2, "217:13 ABY");
        Testing.ReportTestResults();
    }
    static string ConstructPlanet() {
        Planet test = new Planet();
        return test.ID;
    }
    static string PlanetValue() {
        Planet test = new Planet();
        return test.Value().ToString();
    }
    static string ConstructDatabase() {
        db = new Database();
        return "success";
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
}