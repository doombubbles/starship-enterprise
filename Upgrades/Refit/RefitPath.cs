using PathsPlusPlus;

namespace StarshipEnterprise.Upgrades.Refit;

public class RefitPath : PathPlusPlus<StarshipEnterprise>
{
    public override int UpgradeCount => 6;
    
    protected override int Order => 4;
    
    public override bool ValidTiers(int[] tiers) => true;

    protected override int Priority => 100;
}