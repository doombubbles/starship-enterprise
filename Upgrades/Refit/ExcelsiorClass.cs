using Il2CppAssets.Scripts.Simulation.SMath;
using StarshipEnterprise.Displays.Ships;

namespace StarshipEnterprise.Upgrades.Refit;

public class ExcelsiorClass : RefitUpgrade<ExcelsiorDisplay>
{
    public override int Tier => 2;
    public override int Cost => 2700;

    public override string Description => "Upgrade to the Enterprise-B, an Excelsior class starship.\n" +
                                          base.Description;
    
    public override float BuffFactor => .2f;

    public override Vector3 EjectOffset => new(0, 30, 5);

    public override float Speed => 50;
}