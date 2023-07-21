using System.Linq;
using PathsPlusPlus;

namespace StarshipEnterprise.Upgrades.Phasers;

public class PhaserPath : PathPlusPlus<StarshipEnterprise>
{
    public override int UpgradeCount => 6;
    public override int ExtendVanillaPath => Top;
    
    public override bool ValidTiers(int[] tiers) => DefaultValidTiers(tiers.Take(3).ToArray());
}