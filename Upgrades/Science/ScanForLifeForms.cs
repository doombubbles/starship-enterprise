using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Filters;

namespace StarshipEnterprise.Upgrades.Science;

public class ScanForLifeForms : CareerPathUpgrade<Science>
{
    public override int Cost => 500;
    public override int Tier => 1;

    public override string Description =>
        base.Description + "Careful scanning allows Enterprise to see and pop Camo Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
    }
}