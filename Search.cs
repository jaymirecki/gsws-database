////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                                 Search.cs                                  //
//                  Search functions for the Database Class                   //
//            Created by: Jarett (Jay) Mirecki, February 01, 2020             //
//            Modified by: Jarett (Jay) Mirecki, February 06, 2020            //
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
using SearchResult = KeyValuePair<string, Type>;
using RankedResult = KeyValuePair<int, KeyValuePair<string, Type>>;
public partial class Database {
    public List<SearchResult> Search(string query, bool characters, bool factions, bool fleets, bool governments, bool planets) {
        List<RankedResult> rankedResults = new List<RankedResult>();
        List<SearchResult> results = new List<SearchResult>();

        if (factions)
            rankedResults.AddRange(SearchFactions(query));
        if (governments)
            rankedResults.AddRange(SearchGovernments(query));
        if (fleets)
            rankedResults.AddRange(SearchFleets(query));
        if (planets)
            rankedResults.AddRange(SearchPlanets(query));
        if (characters)
            rankedResults.AddRange(SearchCharacters(query));
        rankedResults.Sort(SortRankedResults);
        foreach(RankedResult p in rankedResults) {
            results.Add(p.Value);
        }
        return results;
    }
    private int SortRankedResults(RankedResult a, RankedResult b) {
        return a.Key.CompareTo(b.Key);
    }
    private bool Match(string comparison, string query) {
        return comparison.Contains(query);
    }
    private bool Matches(List<string> comparison, string query) {
        foreach(string s in comparison) {
            if (Match(s, query))
                return true;
        }
        return false;
    }
    private List<RankedResult> SearchCharacters(string query) {
        List<RankedResult> rankedResults = new List<RankedResult>();
        foreach (Character c in Characters.Values) {
            int rank = -1;
            if (Match(c.Name, query) || Match(c.ID, query))
                rank = 0;
            else if (Matches(c.Units, query))
                rank = 1;
            else if (Matches(c.Militaries, query) || Matches(c.Governments, query))
                rank = 2;
            else if (Matches(c.Factions, query) ||
                     Match(c.Species, query) ||
                     Match(c.Homeworld, query))
                rank = 3;
            else if (Matches(c.Factions, query))
                rank = 4;
            if (rank > -1)
                rankedResults.Add(new RankedResult(
                                    rank,
                                    new SearchResult(c.ID, c.GetType())));
        }
        return rankedResults;
    }
    private List<RankedResult> SearchFactions(string query) {
        List<RankedResult> rankedResults = 
            new List<RankedResult>();
        foreach (Faction f in Factions.Values) {
            int rank = -1;
            if (Match(f.Name, query) || Match(f.ID, query))
                rank = 0;
            else if (Match(f.Government, query) ||
                     Match(f.Military, query))
                rank = 1;
            if (rank > -1)
                rankedResults.Add(new RankedResult(
                                    rank,
                                    new SearchResult(f.ID, f.GetType())));
        }
        return rankedResults;
    }
    private List<RankedResult> SearchFleets(string query) {
        List<RankedResult> rankedResults = 
            new List<RankedResult>();
        foreach (Fleet f in Fleets.Values) {
            int rank = -1;
            if (Match(f.Name, query) || Match(f.ID, query))
                rank = 0;
            if (rank > -1)
                rankedResults.Add(new RankedResult(
                                    rank,
                                    new SearchResult(f.ID, f.GetType())));
        }
        return rankedResults;
    }
    private List<RankedResult> SearchGovernments(string query) {
        List<RankedResult> rankedResults = 
            new List<RankedResult>();
        foreach (Government g in Governments.Values) {
            int rank = -1;
            if (Match(g.Name, query) || Match(g.ID, query))
                rank = 0;
            if (rank > -1)
                rankedResults.Add(new RankedResult(
                                    rank,
                                    new SearchResult(g.ID, g.GetType())));
        }
        return rankedResults;
    }
    private List<RankedResult> SearchPlanets(string query) {
        List<RankedResult> rankedResults = 
            new List<RankedResult>();
        foreach (Planet p in Planets.Values()) {
            int rank = -1;
            if (Match(p.Name, query) || Match(p.ID, query))
                rank = 0;
            else if (Match(p.Faction, query))
                rank = 3;
            if (rank > -1)
                rankedResults.Add(new RankedResult(
                                    rank,
                                    new SearchResult(p.ID, p.GetType())));
        }
        return rankedResults;
    }
}
}