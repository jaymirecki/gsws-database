////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                                   Date.cs                                  //
//                                 Date class                                 //
//              Created by: Jarett (Jay) Mirecki, August 07, 2019             //
//             Modified by: Jarett (Jay) Mirecki, October 19, 2019            //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace GSWS {

public enum DateSystem { ABY }
[Serializable] public class Date {
    private const int DayLength = 24;
    private const int WeekLength = 5 * DayLength;
    private const int MonthLength = 7 * WeekLength;
    private const int YearLength = 10 * MonthLength + 
                                   3 * WeekLength + 
                                   3 * DayLength;
    private string[] era = { "ABY" };
    public long DateInt;
    public DateSystem System;
    public Date() {
        SetDate(0, DateSystem.ABY);
    }
    public Date(long date, DateSystem system) {
        SetDate(date, system);
    }
    public void SetDate(long date, DateSystem system) {
        DateInt = date;
        System = system;
    }
    public override string ToString() {
        int day, year;
        string dateString;
        if (System == DateSystem.ABY) {
            day = (int)((DateInt % (long)YearLength) / (long)DayLength + 1);
            year = (int)(DateInt / (long)YearLength);
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