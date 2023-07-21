using System;
using System.Linq;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Simulation.SMath;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Utils;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace StarshipEnterprise.Upgrades.Engineering;

public class Engineering : CareerPath
{
    public override int UpgradeCount => 6;

    protected override int Order => 2;

    public static TowerModel GetBaseShuttle(string name, PrefabReference display, TargetSupplierModel targetProvider)
    {
        var shuttle = Game.instance.model.GetTower(TowerType.MonkeyBuccaneer, 4)
            .GetDescendant<TowerModel>()
            .Duplicate(name);

        shuttle.baseId = name;
        shuttle.towerSet = GetTowerSet<Starfleet>();
        shuttle.isGlobalRange = true;

        shuttle.RemoveBehaviors<AttackModel>();
        shuttle.RemoveBehavior<TowerExpireOnParentUpgradedModel>();

        shuttle.GetBehavior<AirUnitModel>().display = display;

        var fighterMovement = shuttle.GetDescendant<FighterMovementModel>();
        fighterMovement.rollChancePerSecondPassed = 0.00000001f;
        fighterMovement.rollTimeBeforeNext = 1000000000;
        fighterMovement.bankAngleMax = 30;

        shuttle.AddBehavior(new AttackModel("", new Il2CppReferenceArray<WeaponModel>(0), 2000f, new Model[]
        {
            targetProvider
        }, targetProvider, 0, 0, 0, true, false, 0, false, 0));

        shuttle.UpdateTargetProviders();
        
        return shuttle;
    }

    public static void AddShuttles<T>(TowerModel towerModel, int howMany, float factor) where T : ModDisplay
    {
        var attacks = towerModel.GetAttackModels();
        var max = towerModel.tiers.Max();

        if (towerModel.tiers[0] == max)
        {
            attacks = attacks.Where(model => model.name.Contains("Phaser")).ToList();
        }
        else if (towerModel.tiers[1] == max)
        {
            attacks = attacks.Where(model => model.name.Contains("PhotonTorpedo")).ToList();
        }
        else if (towerModel.tiers[2] == max)
        {
            attacks = attacks.Where(model => model.name.Contains("PhaseCannon")).ToList();
        }

        var targetProviders = new TargetSupplierModel[]
        {
            new FighterPilotPatternFirstModel("", false, 40, true),
            new FighterPilotPatternStrongModel("", false, 40, true),
            new FighterPilotPatternLastModel("", false, 40, true),
            new FighterPilotPatternCloseModel("", false, 40, true)
        };

        for (var i = 0; i < howMany; i++)
        {
            var name = "Shuttle" + i;
            var targetProvider = targetProviders[i % targetProviders.Length].Duplicate();

            var shuttle = GetBaseShuttle(name, CreatePrefabReference<T>(), targetProvider);

            foreach (var attack in attacks)
            {
                var newAttack = attack.Duplicate();
                var newWeapon = newAttack.GetChild<WeaponModel>();
                newWeapon.Rate *= factor;
                newWeapon.SetEject(new Vector3(0, 10, 5));
                shuttle.AddBehavior(newAttack);
            }

            shuttle.GetDescendant<FighterMovementModel>().maxSpeed += i;

            towerModel.AddBehavior(new TowerCreateTowerModel(name, shuttle, true));
        }

        towerModel.UpdateTargetProviders();
    }

    [HarmonyPatch(typeof(FighterMovement), nameof(FighterMovement.Process))]
    internal static class FighterMovement_Process
    {
        [HarmonyPrefix]
        private static void Prefix(FighterMovement __instance)
        {
            if (__instance.currentPathSupplier == null)
            {
                __instance.Attatched();
            }
        }
    }
}