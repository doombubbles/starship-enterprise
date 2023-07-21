using System.Linq;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Effects;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using StarshipEnterprise.Displays;
using StarshipEnterprise.Displays.Weapons;

namespace StarshipEnterprise.Upgrades.Phasers;

public class BeamOverload : ModUpgrade<StarshipEnterprise>
{
    public override int Path => TOP;

    public override int Tier => 4;

    public override int Cost => 500;

    public override string Description =>
        "Phaser Beams create a burst of energy on contact with Bloons, also damaging other Bloons nearby.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetDescendants<LineProjectileEmissionModel>().ForEach(emission =>
        {
            var filters = new FilterModel[]
            {
                new FilterInvisibleModel("", true, false)
            };
            var proj = new ProjectileModel(CreatePrefabReference(""), "PhaserBlast", 10, 0, 20, 0, new Model[]
            {
                new DisplayModel("PhaserBlast", CreatePrefabReference(""), 0, DisplayCategory.Projectile),
                new AgeModel("PhaserBlast", .2f, 0, false, null),
                new ProjectileFilterModel("PhaserBlast", filters),
                new DamageModel("PhaserBlast", 1, 0, true, false, true, BloonProperties.Purple, BloonProperties.Purple),
                new CreateEffectOnExhaustFractionModel("PhaserBlast", CreatePrefabReference(null),
                    new EffectModel("", CreatePrefabReference<PhaserBlast>(), 1, .2f, Fullscreen.No, false, false,
                        false, false, false, false), .2f, Fullscreen.No, 0, 1, true)
            }, filters);
            proj.UpdateCollisionPassList();
            emission.projectileInitialHitModel = proj;
            emission.AddChildDependant(proj);
            emission.endProjectileSharesPierce = true;
            emission.emissionAtEndModel = new SingleEmissionModel("", null);
            emission.AddChildDependant(emission.emissionAtEndModel);
        });
        
        
        foreach (var damageModel in towerModel.FindDescendants<ProjectileModel>("Phaser")
                     .SelectMany(model => model.FindDescendants<DamageModel>()))
        {
            damageModel.damage++;
        }
    }
}