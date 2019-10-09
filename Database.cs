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

public class Database {
    private Graph <string, Planet> planets;
    private Dictionary<string, Character> characters;

    private void InitDatabase() {
        planets = new Graph<string, Planet>();
        characters = new Dictionary<string, Character>();
    }

    public Database() {
        InitDatabase();
    }

    public Planet GetPlanet(string planetId) {
        Planet p;
        if (planets.TryFind(planetId, out p))
            return p;
        return new Planet();
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
}