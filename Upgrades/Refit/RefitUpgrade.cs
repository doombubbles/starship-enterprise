using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Simulation.SMath;
using Il2CppAssets.Scripts.Utils;
using PathsPlusPlus;
using Math = System.Math;

namespace StarshipEnterprise.Upgrades.Refit;

public abstract class RefitUpgrade : UpgradePlusPlus<RefitPath>
{
    public override string Description => "Increased pierce and damage for all weapons.";

    public override string Portrait => Name + "-Portrait";

    public abstract float BuffFactor { get; }

    public virtual Vector3 EjectOffset => StarshipEnterprise.EjectOffset;

    public virtual float Speed => StarshipEnterprise.Speed;
    
    public abstract PrefabReference Display { get; }
    
    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        if (Tier == tier)
        {
            towerModel.GetBehavior<AirUnitModel>().display = Display;
            towerModel.GetDescendant<PathMovementModel>().speed = Speed;

            towerModel.GetDescendants<WeaponModel>()
                .ForEach(weapon => weapon.SetEject(EjectOffset, ignoreX: true));
            
            towerModel.portrait = PortraitReference;
        }

        towerModel.GetDescendants<ProjectileModel>()
            .ForEach(projectile => projectile.pierce += (int) Math.Ceiling(projectile.pierce * BuffFactor));

        towerModel.GetDescendants<DamageModel>()
            .ForEach(damage => damage.damage += (int) Math.Ceiling(damage.damage * BuffFactor));

        towerModel.GetDescendants<CritMultiplierModel>()
            .ForEach(crit => crit.damage += (int) Math.Ceiling(crit.damage * BuffFactor));
    }
}

public abstract class RefitUpgrade<T> : RefitUpgrade where T : ModDisplay
{
    public override PrefabReference Display => CreatePrefabReference<T>();
}