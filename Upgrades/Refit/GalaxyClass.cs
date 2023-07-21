using StarshipEnterprise.Displays.Ships;

namespace StarshipEnterprise.Upgrades.Refit;

public class GalaxyClass : RefitUpgrade<GalaxyDisplay>
{
    public override int Tier => 4;
    public override int Cost => 17010;

    public override string Description => "Upgrade to the Enterprise-D, a Galaxy class starship.\n" +
                                          base.Description;

    public override float BuffFactor => .4f;
    
    public override float Speed => 30;
}