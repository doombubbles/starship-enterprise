using PathsPlusPlus;

namespace StarshipEnterprise.Upgrades.Refit;

public class GalaxyClass : UpgradePlusPlus<RefitPath>
{
    public override int Tier => 4;
    public override int Cost => 500;

    public override string Description => "Upgrade to the Enterprise-D, a Galaxy class starship.\n" +
                                          "";

}