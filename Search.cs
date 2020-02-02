////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                                 Search.cs                                  //
//                  Search functions for the Database Class                   //
//            Created by: Jarett (Jay) Mirecki, February 01, 2020             //
//            Modified by: Jarett (Jay) Mirecki, February 01, 2020            //
//                                                                            //
//          This extension for the Database class allows for                  //
//          searching the objects (to provide results for the                 //
//          Datapad in the simulation)                                        //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using JMSuite.Collections;

namespace GSWS {
public partial class Database {
    public List<string> Search(string query, bool characters, bool factions, bool fleets, bool governments, bool planets) {
        List<KeyValuePair<string, int>> rankedResults = new List<KeyValuePair<string, int>>();
        List<string> results = new List<string>();

        if (characters) {
            foreach (Character c in Characters.Values) {
                rankedResults.AddRange(SearchCharacters(query));
            }
        }
        if (factions) {
            foreach (Faction f in Factions.Values) {
                rankedResults.AddRange(SearchFactions(query));
            }
        }
        if (fleets) {
            foreach (Fleet f in Fleets.Values) {
                rankedResults.AddRange(SearchFleets(query));
            }
        }
        if (governments) {
            foreach (Government g in Governments.Values) {
                rankedResults.AddRange(SearchGovernments(query));
            }
        }
        if (planets) {
            foreach (Planet p in Planets.Values()) {
                rankedResults.AddRange(SearchPlanets(query));
            }
        }
        // rankedResults.Sort()
        foreach(KeyValuePair<string, int> p in rankedResults)
            results.Add(p.Key);
        return results;
    }
    private bool Match(string comparison, string query) {
        return comparison.Length <= query.Length && 
               comparison.Substring(0, query.Length) == query;
    }
    private List<KeyValuePair<string, int>> SearchCharacters(string query) {
        List<KeyValuePair<string, int>> rankedResults = 
            new List<KeyValuePair<string, int>>();
        foreach (Character c in Characters.Values) {
            int rank = -1;
            if (Match(c.Name, query))
                rank = 0;
            else if (Match(c.Unit, query))
                rank = 1;
            else if (Match(c.Military, query) || Match(c.Government, query))
                rank = 2;
            else if (Match(c.Faction, query) ||
                     Match(c.Species, query) ||
                     Match(c.Homeworld, query))
                rank = 3;
            if (rank > -1)
                rankedResults.Add(new KeyValuePair<string, int>(c.ID, rank));
        }
        return rankedResults;
    }
    private List<KeyValuePair<string, int>> SearchFactions(string query) {
        List<KeyValuePair<string, int>> rankedResults = 
            new List<KeyValuePair<string, int>>();
        foreach (Faction f in Factions.Values) {
            int rank = -1;
            if (Match(f.Name, query))
                rank = 0;
            else if (Match(f.Government, query) ||
                     Match(f.Military, query))
                rank = 1;
            if (rank > -1)
                rankedResults.Add(new KeyValuePair<string, int>(f.ID, rank));
        }
        return rankedResults;
    }
    private List<KeyValuePair<string, int>> SearchFleets(string query) {
        List<KeyValuePair<string, int>> rankedResults = 
            new List<KeyValuePair<string, int>>();
        foreach (Fleet f in Fleets.Values) {
            int rank = -1;
            if (Match(f.Name, query))
                rank = 0;
            if (rank > -1)
                rankedResults.Add(new KeyValuePair<string, int>(f.ID, rank));
        }
        return rankedResults;
    }
    private List<KeyValuePair<string, int>> SearchGovernments(string query) {
        List<KeyValuePair<string, int>> rankedResults = 
            new List<KeyValuePair<string, int>>();
        foreach (Government g in Governments.Values) {
            int rank = -1;
            if (Match(g.Name, query))
                rank = 0;
            if (rank > -1)
                rankedResults.Add(new KeyValuePair<string, int>(g.ID, rank));
        }
        return rankedResults;
    }
    private List<KeyValuePair<string, int>> SearchPlanets(string query) {
        List<KeyValuePair<string, int>> rankedResults = 
            new List<KeyValuePair<string, int>>();
        foreach (Planet p in Planets.Values) {
            int rank = -1;
            if (Match(p.Name, query))
                rank = 0;
            if (rank > -1)
                rankedResults.Add(new KeyValuePair<string, int>(p.ID, rank));
        }
        return rankedResults;
    }
}
}