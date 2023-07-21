using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Unity.Display;

namespace StarshipEnterprise.Displays.Weapons;

public class TransphasicTorpedo : ModDisplay
{
    public override string BaseDisplay => "187bc7112ccbf6445afc2ef9173b4568"; // PlasmaBlastAntiBloon

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        Set2DTexture(node, Name);
    }
}