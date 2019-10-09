////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                               Coordinate.cs                                //
//                              Coordinate class                              //
//              Created by: Jarett (Jay) Mirecki, July 27, 2019               //
//             Modified by: Jarett (Jay) Mirecki, August 01, 2019             //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

[Serializable] public class Coordinate {
    public int X;
    public int Y;
    public int Z;
    static private float scaleFactor = 0.2f;

    public Coordinate(){}
    public Coordinate(int X, int Y, int Z) {
        this.X = X;
        this.Y = Y;
        this.Z = Z;
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
        return (float)Math.Sqrt(Math.Pow(this.X - destination.X, 2f) + Math.Pow(this.Y - destination.Y, 2f));
    }
    public float MapDistanceTo(Coordinate destination) {
        return this.DistanceTo(destination) * scaleFactor;
    }
    public float Angle(Coordinate destination) {
        return 57.2958f * (float)Math.Atan((float)(this.X - destination.X) / (float)(this.Y - destination.Y));
    }
    public float AngleOfElevation(Coordinate destination) {
        return 57.2958f * (float)Math.Asin((float)(this.Z - destination.Z) / this.DistanceTo(destination));
    }
    public float MapAngleOfElevation(Coordinate destination) {
        return this.AngleOfElevation(destination) + 90f;
    }
    public float MapAngle(Coordinate destination) {
        return this.Angle(destination) - 90f;
    }
    override public string ToString() {
        return ("(" + X.ToString() + "," + Y.ToString() + "," + Z.ToString() + ")");
    }
}