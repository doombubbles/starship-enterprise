using System.Linq;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Unity.Display;
using UnityEngine;

namespace StarshipEnterprise.Displays.Weapons;

public class PhaserBlast : ModDisplay
{
    public override string BaseDisplay => "95e1b845816b6f748af84449fe6b7a59"; // SunTempleBlast

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        node.GetRenderers<SpriteRenderer>().ForEach(renderer =>
        {
            if (renderer.sprite != null)
            {
                renderer.sprite = GetSprite(Name);
            }
        });
    }
}