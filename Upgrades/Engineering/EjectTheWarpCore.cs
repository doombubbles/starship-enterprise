using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using StarshipEnterprise.Displays.Weapons;

namespace StarshipEnterprise.Upgrades.Engineering;

public class EjectTheWarpCore : CareerPathUpgrade<Engineering>
{
    public override int Cost => 30000;
    public override int Tier => 4;

    public override bool Ability => true;

    public override string Description =>
        base.Description +
        "Ability: Eject the Warp Core, creating an Explosion that deals immense damage to all Bloons on Screen.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        var ability = Game.instance.model.GetTower(TowerType.MonkeyAce, 0, 5, 0).GetAbility().Duplicate(Name);

        ability.name = Name;
        ability.displayName = DisplayName;
        ability.description = Description;
        ability.icon = IconReference;

        ability.resetCooldownOnTierUpgrade = false;

        ability.GetDescendant<CreateEffectOnAbilityModel>().effectModel.ApplyDisplay<WarpCoreExplosion>();
        ability.RemoveBehavior<GroundZeroBombBuffModel>();

        var projectileModel = ability.GetDescendant<ProjectileModel>();

        projectileModel.pierce = 9999999;
        projectileModel.RemoveBehavior<SlowModel>();

        towerModel.AddBehavior(ability);
    }
}