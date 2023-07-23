using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;

namespace StarshipEnterprise.Upgrades.Torpedoes;

public class FullSpread : ModUpgrade<StarshipEnterprise>
{
    public override int Path => MIDDLE;

    public override int Tier => 3;

    public override int Cost => 2300;

    public override string Description => "Torpedoes now fire in bursts of 3 in a wide arc.";

    public override string Icon => Name;
    
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        var emission = towerModel.FindDescendant<RandomArcEmissionModel>("PhotonTorpedo");

        emission.angle = 90;
        emission.randomAngle = 15;
        emission.Count = 3;
    }

}