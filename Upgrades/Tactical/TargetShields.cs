using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using static BTD_Mod_Helper.Api.Enums.BloonTag;

namespace StarshipEnterprise.Upgrades.Tactical;

public class TargetShields : CareerPathUpgrade<Tactical>
{
    public override int Cost => 500;
    public override int Tier => 3;

    public override string Description =>
        base.Description + "Enterpise's attacks do bonus damage to Ceramic and Fortified Bloons";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetDescendants<ProjectileModel>().ForEach(model =>
        {
            if (model.HasBehavior<DamageModel>())
            {
                model.AddBehavior(new DamageModifierForTagModel(Fortified, Fortified, 1.2f, 2, false, true));
                model.AddBehavior(new DamageModifierForTagModel(Ceramic, Ceramic, 1.2f, 2, false, true));
                model.hasDamageModifiers = true;
            }
        });
    }
}