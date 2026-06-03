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
using StarshipEnterprise.Displays.Weapons;

namespace StarshipEnterprise.Upgrades.Phasers;

public class BeamOverload : ModUpgrade<StarshipEnterprise>
{
    public override int Path => TOP;

    public override int Tier => 4;

    public override int Cost => 8000;

    public override string Icon => Name;

    public override string Description =>
        "Phaser Beams create a burst of energy on contact with Bloons, also damaging other Bloons nearby.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetDescendants<LineProjectileEmissionModel>().ForEach(emission =>
        {
            var filters = new FilterModel[]
            {
                FilterInvisibleModel.Create(new() { isActive = true })
            };
            var proj = ProjectileModel.Create(new()
            {
                id = "PhaserBlast",
                radius = 10,
                pierce = 25,
                behaviors =
                [
                    DisplayModel.Create(new()
                    {
                        name = "PhaserBlast",
                        category = DisplayCategory.Projectile
                    }),
                    AgeModel.Create(new() { name = "PhaserBlast", lifespan = .2f }),
                    ProjectileFilterModel.Create(new() { name = "PhaserBlast", filters = filters }),
                    DamageModel.Create(new()
                    {
                        name = "PhaserBlast",
                        damage = 2,
                        immuneBloonProperties = BloonProperties.Purple,
                        immuneBloonPropertiesOriginal = BloonProperties.Purple
                    }),
                    CreateEffectOnExhaustFractionModel.Create(new()
                    {
                        name = "PhaserBlast",
                        effectModel = EffectModel.Create(new()
                        {
                            assetId = CreatePrefabReference<PhaserBlast>(),
                            lifespan = .2f
                        }),
                        lifespan = .2f,
                        durationFraction = 1,
                        randomRotation = true
                    })
                ],
                filters = filters
            });
            emission.projectileInitialHitModel = proj;
            emission.AddChildDependant(proj);
            emission.endProjectileSharesPierce = true;
            emission.emissionAtEndModel = SingleEmissionModel.Create();
            emission.AddChildDependant(emission.emissionAtEndModel);
        });


        foreach (var damageModel in towerModel.FindDescendants<ProjectileModel>("Phaser")
                     .SelectMany(model => model.FindDescendants<DamageModel>()))
        {
            damageModel.damage++;
        }
    }
}