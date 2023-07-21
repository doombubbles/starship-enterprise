using System.Linq;
using PathsPlusPlus;

namespace StarshipEnterprise.Upgrades.Torpedoes;

public class PhotonTorpedoPath : PathPlusPlus<StarshipEnterprise>
{
    public override int UpgradeCount => 6;
    public override int ExtendVanillaPath => Middle;

    public override bool ValidTiers(int[] tiers) => DefaultValidTiers(tiers.Take(3).ToArray());
}