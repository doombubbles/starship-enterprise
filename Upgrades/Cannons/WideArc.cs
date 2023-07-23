using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Filters;

namespace StarshipEnterprise.Upgrades.Cannons;

public class WideArc : ModUpgrade<StarshipEnterprise>
{
    public override int Path => BOTTOM;
    public override int Tier => 1;
    public override int Cost => 180;
    public override string Icon => Name;

    public override string Description => "Phase Cannon attack angle increased from 90 degrees to 180.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetAttackModel("PhaseCannon").GetDescendant<FilterTargetAngleModel>().fieldOfView = 180;
    }
}