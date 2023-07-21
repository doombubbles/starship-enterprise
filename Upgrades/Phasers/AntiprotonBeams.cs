using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using PathsPlusPlus;
using StarshipEnterprise.Displays.Weapons;

namespace StarshipEnterprise.Upgrades.Phasers;

public class AntiprotonBeams : UpgradePlusPlus<PhaserPath>
{
    public override int Tier => 6;

    public override int Cost => 100000;

    public override string Description => "Phasers become devastating powerful Antiproton beams, with crits strong enough to one-shot many Bloons";

    public override string Container => UpgradeContainerPlatinum;

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        foreach (var lineEmission in towerModel.FindDescendants<LineProjectileEmissionModel>())
        {
            lineEmission.displayPath.ApplyDisplay<AntiprotonBeam>();
            lineEmission.effectAtEndModel.ApplyDisplay<AntiProtonParticles>();
            lineEmission.projectileInitialHitModel.ApplyDisplay<AntiprotonBlast>();
        }
        
        towerModel.FindDescendants<ProjectileModel>("Phaser").ForEach(projectile =>
        {
            if (projectile.HasBehavior(out DamageModel damageModel))
            {
                damageModel.damage *= 10;
            }
        });
        
        towerModel.GetDescendants<CritMultiplierModel>().ForEach(crit =>
        {
            crit.damage *= 100;
        });
    }
}