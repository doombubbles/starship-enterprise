using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;

namespace StarshipEnterprise.Upgrades.Cannons;

public class DualCannons : ModUpgrade<StarshipEnterprise>
{
    public override int Path => BOTTOM;
    public override int Tier => 2;
    public override int Cost => 500;

    public override string Description => "Phase Cannons fire 2 at a time.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        var emission = towerModel.GetAttackModel("PhaseCannon").GetDescendant<ParallelEmissionModel>();
        emission.count = 2;
        emission.spreadLength = 10;
        emission.UpdateOffset();
    }
}