////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                                 Faction.cs                                 //
//                               Faction class                                //
//              Created by: Jarett (Jay) Mirecki, July 27, 2019               //
//            Modified by: Jarett (Jay) Mirecki, October 09, 2019             //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Drawing;

namespace GSWS {

[Serializable]
public class Relationship {
    public string Faction;
    public int RelationshipRating;
    public Relationship() {}
    public Relationship(string faction, int relationshipRating) {
        Faction = faction;
        RelationshipRating = relationshipRating;
    }
}

[Serializable]
public class Faction {
    [XmlAttribute]
    public string ID;
    public string Name, Government, Military, Color;
    public List<Relationship> Relations;

    [Serializable]
    public struct Relationship {
        public string Faction;
        public int RelationshipRating;
        public Relationship(string faction, int relationshipRating) {
            Faction = faction;
            RelationshipRating = relationshipRating;
        }
    }

    private void InitInstance() {
        Relations = new List<Relationship>();
        Name = ID = Government = Military = Color = "";

    }
    public Faction() {
        InitInstance();
    }

    public Faction(string name) {
        InitInstance();
        this.Name = name;
    }
    public void SetRelationship(string faction, int relationshipRating) {

        int index =  Relations.IndexOf(Relations.Find(x => x.Faction == faction));
        Relations.RemoveAt(index);
        Relations.Insert(index, new Relationship(faction, relationshipRating));
    }
    public void AddRelationship(string faction, int relationshipRating) {
        Relations.Add(new Relationship(faction, relationshipRating));
    }
    public int GetRelationship(string faction) {
        return Relations.Find(r => r.Faction == faction).RelationshipRating;
    }
}
}