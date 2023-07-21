using Il2CppAssets.Scripts.Unity.Display;
using UnityEngine;

namespace StarshipEnterprise.Displays.Weapons;

public class TransphasicTorpedoExplosion : PhotonTorpedoExplosion
{
    private static readonly int TintColor = Shader.PropertyToID("_TintColor");
    public override float Scale => .3f;

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        base.ModifyDisplayNode(node);
        foreach (var renderer in node.genericRenderers)
        {
            renderer.material.SetColor(TintColor, Color.yellow);
        }
    }
}