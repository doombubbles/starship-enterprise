using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Unity.Display;
using UnityEngine;

namespace StarshipEnterprise.Displays.Weapons;

public class WarpCoreExplosion : ModDisplay
{
    private static readonly int TintColor = Shader.PropertyToID("_TintColor");
    
    public override string BaseDisplay => "6f4aa8eecdb528144b69efee775c64f2"; // TsarBombaExplosion.prefab

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        node.transform.Find("BombAnimContainer/Bomb").gameObject.SetActive(false);
        
        foreach (var particleSystemRenderer in node.GetComponentsInChildren<ParticleSystemRenderer>())
        {
            particleSystemRenderer.material.SetColor(TintColor, Color.cyan);
        }
    }
}