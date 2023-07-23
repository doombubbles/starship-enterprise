using System;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;

namespace StarshipEnterprise.Upgrades.Phasers;

public class RepulsorBeams : ModUpgrade<StarshipEnterprise>
{
    public override int Path => TOP;
    public override int Tier => 2;
    public override int Cost => 1300;
    public override string Icon => Name;

    public override string Description => "Phaser Beams deal more damage, and will push back Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
    }

    public override void LateApplyUpgrade(TowerModel towerModel)
    {
        var knockback = Game.instance.model.GetTower(TowerType.SuperMonkey, 0, 0, Math.Clamp(towerModel.tier, 0, 5))
            .GetDescendant<KnockbackModel>();

        towerModel.FindDescendants<ProjectileModel>("Phaser").ForEach(projectile =>
        {
            projectile.GetDamageModel().damage++;
            projectile.AddBehavior(knockback.Duplicate());
            projectile.UpdateCollisionPassList();
        });
    }
}