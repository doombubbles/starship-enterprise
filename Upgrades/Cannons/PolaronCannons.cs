using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using PathsPlusPlus;
using StarshipEnterprise.Displays.Weapons;

namespace StarshipEnterprise.Upgrades.Cannons;

public class PolaronCannons : UpgradePlusPlus<PhaseCannonPath>
{
    public override int Cost => 160000;
    public override int Tier => 6;
    public override string Icon => Name;

    public override string Description => "Upgrade to Polaron Cannons, dealing massive damage with unlimited pierce.";

    public override string Container => UpgradeContainerPlatinum;

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.FindDescendants<ProjectileModel>("PhaseCannon").ForEach(projectile =>
        {
            projectile.ApplyDisplay<PolaronBolt>();
            projectile.ignorePierceExhaustion = true;
            if (projectile.HasBehavior(out DamageModel damageModel))
            {
                damageModel.damage *= 6;
            }
        });
    }
}