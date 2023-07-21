using System.Linq;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.ModOptions;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;
using Il2CppAssets.Scripts.Data;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Simulation.SMath;
using Il2CppAssets.Scripts.Unity;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem.IO;
using StarshipEnterprise.Displays;
using StarshipEnterprise.Displays.Ships;
using StarshipEnterprise.Displays.Weapons;

namespace StarshipEnterprise;

public class StarshipEnterprise : ModTower<Starfleet>
{
    public override string BaseTower => TowerType.MonkeyAce + "-004";

    public override int Cost => 1701;

    public override int TopPathUpgrades => 5;

    public override int MiddlePathUpgrades => 5;

    public override int BottomPathUpgrades => 5;

    public override int ShopTowerCount => 1;

    public override string DisplayName => "USS Enterprise";

    public override string Description => "Equipped with Phasers, Photon Torpedoes, and Phase Cannons. " +
                                          "Upgrade weapons, refit the ship, and choose your career path. " +
                                          "Always costs $1,701.";

    public override ModSettingHotkey Hotkey => StarshipEnterpriseMod.EnterpriseHotkey;

    public static Vector3 EjectOffset => new(0, 20, 5);

    public static float Speed => 60f;

    public override bool IsValidCrosspath(int[] tiers) =>
        base.IsValidCrosspath(tiers) || ModHelper.HasMod("UltimateCrosspathing");

    public override void ModifyBaseTowerModel(TowerModel towerModel)
    {
        var airUnitModel = towerModel.GetBehavior<AirUnitModel>().SetName(Name);
        airUnitModel.display = CreatePrefabReference<ConstitutionDisplay>();
        var path = airUnitModel.GetBehavior<PathMovementModel>();
        path.speed = Speed;
        path.takeOffTime = 0;
        path.takeOffAnimTime = 0;
        path.bankRotation = 30;

        var airAttack = towerModel.GetBehavior<AttackAirUnitModel>();

        airAttack.RemoveChildDependants(airAttack.behaviors);
        airAttack.behaviors = new[]
        {
            airAttack.behaviors[3],
            airAttack.behaviors[1],
            airAttack.behaviors[2]
        };
        airAttack.AddChildDependants(airAttack.behaviors);

        foreach (var figureEight in airAttack.GetBehaviors<FigureEightPatternModel>())
        {
            figureEight.useTowerPosition = false;
        }

        var centerPath = airAttack.GetBehavior<CenterElipsePatternModel>();
        centerPath.canSelectPoint = false;
        centerPath.widthRadius = centerPath.heightRadius = 80;

        towerModel.UpdateTargetProviders();

        // towerModel.display = towerModel.GetBehavior<DisplayModel>().display = CreatePrefabReference(""); // TODO

        var dartMonkey = Game.instance.model.GetTowerFromId(TowerType.DartMonkey);

        towerModel.RemoveBehaviors<CreateSoundOnTowerPlaceModel>();
        towerModel.AddBehavior(dartMonkey.GetBehavior<CreateSoundOnTowerPlaceModel>().Duplicate(towerModel.name));

        towerModel.RemoveBehavior(towerModel.GetAttackModel("Spectre"));

        // Phasers

        var adora = Game.instance.model.GetHeroWithNameAndLevel(TowerType.Adora, 10);
        var ballOfLight = adora.GetDescendant<AbilityCreateTowerModel>().towerModel;
        var phaserAttack = ballOfLight.GetAttackModel().Duplicate("Phaser");
        var phaserWeapon = phaserAttack.GetChild<WeaponModel>().SetName("Phaser");
        var phaserProj = phaserWeapon.projectile.SetName("Phaser");
        var phaserDamage = phaserProj.GetDamageModel();
        var phaserBeam = phaserWeapon.GetChild<LineProjectileEmissionModel>();

        phaserAttack.RemoveBehaviors<CirclePatternModel>();
        phaserAttack.targetProvider = new TargetFirstAirUnitModel("", false, false);
        phaserAttack.AddBehavior(phaserAttack.targetProvider);
        phaserAttack.offsetY = 0;

        phaserWeapon.AddBehavior(new FireFromAirUnitModel(""));
        phaserWeapon.SetEject(EjectOffset);
        phaserWeapon.Rate = 1f;

        phaserBeam.dontUseTowerPosition = true;
        phaserBeam.displayPath.SetName("Phaser").assetPath = CreatePrefabReference<PhaserBeam>();
        phaserBeam.effectAtEndModel.assetId = CreatePrefabReference<PhaserParticles>();
        phaserBeam.displayLifetime = .2f;

        phaserProj.RemoveBehaviors<DamageModifierForTagModel>();

        phaserDamage.damage = 1;
        phaserDamage.immuneBloonPropertiesOriginal = phaserDamage.immuneBloonProperties = BloonProperties.Purple;

        towerModel.AddBehavior(phaserAttack);

        // Photon Torpedoes

        var etienne = Game.instance.model.GetHeroWithNameAndLevel(TowerType.Etienne, 10);
        var ucav = etienne.GetDescendant<UCAVModel>().ucavTowerModel;
        var ucavAttack = ucav.GetAttackModel().Duplicate();
        var torpedoAttack = new AttackModel("PhotonTorpedo", ucavAttack.weapons, ucavAttack.range, ucavAttack.behaviors,
            new TargetStrongAirUnitModel("", false, false), 0, 0, 0, true, false, 0, false, 0);
        var torpedoWeapon = torpedoAttack.GetChild<WeaponModel>().SetName("PhotonTorpedo");
        var torpedoProj = torpedoWeapon.projectile.SetName("PhotonTorpedo");
        var torpedoExplosion = torpedoProj.GetDescendant<ProjectileModel>().SetName("PhotonTorpedoExplosion");
        var torpedoDamage = torpedoProj.GetDescendant<DamageModel>();

        torpedoAttack.RemoveBehaviors<TargetSupplierModel>();
        torpedoAttack.AddBehavior(torpedoAttack.targetProvider);

        torpedoWeapon.SetEject(EjectOffset);
        torpedoWeapon.SetEmission(new RandomArcEmissionModel("PhotonTorpedo", 1, 0, 0, 90, 0, new[]
        {
            new EmissionRotationOffBloonDirectionModel("", true, true)
        }));
        torpedoWeapon.Rate = 2;

        torpedoProj.GetBehavior<TrackTargetModel>().TurnRate = 180;
        var explosionEffect = torpedoProj.GetBehavior<CreateEffectOnContactModel>().effectModel;
        explosionEffect.ApplyDisplay<PhotonTorpedoExplosion>();
        explosionEffect.lifespan = .5f;
        torpedoProj.GetBehavior<TravelStraitModel>().Lifespan = 5;
        torpedoProj.GetBehavior<TravelStraitModel>().Speed = 100;
        torpedoProj.SetDisplay("187bc7112ccbf6445afc2ef9173b4568"); // PlasmaBlastAntiBloon

        torpedoDamage.damage = 2;
        torpedoDamage.immuneBloonPropertiesOriginal = torpedoDamage.immuneBloonProperties = BloonProperties.Purple;

        towerModel.AddBehavior(torpedoAttack);

        // Phase Cannons

        var carrier = Game.instance.model.GetTower(TowerType.MonkeyBuccaneer, 4);
        var plane = carrier.GetDescendant<CreateTowerModel>().tower;
        var phaseCannonAttack = plane.GetAttackModel().Duplicate("PhaseCannon");
        var phaseCannonWeapon = phaseCannonAttack.GetChild<WeaponModel>().SetName("PhaseCannon");
        var phaseCannonProj = phaseCannonWeapon.projectile.SetName("PhaseCannon");
        var phaseCannonDamage = phaseCannonProj.GetDamageModel();

        phaseCannonAttack.GetDescendant<FilterTargetAngleModel>().fieldOfView = 90;
        phaseCannonAttack.RemoveBehaviors<TargetSupplierModel>();
        phaseCannonAttack.targetProvider = new TargetFirstAirUnitModel("", false, false);
        phaseCannonAttack.AddBehavior(phaseCannonAttack.targetProvider);

        phaseCannonWeapon.SetEject(EjectOffset);
        phaseCannonWeapon.Rate = .5f;

        phaseCannonWeapon.SetEmission(new ParallelEmissionModel("", 1, 0, 0, false, new[]
        {
            new EmissionRotationOffBloonDirectionModel("", true, false)
        }));
        phaseCannonWeapon.RemoveBehaviors<ThrowMarkerOffsetModel>();

        phaseCannonProj.pierce = 5;
        phaseCannonProj.scale = .5f;
        phaseCannonProj.SetDisplay("f54fbbae11116a04dafbcde24ff646d8"); // ETNUcavMIssile

        phaseCannonDamage.immuneBloonPropertiesOriginal =
            phaseCannonDamage.immuneBloonProperties = BloonProperties.Purple;

        towerModel.AddBehavior(phaseCannonAttack);

        towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = true);
    }
}