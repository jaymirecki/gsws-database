using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using JMSuite.Collections;

public partial class Game : MonoBehaviour
{
    public static Game Instance;
    public Date Date;
    public Player Player;
    public Graph<string, Planet> Planets;
    public Dictionary<string, Faction> Factions;
    public Dictionary<string, Government> Governments;
    public Dictionary<string, Character> Characters;
    public Vector3 MapCameraLocation;

    void Awake () {
        if (Instance == null) {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        } else if (Instance != this) {
            Destroy (gameObject);
        }
    }
    public static void SetInstance(string playerName, string factionName, int funds, string date) {
        Instance.Player = new Player(playerName);
        Instance.Date = new Date(1840, DateSystem.ABY);
        Instance.Planets = new Graph<string, Planet>();
        Instance.MapCameraLocation = new Vector3(0f, 0f, 0f);
    }
    public static string DateToString() {
        return Instance.Date.ToString();
    }
    public static void InitCampaign(string campaignName, string factionID, Date date) {
        string directory = 
            Application.persistentDataPath + "/Data/Campaigns/" + campaignName + "/";
        Debug.Log(directory);
        Debug.Log(Directory.GetCurrentDirectory());
        Instance.Player.Faction = factionID;
        Instance.Date = date;
        Instance.LoadPlanets(directory);
        Instance.LoadGovernments(directory);
        Instance.LoadFactions(directory);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

        Factions = new Dictionary<string, Faction>();
        foreach(Faction aFaction in factionList)
            Factions.Add(aFaction.ID, aFaction);
        foreach(Planet aPlanet in Planets.Values()) {
            Faction pFaction;
            if (Factions.TryGetValue(aPlanet.Faction, out pFaction)) {
                if (!GetGovernmentFromString(pFaction.Government).MemberPlanets.Contains(aPlanet.ID))
                    GetGovernmentFromString(pFaction.Government).MemberPlanets.Add(aPlanet.ID);
            }
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

        Governments = new Dictionary<string, Government>();
        foreach(Government aGovernment in governmentList)
            Governments.Add(aGovernment.ID, aGovernment);
    }
    private void LoadCharacters(string directory) {

    }
    static public Planet GetPlanetFromString(string planet) {
        return Game.Instance.Planets.Find(planet);
    }
    public Faction GetFactionFromString(string faction) {
        Faction Faction;
        if (Factions.TryGetValue(faction, out Faction))
            return Faction;
        else
            return new Faction();
    }
    public Government GetGovernmentFromString(string government) {
        Government Government;
        if (Governments.TryGetValue(government, out Government))
            return Government;
        else
            return new Government();
    }
    public static Faction GetPlayerFaction() {
        return Game.Instance.GetFactionFromString(Game.Instance.Player.Faction);
    }
    public static Government GetPlayerGovernment() {
        return Game.Instance.GetGovernmentFromString(GetPlayerFaction().Government);
    }
    public static string CreditString(float value) {
        string valueString;
        if (value > 1000000000000000000000000000f)
            valueString =
                value.ToString("###,###,###,###,###,,,,,,,,,.00") + " octillion";
        else if (value > 1000000000000000000000000f)
            valueString = 
                value.ToString("###,###,###,###,###,,,,,,,,.00") + " septillion";
        else if (value > 1000000000000000000000f)
            valueString = 
                value.ToString("###,###,###,###,###,,,,,,,.00") + " sextillion";
        else if (value > 1000000000000000000f)
            valueString = 
                value.ToString("###,###,###,###,###,,,,,,.00") + " quintillion";
        else if (value > 1000000000000000f)
            valueString = 
                value.ToString("###,###,###,###,###,,,,,.00") + " quadrillion";
        else if (value > 1000000000000f)
            valueString = 
                value.ToString("###,###,###,###,###,,,,.00") + " trillion";
        else if (value > 1000000000f)
            valueString = 
                value.ToString("###,###,###,###,###,,,.00") + " billion";
        else if (value > 1000000f)
            valueString = 
                value.ToString("###,###,###,###,###,,.00") + " million";
        else
            valueString = 
                value.ToString("###,##0");
        return valueString + " credits";
    }
}
