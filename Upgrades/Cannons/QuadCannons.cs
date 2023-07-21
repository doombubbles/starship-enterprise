using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;

namespace StarshipEnterprise.Upgrades.Cannons;

public class QuadCannons : ModUpgrade<StarshipEnterprise>
{
    public override int Path => BOTTOM;
    public override int Tier => 4;
    public override int Cost => 14000;

    public override string Description => "Phase Cannons fire 4 at a time.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        var emission = towerModel.GetAttackModel("PhaseCannon").GetDescendant<ParallelEmissionModel>();
        emission.count = 4;
        emission.spreadLength = 20;
        emission.UpdateOffset();
    }
}