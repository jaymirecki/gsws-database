////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                                  Fleet.cs                                  //
//                                 Fleet class                                //
//             Created by: Jarett (Jay) Mirecki, January 31, 2020             //
//             Modified by: Jarett (Jay) Mirecki, January 31, 2020            //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace GSWS {

[Serializable] public class Fleet {
    [XmlAttribute] public string ID;
    public string Name, Destination, NextStop;
    public Coordinate Position;
    public List<string> Ships;

    #region Constructing
    private void InitInstance() {
        ID = Database.GetFreshID();
        Name = "";
        Destination = NextStop = null;
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
}
}