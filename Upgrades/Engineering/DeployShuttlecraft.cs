using BTD_Mod_Helper;
using HarmonyLib;
using Il2CppAssets.Scripts.Models.Towers;
using StarshipEnterprise.Displays.Ships;

namespace StarshipEnterprise.Upgrades.Engineering;

public class DeployShuttlecraft : CareerPathUpgrade<Engineering>
{
    public override int Cost => 800;
    public override int Tier => 1;

    public override string Description =>
        base.Description + "Deploy 2 small Shuttles with slower copies of Enterprise's most upgraded weapon.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        if (tier < 3)
        {
            Engineering.AddShuttles<ShuttleDisplay>(towerModel, 2, 10f);
        }
    }
}