////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                               Government.cs                                //
//                              Government class                              //
//             Created by: Jarett (Jay) Mirecki, August 08, 2019              //
//             Modified by: Jarett (Jay) Mirecki, August 08, 2019             //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

[Serializable] public class Government {
    [XmlAttribute] public string ID;
    public string Name;
    public float ExecutivePower, LegislativePower, JudicialPower, ResidentialTax, CommercialTax;
    public List<string> MemberPlanets;
    public Budget Budget;

    private void InitInstance() {
        ID = Name = "null";
        ExecutivePower = LegislativePower = JudicialPower = ResidentialTax = CommercialTax = 0f;
        MemberPlanets = new List<string>();
        Budget = new Budget();
    }
    public Government() {
        InitInstance();
    }
    public Government(string Name) {
        InitInstance();
        this.Name = Name;
        ID = Name.ToLower().Replace("'", "").Replace(' ', '_');
    }
}