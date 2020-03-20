////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                               Coordinate.cs                                //
//                              Coordinate class                              //
//              Created by: Jarett (Jay) Mirecki, July 27, 2019               //
//             Modified by: Jarett (Jay) Mirecki, March 19, 2020              //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace GSWS {

[Serializable] public struct Coordinate {
    public int X;
    public int Y;
    public int Z;

    public Coordinate(int X, int Y, int Z) {
        this.X = X;
        this.Y = Y;
        this.Z = Z;
    }

    public override bool Equals(Object obj) {
        if (!(obj is Coordinate))
            return false;
        Coordinate c = (Coordinate) obj;
        return this.X == c.X && 
               this.Y == c.Y && 
               this.Z == c.Z;
    }
    public override int GetHashCode() {
        return this.X.GetHashCode();
    }
    public static bool operator ==(Coordinate a, Coordinate b) {
        return a.Equals(b);
    }
    public static bool operator !=(Coordinate a, Coordinate b) {
        return !a.Equals(b);
    }

    // public Vector3 AsVector() {
    //     return new Vector3(this.X, this.Y, this.Z);
    // }
    // public Vector3 AsMapVector() {
    //     return new Vector3(this.X * scaleFactor * -1,
    //                        this.Y * scaleFactor,
    //                        this.Z * scaleFactor);
    // }
    public float DistanceTo(Coordinate destination) {
        return (float)Math.Sqrt(Math.Pow(this.X - destination.X, 2f) + 
                                Math.Pow(this.Y - destination.Y, 2f) +
                                Math.Pow(this.Z - destination.Z, 2f));
    }
    public float Angle(Coordinate destination) {
        return 57.2958f * 
               (float)Math.Atan((float)(this.X - destination.X) / 
               (float)(this.Y - destination.Y));
    }
    public float AngleOfElevation(Coordinate destination) {
        return 57.2958f * 
               (float)Math.Asin((float)(this.Z - destination.Z) / 
               this.DistanceTo(destination));
    }
    public float MapAngleOfElevation(Coordinate destination) {
        return this.AngleOfElevation(destination);
    }
    public float MapAngle(Coordinate destination) {
        return this.Angle(destination) - 90f;
    }
    // Move 20 units per hour!!!
    public Coordinate MoveTowards(Coordinate destination, int distance) {
        float relativeDistance = (float)distance / this.DistanceTo(destination);
        if (relativeDistance >= 1f)
            return destination;
        else {
            int x = X + 
                    (int)Math.Round((float)(destination.X - X) * 
                    relativeDistance);
            int y = Y + (int)Math.Round((float)(destination.Y - Y) * relativeDistance);
            int z = Z + (int)Math.Round((float)(destination.Z - Z) * relativeDistance);
            return new Coordinate(x, y, z);
        }
    }
    override public string ToString() {
        return ("(" + X.ToString() + "," + Y.ToString() + "," + Z.ToString() + ")");
    }
}
}