using PathsPlusPlus;

namespace StarshipEnterprise.Upgrades.Refit;

public class RefitPath : PathPlusPlus<StarshipEnterprise>
{
    public override int UpgradeCount => 6;
    
    protected override int Order => 0;
    
    public override bool ValidTiers(int[] tiers) => true;
}