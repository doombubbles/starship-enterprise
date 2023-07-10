using PathsPlusPlus;

namespace StarshipEnterprise.Upgrades.Refit;

public class SovereignClass : UpgradePlusPlus<RefitPath>
{
    public override int Tier => 5;
    public override int Cost => 500;

    public override string Description => "Upgrade to the Enterprise-E, a Sovereign class starship.\n" +
                                          "";

}