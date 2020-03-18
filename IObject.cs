////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                                 IObject.cs                                 //
//                             IObject Interface                              //
//                  Created by: Jay Mirecki, March 17, 2020                   //
//                  Modified by: Jay Mirecki, March 17, 2020                  //
//                                                                            //
//          The IObject interface is everything that each object              //
//          should implement, such as the ability to verify the               //
//          integrity of objects and keys.                                    //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////
namespace GSWS {

interface IObject {
    string ToString();
    string DatapadDescription();
    void UpdateValues(Database db);
    void UpdateKeys();
    void VerifySubGroups();
    void UpdateSuperGroups();
}}