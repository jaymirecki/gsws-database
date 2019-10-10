////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                                 Database.cs                                //
//                               Database class                               //
//             Created by: Jarett (Jay) Mirecki, October 09, 2019             //
//             Modified by: Jarett (Jay) Mirecki, October 09, 2019            //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using JMSuite.Collections;

namespace GSWS {
public class Database {
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

    public Planet GetPlanet(string planetId) {
        Planet p;
        if (planets.TryFind(planetId, out p))
            return p;
        return new Planet();
    }

    public Graph<string, Planet> GetPlanets() {
        return planets;
    }

    public void AdvanceTime() {
        date.AdvanceTime();
    }

    public void AddPlanet(Planet p) {
        // int[] distances = new int[p.Neighbors.Length];
        // foreach ()
        planets.Add(p.ID, p, p.Neighbors);
    }

    public void AddCharacter(Character c) {
        characters.Add(c.ID, c);
    }

    public Character GetCharacter(string characterId) {
        Character c;
        if (characters.TryGetValue(characterId, out c))
            return c;
        return new Character();
    }

    public Planet GetHomeworld(string characterId) {
        return GetHomeworld(GetCharacter(characterId));
    }

    public Planet GetHomeworld(Character c) {
        return GetPlanet(c.Homeworld);
    }

    public Faction GetFaction(string factionId) {
        Faction f;
        if (factions.TryGetValue(factionId, out f))
            return f;
        return new Faction();
    }

    public Faction GetFaction(Planet p) {
        return GetFaction(p.Faction);
    }

    public Government GetGovernment(string governmentId) {
        Government g;
        if (governments.TryGetValue(governmentId, out g))
            return g;
        return new Government();
    }

    public Government GetGovernment(Faction f) {
        return GetGovernment(f.Government);
    }

    public Character GetPlayerCharacter() {
        return GetCharacter(player.Character);
    }

    public Faction GetPlayerFaction() {
        return GetFaction(player.Faction);
    }
    public Government GetPlayerGovernment() {
        return GetGovernment(GetFaction(player.Faction).Government);
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

    public void Save(string directory) {
        new Serializer<List<Planet>>().Serialize(directory + "planets.xml", planets.Values());

        new Serializer<Faction>().SerializeDictionary(directory + "factions.xml", factions);

        new Serializer<Government>().SerializeDictionary(directory + "governments.xml", governments);
        
        // new Serializer<Character>().SerializeDictionary(directory + "characters.xml", Game.Instance.Characters);

        new Serializer<Date>().Serialize(directory + "date.xml", date);
    }
    
    // Loading database from file
    public void LoadDatabase(string directory) {
        LoadPlanets(directory);
        LoadFactions(directory);
        LoadGovernments(directory);
    }
    private void LoadPlanets(string directory) {
        List<Planet> planetList = new List<Planet>();
        Serializer<List<Planet>> PlanetListSerializer = 
            new Serializer<List<Planet>>();
        planetList = 
            PlanetListSerializer.Deserialize(directory + "planets.xml");
        foreach(Planet aPlanet in planetList) 
            planets.Add(aPlanet.ID, aPlanet, aPlanet.Neighbors);
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
            factions.Add(aFaction.ID, aFaction);
        foreach(Planet aPlanet in planets.Values()) {
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
            governments.Add(aGovernment.ID, aGovernment);
    }
    private void LoadCharacters(string directory) {

    }
}
}