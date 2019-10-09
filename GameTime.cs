using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using JMSuite.Collections;

public partial class Game : MonoBehaviour
{
    public void StartTime() {
        InvokeRepeating("AdvanceTime", 1f, 1f);
    }
    public void StopTime() {
        CancelInvoke("AdvanceTime");
    }
    public void AdvanceTime() {
        AdvanceDay();
        if (Date.SimWeek())
            AdvanceWeek();
        if (Date.SimMonth())
            AdvanceMonth();
        if (Date.SimYear())
            AdvanceYear();
    }
    public void AdvanceDay() {
        Date.DateInt++;

    }
    public void AdvanceWeek() {

    }
    public void AdvanceMonth() {
        float residentialValue, commercialValue, totalRevenue;
        foreach (Government aGovernment in new List<Government>(Governments.Values)) {
            residentialValue = commercialValue = totalRevenue = 0f;
            foreach (string planetName in aGovernment.MemberPlanets) {
                residentialValue += Game.GetPlanetFromString(planetName).ResidentialValue();
                commercialValue += Game.GetPlanetFromString(planetName).IndustrialValue();
            }
            totalRevenue = 
                residentialValue * aGovernment.ResidentialTax + commercialValue * aGovernment.CommercialTax;
            totalRevenue = totalRevenue / (368f/12f);
            aGovernment.Budget.Balance += 
                aGovernment.Budget.GetSurplus() * totalRevenue;
        }
    }
    public void AdvanceYear() {

    }
}