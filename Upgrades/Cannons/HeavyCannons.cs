using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;

namespace StarshipEnterprise.Upgrades.Cannons;

public class HeavyCannons : ModUpgrade<StarshipEnterprise>
{
    public override int Path => BOTTOM;
    public override int Tier => 5;
    public override int Cost => 50000;

    public override string Description => "Phase Cannon shots are bigger, deal more damage, and have more pierce.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        var projectile = towerModel.FindDescendant<ProjectileModel>("PhaseCannon");

        projectile.GetDamageModel().damage *= 4;
        projectile.pierce *= 5;
        projectile.radius *= 2;
        projectile.scale *= 2;
    }
}