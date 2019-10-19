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
    public string Character;
    public string Faction;

    private void initInstance() {
        Character = Faction = "";
    }

    public Player() {
        initInstance();
    }

    public Player(string name, string faction) {
        initInstance();
        this.Character = name;
        this.Faction = faction;
    }
}
}