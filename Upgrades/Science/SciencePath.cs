using System.Linq;
using PathsPlusPlus;

namespace StarshipEnterprise.Upgrades.Science;

public class SciencePath : PathPlusPlus<StarshipEnterprise>
{
    public override int UpgradeCount => 1;

    protected override int Order => 3;

    public override bool ValidTiers(int[] tiers) => DefaultValidTiers(tiers.TakeLast(3).ToArray());
}