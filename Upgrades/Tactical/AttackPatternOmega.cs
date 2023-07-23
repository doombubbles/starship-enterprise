using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace StarshipEnterprise.Upgrades.Tactical;

public class AttackPatternOmega : CareerPathUpgrade<Tactical>
{
    public override int Cost => 240000;
    public override int Tier => 6;

    public override string Description => base.Description + "Enterprise's attacks do bonus damage based on maximum HP of Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetDescendants<ProjectileModel>().ForEach(projectile =>
        {
            if (projectile.HasBehavior<DamageModel>())
            {
                projectile.AddBehavior(new DamagePercentOfMaxModel("", .005f, new Il2CppStringArray(0), false));
            }
        });
    }
}