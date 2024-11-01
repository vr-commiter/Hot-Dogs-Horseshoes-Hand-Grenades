using FistVR;
using System.Collections.Generic;
using System;
using System.Linq;

namespace H3VR_TrueGear
{
    public class RoundType
    {
        public static string RoundTypeCheck(FireArmRoundType roundType)
        {
            string result = "Shotgun";
            string[] type1 = new string[]
            {
                "a9_19_Parabellum",
                "a50ActionExpress",
                "a763x25mmMauser",
                "a8mmBergmann",
                "a765x25mmBorchardt",
                "a45_ACP",
                "a32ACP",
                "a45_ACP",
                "a50_Imaginary",
                "a357_Magnum",
                "a44_Magnum",
                "a22_LR",
                "a57x28mm",
                "a40_SW",
                "a25_ACP",
                "a9mmSteyr",
                "a9_18_Makarov",
                "a22WinchesterMagnum",
                "a38ACP",
                "a380_ACP",
                "a556_45_Nato",
                "a762_25_Tokarev",
                "a455WebleyAuto",
                "a792x57mmMauser",
                "a600MagnumBolt",
                "a45_70_Govt",
                "a38Special",
                "a454Casull",
                "a38Rimfire",
                "a357_Magnum",
                "a38Tround",
                "a106_25mmR",
                "a762x38mmR",
                "a762x42mm",
                "a22WinchesterMagnum",
                "a44_40Winchester",
                "a45_Colt",
                "a500SW",
                "a44_Magnum",
                "a455Webley",
                "a9mmSteyr",
                "a46x30mm",
                "a9_18_Makarov",
                "aCpbp",
                "a40_SW",
                "a25_ACP",
                "a380_ACP",
                "a762_54_Mosin",
                "a308_Winchester",
                "a41_Short",
                "a41Volcanic"
            };
            string[] type2 = new string[]
            {
                "a12g_Shotgun",
                "a20g_Shotgun",
                "a3gauge",
                "a12gaugeShort",
                "a12GaugeBelted",
                "a40_46_Grenade"
            };
            string[] type3 = new string[]
            {
                "aRPG7Rocket",
                "aM1A1Rocket",
                "aPanzerSchreckRocket",
                "a45_ColtOversize"
            };
            string[] type4 = new string[]
            {
                "a50_BMG",
                "a20x82mm",
                "a13_2mmTuF",
                "a408Cheytac",
                "a50_Remington_BP",
                "a50mmPotato"
            };
            string[] type5 = new string[]
            {
                "a762_51_Nato",
                "a762_54_Mosin",
                "a3006_Springfield",
                "a75x54mmFrench",
                "a762_54_Mosin",
                "a300_Winchester_Magnum",
                "a338Lapua"
            };
            string[] type6 = new string[]
            {
                "a556_45_Nato",
                "a545_39_Soviet",
                "a762_39_Soviet",
                "a280British",
                "a58x42mm",
                "a792x33mmKurz",
                "a762_39_Soviet"
            };
            if (type1.Contains(roundType.ToString()))
            {
                result = "Pistol";
            }
            else if (type2.Contains(roundType.ToString()))
            {
                result = "Shotgun";
            }
            else if (type3.Contains(roundType.ToString()))
            {
                result = "Shotgun";     //Rocket
            }
            else if (type4.Contains(roundType.ToString()))
            {
                result = "Shotgun";     //BigRifle
            }
            else if (type5.Contains(roundType.ToString()))
            {
                result = "Rifle";
            }
            else if (type6.Contains(roundType.ToString()))
            {
                result = "Rifle";
            }
            return result;
        }


    }
}