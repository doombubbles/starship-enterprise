using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Data;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Simulation.Bloons.Behaviors;
using static BTD_Mod_Helper.Api.Enums.BloonTag;

namespace StarshipEnterprise.Upgrades.Torpedoes;

public class ChronitonTorpedoes : ModUpgrade<StarshipEnterprise>
{
    public override int Path => MIDDLE;

    public override int Tier => 4;

    public override int Cost => 12345;

    public override string Description =>
        "Imbue Torpedoes with Chroniton particles, giving them more pierce and making them greatly slow hit Bloons.";

    public override string Icon => Name;

    public override void Register()
    {
        base.Register();

        GameData.Instance.bloonMutatorData.badImmunityIds =
            GameData.Instance.bloonMutatorData.badImmunityIds.AddTo(Name);
    }

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.FindDescendants<ProjectileModel>("PhotonTorpedo").ForEach(proj =>
        {
            proj.pierce *= 1.5f;

            proj.AddBehavior(SlowModel.Create(new()
            {
                multiplier = .25f,
                lifespan = 5,
                mutationId = Name,
                layers = 9999,
                isUnique = true
            }));

            proj.AddBehavior(SlowModifierForTagModel.Create(new() { name = Moab, tag = Moab, slowId = Name, slowMultiplier = 1, lifespanOverride = .5f }));
            proj.AddBehavior(SlowModifierForTagModel.Create(new() { name = Bfb, tag = Bfb, slowId = Name, slowMultiplier = 1, lifespanOverride = .3f }));
            proj.AddBehavior(SlowModifierForTagModel.Create(new() { name = Ddt, tag = Ddt, slowId = Name, slowMultiplier = 1, lifespanOverride = .3f }));
            proj.AddBehavior(SlowModifierForTagModel.Create(new() { name = Zomg, tag = Zomg, slowId = Name, slowMultiplier = 1, lifespanOverride = .1f }));

            proj.UpdateCollisionPassList();
        });
    }
}