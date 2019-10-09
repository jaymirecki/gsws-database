////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                                ShipModel.cs                                //
//                               ShipModel class                              //
//            Created by: Jarett (Jay) Mirecki, September 02, 2019            //
//            Modified by: Jarett (Jay) Mirecki, September 02, 2019           //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Collections.Generic;

public enum ShipClass { Interceptor, Fighter, Bomber, Corvette, Frigate, LightCruiser, HeavyCruiser, Destroyer, Battlecruiser, Dreadnought }

[Serializable] public class ShipModel {
    [XmlAttribute] public string ID;
    public string Name, Manufacturer;
    public float HyperdriveRating, Price, Crew;
    ShipClass Class;
    List<string> Weapons;
    public int Complement, Passengers, Speed, Acceleration, Shields, HullStrength;

    void InitInstance() {
        ID = Name = Manufacturer = "";
        HyperdriveRating = Price = Crew = 0f;
        Weapons = new List<string>();
        Complement = Passengers = Speed = Acceleration = Shields = HullStrength = 0;
        Class = ShipClass.Corvette;
    }
    public ShipModel() {
        InitInstance();
    }
}