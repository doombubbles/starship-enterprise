using System.Linq;
using PathsPlusPlus;

namespace StarshipEnterprise.Upgrades.Engineering;

public class EngineeringPath : PathPlusPlus<StarshipEnterprise>
{
    public override int UpgradeCount => 1;

    protected override int Order => 2;
    
    public override bool ValidTiers(int[] tiers) => DefaultValidTiers(tiers.TakeLast(3).ToArray());
}