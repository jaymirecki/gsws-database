using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using JMSuite.Collections;

public class Save : MonoBehaviour {
    void Awake() {

    }
    void Start() {

    }
    void Update() {

    }
    public void SaveGame() {
        string saveName = "TestSave";
        string directory = Application.persistentDataPath + "/Data/Saves/" + saveName + "/";
        Directory.CreateDirectory(directory);

        new Serializer<List<Planet>>().Serialize(directory + "planets.xml", Game.Instance.Planets.Values());

        new Serializer<Faction>().SerializeDictionary(directory + "factions.xml", Game.Instance.Factions);

        new Serializer<Government>().SerializeDictionary(directory + "governments.xml", Game.Instance.Governments);
        
        // new Serializer<Character>().SerializeDictionary(directory + "characters.xml", Game.Instance.Characters);

        new Serializer<Date>().Serialize(directory + "date.xml", Game.Instance.Date);
    }
}