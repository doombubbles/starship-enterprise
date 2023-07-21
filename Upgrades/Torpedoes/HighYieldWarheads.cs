using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;

namespace StarshipEnterprise.Upgrades.Torpedoes;

public class HighYieldWarheads : ModUpgrade<StarshipEnterprise>
{
    public override int Path => MIDDLE;

    public override int Tier => 2;

    public override int Cost => 1000;

    public override string Description => "Torpedoes deal increased damage, and bonus damage to their primary target.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        var torpedoProj = towerModel.FindDescendant<ProjectileModel>("PhotonTorpedo");

        torpedoProj.GetDescendants<DamageModel>().ForEach(model => model.damage *= 2);

        // torpedoProj.AddFilter(new FilterAllExceptTargetModel(""));

        var explosion = torpedoProj.GetDescendant<ProjectileModel>();

        torpedoProj.AddBehavior(explosion.GetBehavior<DamageModel>().Duplicate());
    }
}