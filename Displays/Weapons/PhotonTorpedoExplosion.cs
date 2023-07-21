using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Unity.Display;

namespace StarshipEnterprise.Displays.Weapons;

public class PhotonTorpedoExplosion : ModDisplay
{
    public override string BaseDisplay => "21f659bbb9e1d9441adf3239a773e224"; // AntiBloonAnnihilationFX

    public override float Scale => .2f;

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        node.transform.GetChild(2).gameObject.SetActive(false);
    }
}