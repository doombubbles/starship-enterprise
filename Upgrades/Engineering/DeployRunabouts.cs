using Il2CppAssets.Scripts.Models.Towers;
using StarshipEnterprise.Displays.Ships;

namespace StarshipEnterprise.Upgrades.Engineering;

public class DeployRunabouts : CareerPathUpgrade<Engineering>
{
    public override int Cost => 500;
    public override int Tier => 3;

    public override string Description =>
        base.Description + "Upgrades deployed Shuttles to stronger Danube-class Runabouts.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        if (tier < 5)
        {
            Engineering.AddShuttles<RunaboutDisplay>(towerModel, 2, 5f);
        }
    }
}