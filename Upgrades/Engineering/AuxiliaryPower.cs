using System;
using System.Linq;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons;

namespace StarshipEnterprise.Upgrades.Engineering;

public class AuxiliaryPower : CareerPathUpgrade<Engineering>
{
    public override int Cost => 1000;
    public override int Tier => 2;

    public override string Description => base.Description + "Boost the power of Enterprise's least upgraded weapon.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        var min = towerModel.tiers.Min();

        var factor = 1f / towerModel.tiers.Count(t => t == min);

        if (towerModel.tiers[0] == min)
        {
            Apply(towerModel, "Phaser", factor);
        }

        if (towerModel.tiers[1] == min)
        {
            Apply(towerModel, "Torpedo", factor);
        }

        if (towerModel.tiers[2] == min)
        {
            Apply(towerModel, "Cannon", factor);
        }
    }

    private static void Apply(TowerModel towerModel, string nameContains, float factor)
    {
        towerModel.FindDescendants<WeaponModel>(nameContains).ForEach(weapon =>
        {
            weapon.Rate /= 1 + factor;

            weapon.GetDescendants<ProjectileModel>().ForEach(projectile =>
            {
                projectile.pierce += (int) Math.Ceiling(projectile.pierce * factor);
            });

            weapon.GetDescendants<DamageModel>().ForEach(damage =>
            {
                damage.damage += (int) Math.Ceiling(damage.damage * factor);
            });
        });
    }
}