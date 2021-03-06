////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                                 Budget.cs                                  //
//                                Budget class                                //
//                  Created by: Jay Mirecki, August 08, 2019                  //
//                  Modified by: Jay Mirecki, March 17, 2020                  //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace GSWS {

[Serializable] public class Budget {
    public float Military, PublicSafety, Health, Education, Balance;
    public Budget() {
        Military = PublicSafety = Health = Education = Balance = 0;
    }
    public void SetMilitary(float value) {
        if (GetSurplus() >= (value - Military))
            Military = value;
        else
            Military = GetSurplus() + Military;
    }
    public void SetPublicSafety(float value) {
        if (GetSurplus() >= (value - PublicSafety))
            PublicSafety = value;
        else
            PublicSafety = GetSurplus() + PublicSafety;
    }
    public void SetHealth(float value) {
        if (GetSurplus() >= (value - Health))
            Health = value;
        else
            Health = GetSurplus() + Health;
    }
    public void SetEducation(float value) {
        if (GetSurplus() >= (value - Education))
            Education = value;
        else
            Education = GetSurplus() + Education;
    }
    public float GetSurplus() {
        return 1 - (Military + PublicSafety + Health + Education);
    }
}
}