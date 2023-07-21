using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using static BTD_Mod_Helper.Api.Enums.BloonTag;

namespace StarshipEnterprise.Upgrades.Tactical;

public class ResistanceIsFutile : CareerPathUpgrade<Tactical>
{
    public override int Cost => 500;
    public override int Tier => 5;

    public override string Description => base.Description + "Enterprise attacks do bonus damage to Moab-Class Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetDescendants<ProjectileModel>().ForEach(model =>
        {
            if (model.HasBehavior<DamageModel>())
            {
                model.AddBehavior(new DamageModifierForTagModel(Moabs, Moabs, 1.2f, 2, false, true));
                model.hasDamageModifiers = true;
            }
        });
    }
}