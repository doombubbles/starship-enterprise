using System.Linq;
using PathsPlusPlus;

namespace StarshipEnterprise.Upgrades.Tactical;

public class TacticalPath : PathPlusPlus<StarshipEnterprise>
{
    public override int UpgradeCount => 1;

    protected override int Order => 1;
    
    public override bool ValidTiers(int[] tiers) => DefaultValidTiers(tiers.TakeLast(3).ToArray());
}