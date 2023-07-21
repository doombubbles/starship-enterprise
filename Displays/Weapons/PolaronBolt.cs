using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Unity.Display;
using UnityEngine;

namespace StarshipEnterprise.Displays.Weapons;

public class PolaronBolt : ModDisplay
{
    public override string BaseDisplay => "f54fbbae11116a04dafbcde24ff646d8"; // ETNUcavMIssile
    
    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        Set2DTexture(node, Name);
        node.GetRenderer<TrailRenderer>().startColor = new Color(0.63f, 0f, 1f, 1);
        node.GetRenderer<TrailRenderer>().endColor = new Color(0.63f, 0f, 1f, 0);
    }

}