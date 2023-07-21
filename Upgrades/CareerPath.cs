using System.Linq;
using PathsPlusPlus;

namespace StarshipEnterprise.Upgrades;

public abstract class CareerPath : PathPlusPlus<StarshipEnterprise>
{
    public override bool ValidTiers(int[] tiers) => DefaultValidTiers(tiers.TakeLast(3).ToArray());
}