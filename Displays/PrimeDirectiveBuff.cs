using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Unity.Display;
using UnityEngine;

namespace StarshipEnterprise.Displays;

public class PrimeDirectiveBuff : ModDisplay
{
    private static readonly int TintColor = Shader.PropertyToID("_TintColor");
    
    public override string BaseDisplay => "900108af532d9bb41a0b48e891789fa1"; // OverclockBuff.prefab

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        foreach (var particleSystemRenderer in node.GetComponentsInChildren<ParticleSystemRenderer>())
        {
            particleSystemRenderer.material.SetColor(TintColor, new Color(0, .5f, 1, 1));
        }
    }
}