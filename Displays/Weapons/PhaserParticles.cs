using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Unity.Display;
using UnityEngine;

namespace StarshipEnterprise.Displays.Weapons;

public class PhaserParticles : ModDisplay
{
    private static readonly int TintColor = Shader.PropertyToID("_TintColor");
    
    public override string BaseDisplay => "3c445c563b7fc3f44aacf5cb2a79e424"; // BeamHitParticlesLvl10

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        foreach (var renderer in node.genericRenderers)
        {
            renderer.material.SetColor(TintColor, new Color(1, .1f, 0));
        }
    }
}