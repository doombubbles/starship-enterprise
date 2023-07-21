using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Weapons;

namespace StarshipEnterprise.Upgrades.Cannons;

public class PhaseTurret : ModUpgrade<StarshipEnterprise>
{
    public override int Path => BOTTOM;
    public override int Tier => 3;
    public override int Cost => 500;

    public override string Description => "Phase Cannons fire much faster and with a 360 degree attack angle.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        var phaseCannonAttack = towerModel.GetAttackModel("PhaseCannon");
        phaseCannonAttack.GetDescendant<FilterTargetAngleModel>().fieldOfView = 360;
        phaseCannonAttack.GetChild<WeaponModel>().Rate /= 2;
    }
}