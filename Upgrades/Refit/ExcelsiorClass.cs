using PathsPlusPlus;

namespace StarshipEnterprise.Upgrades.Refit;

public class ExcelsiorClass : UpgradePlusPlus<RefitPath>
{
    public override int Tier => 2;
    public override int Cost => 500;
    
    public override string Description => "Upgrade to the Enterprise-B, an Excelsior class.\n" +
                                          "";

}