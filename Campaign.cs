////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                                Campaign.cs                                 //
//                               Campaign class                               //
//             Created by: Jarett (Jay) Mirecki, August 09, 2019              //
//            Modified by: Jarett (Jay) Mirecki, October 09, 2019             //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace GSWS {
[Serializable] public class Campaign {
    [XmlAttribute] public string ID;
    public string Name;
    public List<string> FactionNames;
    public List<string> FactionIDs;
    public Date Date;

    public Campaign() {

    }
}
}