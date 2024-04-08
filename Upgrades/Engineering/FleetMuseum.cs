using System.Linq;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Simulation.SMath;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using StarshipEnterprise.Displays.Ships;
using StarshipEnterprise.Upgrades.Refit;

namespace StarshipEnterprise.Upgrades.Engineering;

public class FleetMuseum : CareerPathUpgrade<Engineering>
{
    public override int Cost => 300000;
    public override int Tier => 6;

    public override string Description =>
        base.Description + "Instead of deploying Shuttles, calls in all previous Enterprises from retirement.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.FindDescendant<AbilityModel>(nameof(EjectTheWarpCore))
            .GetDescendant<ProjectileModel>()
            .AddBehavior(new DamagePercentOfMaxModel("", .5f, new Il2CppStringArray(0), false));

        var refits = GetContent<RefitUpgrade>()
            .Where(upgrade => towerModel.appliedUpgrades.Contains(upgrade.Id))
            .OrderByDescending(upgrade => upgrade.Tier)
            .ToArray();

        if (!refits.Any()) return;

        var buffReductionFactor = 1 + refits.First().BuffFactor;

        foreach (var refit in refits.Skip(1))
        {
            AddShuttle(towerModel, refit.Display, buffReductionFactor, refit.EjectOffset, refit.Speed);

            buffReductionFactor *= 1 + refit.BuffFactor;
        }

        AddShuttle(towerModel, CreatePrefabReference<ConstitutionDisplay>(), buffReductionFactor,
            StarshipEnterprise.EjectOffset, StarshipEnterprise.Speed);
    }

    private static void AddShuttle(TowerModel towerModel, PrefabReference display, float buffReductionFactor,
        Vector3 eject, float speed)
    {
        var name = display.guidRef.Replace("StarshipEnterprise-", "").Replace("Display", "");

        var shuttle = Engineering.GetBaseShuttle(name, display,
            new FighterPilotPatternFirstModel("", false, 40, true));

        foreach (var attackModel in towerModel.GetAttackModels().Skip(1))
        {
            shuttle.AddBehavior(attackModel.Duplicate());
        }

        shuttle.GetBehavior<AirUnitModel>().display = display;
        shuttle.GetDescendant<FighterMovementModel>().maxSpeed = speed;
        shuttle.GetDescendant<FighterMovementModel>().loopRadius += buffReductionFactor;

        shuttle.GetDescendants<WeaponModel>().ForEach(weapon =>
        {
            weapon.SetEject(eject, ignoreX: true);
            weapon.Rate *= buffReductionFactor;
        });

        towerModel.AddBehavior(new TowerCreateTowerModel(name, shuttle, true));
    }
}