using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;

namespace StarshipEnterprise.Upgrades.Science;

public class ModulateFrequencies : CareerPathUpgrade<Science>
{
    public override int Cost => 500;
    public override int Tier => 2;

    public override string Description => base.Description + "All of Enterprise's attacks can pop Purple Bloons.";

    public override string Icon => VanillaSprites.PurpleBloonsIcon;

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetDescendants<DamageModel>().ForEach(model => model.immuneBloonProperties = BloonProperties.None);
    }
}