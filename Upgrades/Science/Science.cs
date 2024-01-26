using System;
using System.Collections.Generic;
using System.Linq;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors;
using Il2CppSystem.Linq;
using StarshipEnterprise.Upgrades.Cannons;
using StarshipEnterprise.Upgrades.Phasers;
using StarshipEnterprise.Upgrades.Torpedoes;

namespace StarshipEnterprise.Upgrades.Science;

public class Science : CareerPath
{
    public override int UpgradeCount => 6;

    protected override int Order => 3;

    public static bool PrimeDirectiveTech(TowerModel towerModel, int tier)
    {
        if (!PrimeDirectiveModes.TryGetValue(towerModel.baseId, out var mode)) return false;

        var towerToAddTo = mode switch
        {
            PrimeDirectiveMode.PhaserDrones => towerModel.GetBehavior<DroneSupportModel>().droneModel,
            _ => towerModel
        };

        AttackModel attackModel;

        switch (mode)
        {
            case PrimeDirectiveMode.Phaser:
            case PrimeDirectiveMode.PhaserDrones:
            default:
                attackModel = GetEnterprise(tier, 0, 0).FindDescendant<AttackModel>("Phaser").Duplicate();
                if (tier >= 3)
                {
                    attackModel.GetChild<WeaponModel>().Rate /= 3;
                }

                break;
            case PrimeDirectiveMode.PhotonTorpedo:
            case PrimeDirectiveMode.PhotonTorpedoOnSelf:
            case PrimeDirectiveMode.PhotonTorpedoOnPoint:
            case PrimeDirectiveMode.PhotonTorpedoOnTarget:
                attackModel = GetEnterprise(0, tier, 0).FindDescendant<AttackModel>("PhotonTorpedo").Duplicate();

                var arcEmission = attackModel.GetDescendant<RandomArcEmissionModel>();
                arcEmission.RemoveBehavior<EmissionRotationOffBloonDirectionModel>();
                if (!towerToAddTo.HasDescendant<ArcEmissionModel>())
                {
                    attackModel.GetChild<WeaponModel>().SetEmission(new SingleEmissionModel("", null));
                    if (tier >= 3)
                    {
                        attackModel.GetChild<WeaponModel>().Rate /= 3;
                    }
                }
                else
                {
                    arcEmission.randomAngle = 0;
                }

                break;
            case PrimeDirectiveMode.PhaseCannon:
            case PrimeDirectiveMode.PhaseCannonSpread:
                attackModel = GetEnterprise(0, 0, tier).FindDescendant<AttackModel>("PhaseCannon").Duplicate();
                attackModel.RemoveFilter<FilterTargetAngleModel>();
                var emission = attackModel.GetDescendant<ParallelEmissionModel>();
                if (!towerToAddTo.HasBehavior<AirUnitModel>())
                {
                    emission.RemoveBehavior<EmissionRotationOffBloonDirectionModel>();
                }
                if (!towerToAddTo.HasBehavior<AirUnitModel>())
                {
                    emission.spreadLength /= 2;
                    emission.UpdateOffset();
                }

                break;
        }
        
        attackModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
        attackModel.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
        

        if (towerToAddTo.GetDescendants<TargetSupplierModel>().Any(model =>
                model.GetName() == "Close" || model.Is<TargetSelectedPointModel>()))
        {
            attackModel.RemoveBehavior<TargetSupplierModel>();
            attackModel.targetProvider = null;
        }

        var weaponModel = attackModel.GetChild<WeaponModel>();

        if (!towerToAddTo.isGlobalRange && !towerModel.HasBehavior<AirUnitModel>())
        {
            attackModel.range = towerModel.range;
        }

        var camo = towerModel.GetDescendant<FilterInvisibleModel>()?.isActive ?? false;
        attackModel.GetDescendants<FilterInvisibleModel>().ForEach(camoModel => camoModel.isActive = camo);


        var eject = towerToAddTo.GetDescendant<WeaponModel>().GetEject();
        if (towerToAddTo.baseId == TowerType.AdmiralBrickell)
        {
            eject = towerToAddTo.FindDescendants<WeaponModel>().Skip(1).First().GetEject();
        }
        eject.y -= 1;

        foreach (var weapon in attackModel.weapons)
        {
            if (!towerToAddTo.HasBehavior<AirUnitModel>())
            {
                weapon.RemoveBehavior<FireFromAirUnitModel>();
            }

            weapon.SetEject(eject);
        }

        if (towerToAddTo.GetAttackModel().HasBehavior(out RotateToTargetModel rotate))
        {
            attackModel.AddBehavior(towerToAddTo.GetDescendant<RotateToTargetModel>().Duplicate());
            if (rotate.additionalRotation != 0 && attackModel.HasDescendant(out EmissionModel emissionModel))
            {
                if (emissionModel.Is<ArcEmissionModel>())
                {
                    emissionModel
                        .AddBehavior(new EmissionArcRotationOffTowerDirectionModel("", -rotate.additionalRotation));
                }
                else
                {
                    emissionModel
                        .AddBehavior(new EmissionRotationOffTowerDirectionModel("", -rotate.additionalRotation));
                }
            }
        }

        switch (mode)
        {
            case PrimeDirectiveMode.PhotonTorpedoOnSelf:
                var explosion = weaponModel.projectile.GetDescendant<ProjectileModel>();
                var effect = weaponModel.GetDescendant<CreateEffectOnContactModel>().effectModel;

                weaponModel.SetProjectile(explosion);
                weaponModel.AddBehavior(new EjectEffectModel("", effect.assetId, effect, effect.lifespan,
                    effect.fullscreen, false, false, false, false));
                break;
            case PrimeDirectiveMode.PhotonTorpedoOnPoint:
                weaponModel.SetEmission(towerToAddTo.GetDescendant<RandomTargetSpreadModel>().Duplicate());
                weaponModel.projectile.AddBehavior(new InstantModel("", true));
                weaponModel.projectile.AddBehavior(new AgeModel("", .1f, 0, false, null));
                weaponModel.projectile.RemoveBehavior<TravelStraitModel>();
                weaponModel.projectile.RemoveBehavior<TrackTargetModel>();
                towerToAddTo.GetAttackModel().AddWeapon(weaponModel);
                return true;
            case PrimeDirectiveMode.PhotonTorpedoOnTarget:
                attackModel.GetChild<WeaponModel>().SetEmission(new InstantDamageEmissionModel("", null));
                weaponModel.projectile.AddBehavior(new InstantModel("", true));
                weaponModel.projectile.RemoveBehavior<TravelStraitModel>();
                break;
            case PrimeDirectiveMode.PhaseCannonSpread:
                var count = weaponModel.GetChild<ParallelEmissionModel>().count;

                if (towerToAddTo.GetAttackModel().HasDescendant(out EmissionWithOffsetsModel emissionWithOffsets))
                {
                    var newEmission = emissionWithOffsets.Duplicate();
                    newEmission.projectileCount = count;
                    weaponModel.SetEmission(newEmission);
                }

                if (towerToAddTo.GetAttackModel().HasDescendant(out ArcEmissionModel arcEmission))
                {
                    var newEmission = arcEmission.Duplicate();
                    count = weaponModel.GetChild<ParallelEmissionModel>().count;
                    if (newEmission.angle >= 360) count *= 4;
                    newEmission.count = count;
                    weaponModel.SetEmission(newEmission);
                    weaponModel.GetDescendant<TravelStraitModel>().Lifespan =
                        towerToAddTo.GetDescendant<TravelStraitModel>().Lifespan;
                }

                if (towerToAddTo.GetAttackModel().HasDescendant(out RandomEmissionModel randomEmission))
                {
                    var newEmission = randomEmission.Duplicate();
                    newEmission.count = count;
                    weaponModel.SetEmission(newEmission);
                    weaponModel.GetDescendant<TravelStraitModel>().Lifespan =
                        towerToAddTo.GetDescendant<TravelStraitModel>().Lifespan;
                }

                if (towerToAddTo.baseId == TowerType.DartlingGunner)
                {
                    attackModel.fireWithoutTarget = true;
                }

                break;
        }

        towerToAddTo.AddBehavior(attackModel);
        return true;
    }

    private static TowerModel GetEnterprise(int t1, int t2, int t3)
    {
        var enterprise =
            GetTowerModel<StarshipEnterprise>(Math.Clamp(t1, 0, 5), Math.Clamp(t2, 0, 5), Math.Clamp(t3, 0, 5));

        if (t1 >= 6 || t2 >= 6 || t3 >= 6)
        {
            enterprise = enterprise.Duplicate();
        }

        foreach (var u in GetInstance<PhaserPath>().Upgrades.Values.Where(u => u.Tier <= t1))
        {
            u!.ApplyUpgrade(enterprise);
            u.ApplyUpgrade(enterprise, t1);
        }

        foreach (var u in GetInstance<PhotonTorpedoPath>().Upgrades.Values.Where(u => u.Tier <= t2))
        {
            u!.ApplyUpgrade(enterprise);
            u.ApplyUpgrade(enterprise, t2);
        }

        foreach (var u in GetInstance<PhaseCannonPath>().Upgrades.Values.Where(u => u.Tier <= t3))
        {
            u!.ApplyUpgrade(enterprise);
            u.ApplyUpgrade(enterprise, t3);
        }

        return enterprise;
    }

    public static readonly Dictionary<string, PrimeDirectiveMode> PrimeDirectiveModes = new()
    {
        { TowerType.DartMonkey, PrimeDirectiveMode.PhaseCannonSpread },
        { TowerType.BoomerangMonkey, PrimeDirectiveMode.Phaser },
        { TowerType.TackShooter, PrimeDirectiveMode.PhaseCannonSpread },
        { TowerType.BombShooter, PrimeDirectiveMode.PhotonTorpedo },
        { TowerType.GlueGunner, PrimeDirectiveMode.PhaseCannon },
        { TowerType.IceMonkey, PrimeDirectiveMode.PhotonTorpedoOnSelf },

        { TowerType.SniperMonkey, PrimeDirectiveMode.Phaser },
        { TowerType.DartlingGunner, PrimeDirectiveMode.PhaseCannonSpread },
        { TowerType.MonkeySub, PrimeDirectiveMode.Phaser },
        { TowerType.MonkeyBuccaneer, PrimeDirectiveMode.PhotonTorpedo },
        { TowerType.MortarMonkey, PrimeDirectiveMode.PhotonTorpedoOnPoint },
        { TowerType.MonkeyAce, PrimeDirectiveMode.Phaser },
        { TowerType.HeliPilot, PrimeDirectiveMode.PhaseCannon },

        { TowerType.NinjaMonkey, PrimeDirectiveMode.PhaseCannonSpread },
        { TowerType.WizardMonkey, PrimeDirectiveMode.Phaser },
        { TowerType.SuperMonkey, PrimeDirectiveMode.PhaseCannonSpread },
        { TowerType.Druid, PrimeDirectiveMode.PhaseCannonSpread },
        { TowerType.Alchemist, PrimeDirectiveMode.PhotonTorpedo },

        { TowerType.EngineerMonkey, PrimeDirectiveMode.Phaser },
        { TowerType.BeastHandler, PrimeDirectiveMode.Phaser },

        { TowerType.Quincy, PrimeDirectiveMode.Phaser },
        { TowerType.Gwendolin, PrimeDirectiveMode.PhaseCannon },
        { TowerType.StrikerJones, PrimeDirectiveMode.PhotonTorpedo },
        { TowerType.ObynGreenfoot, PrimeDirectiveMode.PhotonTorpedo },
        { TowerType.CaptainChurchill, PrimeDirectiveMode.PhaseCannon },
        { TowerType.Ezili, PrimeDirectiveMode.Phaser },
        { TowerType.PatFusty, PrimeDirectiveMode.PhotonTorpedoOnTarget },
        { TowerType.Adora, PrimeDirectiveMode.Phaser },
        { TowerType.AdmiralBrickell, PrimeDirectiveMode.Phaser },
        { TowerType.Etienne, PrimeDirectiveMode.PhaserDrones },
        { TowerType.Sauda, PrimeDirectiveMode.PhotonTorpedoOnTarget },
        { TowerType.Psi, PrimeDirectiveMode.Phaser },
        { TowerType.Geraldo, PrimeDirectiveMode.Phaser },
        
        
        { TowerType.SunAvatarMini, PrimeDirectiveMode.PhaseCannonSpread },
        { TowerType.TrueSunAvatarMini, PrimeDirectiveMode.PhaseCannonSpread },
        { TowerType.Sentry, PrimeDirectiveMode.Phaser },
        { TowerType.SentryBoom, PrimeDirectiveMode.PhotonTorpedo },
        { TowerType.SentryCold, PrimeDirectiveMode.Phaser },
        { TowerType.SentryEnergy, PrimeDirectiveMode.PhaseCannon },
        { TowerType.SentryCrushing, PrimeDirectiveMode.PhaseCannon },
        { TowerType.SentryParagon, PrimeDirectiveMode.PhaseCannon },
    };

    public enum PrimeDirectiveMode
    {
        Phaser,
        PhaserDrones,
        PhotonTorpedo,
        PhotonTorpedoOnSelf,
        PhotonTorpedoOnPoint,
        PhotonTorpedoOnTarget,
        PhaseCannon,
        PhaseCannonSpread,
    }

    /// <summary>
    /// Avoid adding it every frame while placing
    /// </summary>
    [HarmonyPatch(typeof(Support), nameof(Support.IsInZone))]
    internal static class Support_IsInZone
    {
        [HarmonyPrefix]
        private static bool Prefix(Support __instance)
        {
            return !__instance.model.name.Contains("PrimeDirective");
        }
    }
}