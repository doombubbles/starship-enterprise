using PathsPlusPlus;

namespace StarshipEnterprise.Upgrades.Refit;

public class ConstitutionClass : UpgradePlusPlus<RefitPath>
{
    public override int Tier => 1;
    public override int Cost => 500;

    public override string DisplayName => "Constitution II Class";

    public override string Description => "Upgrade to the Enterprise-A, a Constitution refit class starship.\n" +
                                          "";
}