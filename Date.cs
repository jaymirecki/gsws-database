////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                                   Date.cs                                  //
//                                 Date class                                 //
//              Created by: Jarett (Jay) Mirecki, August 07, 2019             //
//             Modified by: Jarett (Jay) Mirecki, October 09, 2019            //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace GSWS {

public enum DateSystem { ABY }
[Serializable] public class Date {
    private const int WeekLength = 7;
    private const int MonthLength = 28;
    private const int YearLength = 368;
    private string[] era = { "ABY" };
    public int DateInt;
    private DateSystem System;
    public Date() {
        SetDate(0, DateSystem.ABY);
    }
    public Date(int date, DateSystem system) {
        SetDate(date, system);
    }
    public void SetDate(int date, DateSystem system) {
        DateInt = date;
        System = system;
    }
    public override string ToString() {
        int day, year;
        string dateString;
        if (System == DateSystem.ABY) {
            day = DateInt % YearLength + 1;
            year = DateInt / YearLength;
        } else {
            day = 0;
            year = 0;
        }
        dateString = day.ToString() + ":" 
                        + year.ToString() + " " 
                        + era[(int)System];
        return dateString;
    }
    // public void StartTime() {
    //     InvokeRepeating("AdvanceTime", 1f, 1f);
    // }
    // public void StopTime() {
    //     CancelInvoke("AdvanceTime");
    // }
    public void AdvanceTime() {
        AdvanceDay();
        if (IsWeek())
            AdvanceWeek();
        if (IsMonth())
            AdvanceMonth();
        if (IsYear())
            AdvanceYear();
    }
    public void AdvanceDay() {
        DateInt++;

    }
    public void AdvanceWeek() {

    }
    public void AdvanceMonth() {
        // float residentialValue, commercialValue, totalRevenue;
        // foreach (Government aGovernment in new List<Government>(governments.Values)) {
        //     residentialValue = commercialValue = totalRevenue = 0f;
        //     foreach (string planetName in aGovernment.MemberPlanets) {
        //         residentialValue += Game.GetPlanetFromString(planetName).ResidentialValue();
        //         commercialValue += Game.GetPlanetFromString(planetName).IndustrialValue();
        //     }
        //     totalRevenue = 
        //         residentialValue * aGovernment.ResidentialTax + commercialValue * aGovernment.CommercialTax;
        //     totalRevenue = totalRevenue / (368f/12f);
        //     aGovernment.Budget.Balance += 
        //         aGovernment.Budget.GetSurplus() * totalRevenue;
        // }
    }
    public void AdvanceYear() {

    }
    public bool IsWeek() {
        return DateInt % WeekLength == 0;
    }
    public bool IsMonth() {
        return DateInt % MonthLength == 0;
    }
    public bool IsYear() {
        return DateInt % YearLength == 0;
    }
}
}