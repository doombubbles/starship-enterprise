using StarshipEnterprise.Displays.Ships;

namespace StarshipEnterprise.Upgrades.Refit;

public class AmbassadorClass : RefitUpgrade<AmbassadorDisplay>
{
    public override int Tier => 3;
    public override int Cost => 7010;
    
    public override string Description => "Upgrade to the Enterprise-C, an Ambassador class starship.\n" +
                                          "All attack damage increased.";

    public override float BuffFactor => .3f;
    
    public override float Speed => 40;
}