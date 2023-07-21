using Il2CppAssets.Scripts.Simulation.SMath;
using PathsPlusPlus;

namespace StarshipEnterprise.Upgrades;

public abstract class CareerPathUpgrade<T> : UpgradePlusPlus<T> where T : CareerPath
{
    public override string Description => Path.Name + " Officer - " + Title + "\n";

    private string Title => Tier switch
    {
        1 => "Ensign",
        2 => "Lieutenant",
        3 => "Lt. Commander",
        4 => "Commander",
        5 => "Captain",
        6 => "Admiral",
        _ => "Cadet"
    };

    public override string Container => Path.Name + "UpgradeContainer";
}