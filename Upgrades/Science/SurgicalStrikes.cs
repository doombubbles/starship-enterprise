using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem.Collections.Generic;

namespace StarshipEnterprise.Upgrades.Science;

public class SurgicalStrikes : CareerPathUpgrade<Science>
{
    public override int Cost => 7000;
    public override int Tier => 3;

    public override string Description =>
        base.Description + "Enterprise's attacks remove Regrow, Camo and Fortified properties from Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetDescendants<ProjectileModel>().ForEach(projectile =>
        {
            if (projectile.HasBehavior<DamageModel>())
            {
                projectile.AddBehavior(RemoveBloonModifiersModel.Create(new()
                {
                    cleanseRegen = true,
                    cleanseCamo = true,
                    cleanseFortified = true,
                    cleanseOnlyIfDamaged = true,
                    bloonTagExcludeList = new Il2CppStringArray(0),
                    bloonTagExplicitList = new Il2CppStringArray(0)
                }));
                projectile.UpdateCollisionPassList();
            }
        });
    }
}