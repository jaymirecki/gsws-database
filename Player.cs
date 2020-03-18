////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                                  Player.cs                                 //
//                                 Player class                               //
//              Created by: Jarett (Jay) Mirecki, July 27, 2019               //
//            Modified by: Jarett (Jay) Mirecki, October 09, 2019             //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

using System;

namespace GSWS {

public class Player {
    public string kCharacter, kFaction;
    // public Character Character;
    public Government Faction;

    public Player() {
        kCharacter = kFaction = "";
        // Character = new Character();
        Faction = new Government();
    }
    public override string ToString() {
        return "{" +  Faction.ID + "}";
    }
    public void UpdateValues(Database db) {
        // if (kCharacter != "")
        //     Character = db.Characters[kCharacter];
        if (kFaction != "")
            Faction = db.Governments[kFaction];

    }
    public void UpdateKeys() {
        // if (Character != null)
        //     kCharacter = Character.ID;
        // else
        //     kCharacter = "";
        if (Faction != null)
            kFaction = Faction.ID;
        else
            kFaction = "";
    }
}
}