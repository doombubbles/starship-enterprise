using PathsPlusPlus;
using StarshipEnterprise.Displays.Ships;

namespace StarshipEnterprise.Upgrades.Refit;

public class ConstitutionRefit : RefitUpgrade<ConstitutionIIDisplay>
{
    public override int Tier => 1;
    public override int Cost => 1170;

    public override string Description => "Upgrade to the Enterprise-A, a Constitution-II class starship.\n" +
                                          base.Description;

    public override float BuffFactor => .1f;
}