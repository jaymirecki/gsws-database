////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                                 Search.cs                                  //
//                  Search functions for the Database Class                   //
//                 Created by: Jay Mirecki, February 01, 2020                 //
//                  Modified by: Jay Mirecki, March 17, 2020                  //
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
    public List<SearchResult> Search(string query, bool characters, bool factions, bool fleets, bool governments, bool militaries, bool planets) {
        List<RankedResult> rankedResults = new List<RankedResult>();
        List<SearchResult> results = new List<SearchResult>();


        if (governments || factions)
            rankedResults.AddRange(SearchGovernments(query));
        if (fleets)
            rankedResults.AddRange(SearchFleets(query));
        if (planets)
            rankedResults.AddRange(SearchPlanets(query));
        if (militaries)
            rankedResults.AddRange(SearchMilitaries(query));
        // if (characters)
        //     rankedResults.AddRange(SearchCharacters(query));
        rankedResults.Sort(SortRankedResults);
        foreach(RankedResult p in rankedResults) {
            results.Add(p.Value);
        }
        return results;
    }
    private int SortRankedResults(RankedResult a, RankedResult b) {
        return a.Key.CompareTo(b.Key);
    }
    private int BestRank(int current, int candidate) {
        return BestRank(current, candidate, 0);
    }
    private int BestRank(int current, int candidate, int modifier) {
        int rank;
        if (candidate > -1 && (candidate + modifier < current || current == -1))
            rank = candidate + modifier;
        else
            rank = current;
        return rank;
    }
    private bool Match(string comparison, string query) {
        return comparison.ToLower().Contains(query.ToLower());
    }
    private int Match(Government comparison, string query, bool fromSub, bool fromSuper) {
        int rank = -1;
        if (Match(comparison.Name, query))
            return 0;
        if (comparison.SuperGovernment != null && !fromSuper) {
            rank = BestRank(rank, 
                         Match(comparison.SuperGovernment, query, true, false), 
                         1);
        }
        if (!fromSub) {
            foreach(Planet p in comparison.MemberPlanets) {
                if (rank == 1)
                    return rank;
                rank = BestRank(rank, Match(p, query, false, true), 1);
            }
            rank = 
                BestRank(rank, Match(comparison.Military, query, false, true), 1);
        }
        return rank;
    }
    private int Match(Planet comparison, string query, bool fromSub, bool fromSuper) {
        int rank = -1;
        if (Match(comparison.Name, query) ||
            Match(comparison.Demonym, query))
            return 0;
        if (!fromSuper) {
            rank = BestRank(rank, 
                            Match(comparison.Government, query, true, true), 
                            1);
            if (Match(comparison.System, query))
                rank = BestRank(rank, 1);
            else if (Match(comparison.Sector, query))
                rank = BestRank(rank, 2);
            else if (Match(comparison.Region.ToString(), query))
                rank = BestRank(rank, 3);
        }
        return rank;
    }
    private int Match(Fleet comparison, string query, bool fromSub, bool fromSuper) {
        int rank = -1;
        if (Match(comparison.Name, query))
            return 0;
        if (!fromSuper) {
            if (comparison.Orbiting != null)
                rank = BestRank(rank, 
                                Match(comparison.Orbiting, query, true, false),
                                1);
            else if (comparison.Destination != null)
                rank = BestRank(rank, 
                                Match(comparison.Destination, query, true, false),
                                2);
            else if (comparison.NextStop != null)
                rank = BestRank(rank, 
                                Match(comparison.NextStop, query, true, false),
                                3);
        }
        rank = BestRank(rank, 
                        Match(comparison.Military, query, true, false), 
                        3);
        return rank;
    }
    private int Match(Military comparison, string query, bool fromSub, bool fromSuper) {
        int rank = -1;
        if (Match(comparison.Name, query))
            return 0;
        if (!fromSuper) {
            if (comparison.Government != null)
                rank = BestRank(rank, 
                                Match(comparison.Government, query, true, false), 
                                2);
            else if (comparison.SuperMilitary != null)
                rank = BestRank(rank,
                                Match(comparison.SuperMilitary, query, true, false), 
                                1);
        }
        if (!fromSub) {
            foreach(Fleet f in comparison.Fleets)
                rank = BestRank(rank,
                                Match(f, query, false, true),
                                2);
            foreach(Military m in comparison.Branches) {
                rank = BestRank(rank,
                                Match(m, query, false, true),
                                2);
            }
        }
        return rank;
    }
    private bool Matches(List<string> comparison, string query) {
        foreach(string s in comparison) {
            if (Match(s, query))
                return true;
        }
        return false;
    }
    private List<RankedResult> SearchFleets(string query) {
        List<RankedResult> rankedResults = 
            new List<RankedResult>();
        foreach (Fleet f in Fleets.Values) {
            int rank = Match(f, query, false, false);
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
            int rank = Match(g, query, false, false);
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
        foreach (Planet p in Planets.Values) {
            int rank = Match(p, query, false, false);
            if (rank > -1)
                rankedResults.Add(new RankedResult(
                                    rank,
                                    new SearchResult(p.ID, p.GetType())));
        }
        return rankedResults;
    }
    private List<RankedResult> SearchMilitaries(string query) {
        List<RankedResult> rankedResults = 
            new List<RankedResult>();
        foreach (Military m in Militaries.Values) {
            int rank = Match(m, query, false, false);
            if (rank > -1)
                rankedResults.Add(new RankedResult(
                                    rank,
                                    new SearchResult(m.ID, m.GetType())));
        }
        return rankedResults;
    }
}}