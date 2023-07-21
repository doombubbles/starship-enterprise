using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Unity.Display;
using UnityEngine;

namespace StarshipEnterprise.Displays.Ships;

public class ConstitutionDisplay : ModCustomDisplay
{
    public override string AssetBundleName => "assets";

    public override string PrefabName => "ConstitutionClass";

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        var renderer = node.GetMeshRenderer();
        renderer.ApplyOutlineShader();
        renderer.SetOutlineColor(new Color(.25f, .25f, .25f, 1));
    }
}