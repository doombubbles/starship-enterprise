using Il2CppAssets.Scripts.Models.Towers;
using StarshipEnterprise.Displays.Ships;

namespace StarshipEnterprise.Upgrades.Engineering;

public class ShuttleSquadron : CareerPathUpgrade<Engineering>
{
    public override int Cost => 500;
    public override int Tier => 5;

    public override string Description => base.Description + "Now deploys 5 total Shuttles.";
    
    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        if (tier < 6)
        {
            Engineering.AddShuttles<RunaboutDisplay>(towerModel, 5, 5f);
        }
    }
}