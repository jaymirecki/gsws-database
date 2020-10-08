////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                                   Ship.cs                                  //
//                                 Ship class                                 //
//             Created by: Jarett (Jay) Mirecki, October 16, 2019             //
//             Modified by: Jarett (Jay) Mirecki, October 16, 2019            //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace GSWS {

[Serializable] public class Ship {
    public string ID, Name, Model;
    public List<string> Complement, Passengers, AssociatedUnits, Commanders;

    private void InitInstance() {
        ID = Name = Model = "";
        Complement = new List<string>();
        Passengers = new List<string>();
        AssociatedUnits = new List<string>();
        Commanders = new List<string>();
    }
    public Ship() {
        InitInstance();
    }
}

}