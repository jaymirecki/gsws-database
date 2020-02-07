////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                                  Fleet.cs                                  //
//                                 Fleet class                                //
//             Created by: Jarett (Jay) Mirecki, January 31, 2020             //
//            Modified by: Jarett (Jay) Mirecki, February 06, 2020            //
//                                                                            //
//          The Fleet class is a representation of groups of space            //
//          vessels, allowing them to move as groups.                         //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace GSWS {

[Serializable] public class Fleet {
    [XmlAttribute] public string ID;
    public string Name, Destination, NextStop, Orbiting;
    public Coordinate Position;
    public List<string> Ships;

    #region Constructing
    private void InitInstance() {
        ID = Database.GetFreshID();
        Name = "";
        Destination = NextStop = Orbiting = null;
        Position = new Coordinate(0, 0, 0);
        Ships = new List<string>();
    }
    public Fleet() {
        InitInstance();
    }
    public Fleet(string name) {
        InitInstance();
        Name = name;
    }
    #endregion
    public override string ToString() {
        return "{" + ID + ", " + Name + "}";
    }
    public string DatapadDescription() {
        string description = 
            Name;
        return description;
    }
    public bool Stationary() {
        return Destination == null;
    }
}
}