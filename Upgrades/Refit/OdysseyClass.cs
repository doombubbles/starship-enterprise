using Il2CppAssets.Scripts.Simulation.SMath;
using StarshipEnterprise.Displays.Ships;

namespace StarshipEnterprise.Upgrades.Refit;

public class OdysseyClass : RefitUpgrade<OdysseyDisplay>
{
    public override int Tier => 6;
    public override int Cost => 97000; // USS Odyssey Registry number

    public override string Description => "Upgrade to the Enterprise-F, an Odyssey class starship.\n" +
                                          base.Description;

    public override float BuffFactor => 1f;

    public override string Container => UpgradeContainerPlatinum;
    
    public override Vector3 EjectOffset => new(0, 40, 5);
    
    public override float Speed => 30;
}