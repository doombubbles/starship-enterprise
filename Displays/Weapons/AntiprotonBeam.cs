using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Unity.Display;
using UnityEngine;

namespace StarshipEnterprise.Displays.Weapons;

public class AntiprotonBeam : ModDisplay
{
    private static readonly int Color1 = Shader.PropertyToID("_Color1");
    private static readonly int Color1Power = Shader.PropertyToID("_Color1Power");
    private static readonly int Color2 = Shader.PropertyToID("_Color2");
    private static readonly int BeamPower = Shader.PropertyToID("_BeamPower");

    public override string BaseDisplay => "b740dac7b6bc850438df2a7a10b01bfb"; // BeamLvl10

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        var material = node.GetMeshRenderer().material;

        material.SetColor(Color1, new Color(0, 0, 0));
        material.SetFloat(Color1Power, 1f);
        material.SetColor(Color2, new Color(1, 0, 0));
        material.SetFloat(BeamPower, 3.3f);
    }
}