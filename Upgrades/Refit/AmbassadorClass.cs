using PathsPlusPlus;

namespace StarshipEnterprise.Upgrades.Refit;

public class AmbassadorClass : UpgradePlusPlus<RefitPath>
{
    public override int Tier => 3;
    public override int Cost => 500;
    
    public override string Description => "Upgrade to the Enterprise-C, an Ambassador class starship.\n" +
                                          "";

}