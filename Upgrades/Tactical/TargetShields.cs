using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using static BTD_Mod_Helper.Api.Enums.BloonTag;

namespace StarshipEnterprise.Upgrades.Tactical;

public class TargetShields : CareerPathUpgrade<Tactical>
{
    public override int Cost => 2700;
    public override int Tier => 3;

    public override string Description =>
        base.Description + "Enterpise's attacks do bonus damage to Ceramic and Fortified Bloons";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetDescendants<ProjectileModel>().ForEach(model =>
        {
            if (model.HasBehavior<DamageModel>())
            {
                model.AddBehavior(DamageModifierForTagModel.Create(new()
                {
                    name = Fortified,
                    tag = Fortified,
                    damageMultiplier = 1.2f,
                    damageAddative = 2,
                    applyOverMaxDamage = true
                }));
                model.AddBehavior(DamageModifierForTagModel.Create(new()
                {
                    name = Ceramic,
                    tag = Ceramic,
                    damageMultiplier = 1.2f,
                    damageAddative = 2,
                    applyOverMaxDamage = true
                }));
                model.hasDamageModifiers = true;
            }
        });
    }
}