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
    public bool SimWeek() {
        return DateInt % WeekLength == 0;
    }
    public bool SimMonth() {
        return DateInt % MonthLength == 0;
    }
    public bool SimYear() {
        return DateInt % YearLength == 0;
    }
}
}