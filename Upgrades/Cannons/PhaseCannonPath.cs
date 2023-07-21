using System.Linq;
using PathsPlusPlus;

namespace StarshipEnterprise.Upgrades.Cannons;

public class PhaseCannonPath : PathPlusPlus<StarshipEnterprise>
{
    public override int UpgradeCount => 6;
    public override int ExtendVanillaPath => Bottom;

    public override bool ValidTiers(int[] tiers) => DefaultValidTiers(tiers.Take(3).ToArray());
}