////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                                  Body.cs                                   //
//                                 Body class                                 //
//              Created by: Jarett (Jay) Mirecki, March 10, 2020              //
//             Modified by: Jarett (Jay) Mirecki, March 10, 2020              //
//                                                                            //
//          The Body class stores information related to the                  //
//          physical planetary bodies in the galaxy.                          //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace GSWS {

public enum Atmosphere { Breathable, Dangerous, Toxic, None };
public enum Class { Terrestrial, Gas, Barren, Ocean };
public enum TerrainType { Varied, Ecumenopolis };
public enum Region { DeepCore, Core, Colonies, InnerRim, MidRim, OuterRim, ExpansionRegion, UnknownRegions };

[Serializable] public class Body {
    #region Properties
    [XmlAttribute] public string ID;
    public string Name, kSystem, kSector;
    public Atmosphere Atmosphere;
    public Coordinate Position;
    public Class Class;
    public float Gravity, Surface;
    public TerrainType Terrain;
    public Region Region;
    public int DayLength, YearLength, Diameter;
    #endregion
    #region Constructing
    public Body() {
        Name = kSystem = kSector = "";
        Atmosphere = Atmosphere.Breathable;
        Class = Class.Terrestrial;
        Gravity = Surface = 1;
        Terrain = TerrainType.Varied;
        DayLength = YearLength = Diameter = 1;
        Position = new Coordinate();
        Region = Region.Core;
    }
    #endregion
}
}