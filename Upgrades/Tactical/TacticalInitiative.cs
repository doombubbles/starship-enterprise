using System;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;

namespace StarshipEnterprise.Upgrades.Tactical;

public class TacticalInitiative : CareerPathUpgrade<Tactical>
{
    public override int Cost => 1700;
    public override int Tier => 2;

    public override string Description => base.Description + "Increase the pierce of all of Enterprise's weapons.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetDescendants<ProjectileModel>().ForEach(proj =>
        {
            if (proj.HasBehavior<DamageModel>())
            {
                proj.pierce += (int) Math.Ceiling(proj.pierce * .2);
            }
        });
    }
}