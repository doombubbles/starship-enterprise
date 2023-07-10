using PathsPlusPlus;

namespace StarshipEnterprise.Upgrades.Refit;

public class OdysseyClass : UpgradePlusPlus<RefitPath>
{
    public override int Tier => 6;
    public override int Cost => 500;

    public override string Description => "Upgrade to the Enterprise-F, an Odyssey class starship.\n" +
                                          "";

}