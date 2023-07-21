using StarshipEnterprise.Displays.Ships;

namespace StarshipEnterprise.Upgrades.Refit;

public class SovereignClass : RefitUpgrade<SovereignDisplay>
{
    public override int Tier => 5;
    public override int Cost => 47000;

    public override string Description => "Upgrade to the Enterprise-E, a Sovereign class starship.\n" +
                                          base.Description;

    public override float BuffFactor => .5f;
    
    public override float Speed => 40;
}