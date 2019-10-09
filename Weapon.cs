////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                                 Weapon.cs                                  //
//                                Weapon class                                //
//            Created by: Jarett (Jay) Mirecki, September 02, 2019            //
//            Modified by: Jarett (Jay) Mirecki, October 09, 2019             //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace GSWS {
[Serializable] public class Weapon {
    [XmlAttribute] public string ID;
    public string Name;
    public float Damage, Accuracy;
    public bool Independent;
    public int Capacity;

    void InitInstance() {
        ID = Name = "";
        Damage = Accuracy = 0f;
        Independent = false;
        Capacity = 0;
    }
    public Weapon() {
        InitInstance();
    }
}
}