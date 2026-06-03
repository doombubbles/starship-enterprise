using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using static BTD_Mod_Helper.Api.Enums.BloonTag;

namespace StarshipEnterprise.Upgrades.Tactical;

public class ResistanceIsFutile : CareerPathUpgrade<Tactical>
{
    public override int Cost => 42000;
    public override int Tier => 5;

    public override string Description => base.Description + "Enterprise's attacks do bonus damage to Moab-Class Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetDescendants<ProjectileModel>().ForEach(model =>
        {
            if (model.HasBehavior<DamageModel>())
            {
                model.AddBehavior(DamageModifierForTagModel.Create(new()
                {
                    name = Moabs,
                    tag = Moabs,
                    damageMultiplier = 1.2f,
                    damageAddative = 2,
                    applyOverMaxDamage = true
                }));
                model.hasDamageModifiers = true;
            }
        });
    }
}