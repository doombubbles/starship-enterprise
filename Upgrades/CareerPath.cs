using System.Linq;
using PathsPlusPlus;

namespace StarshipEnterprise.Upgrades;

public abstract class CareerPath : PathPlusPlus<StarshipEnterprise>
{
    public override bool ValidTiers(int[] tiers) => DefaultValidTiers(tiers.Skip(3).Take(3).ToArray());
}