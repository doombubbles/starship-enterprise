using BTD_Mod_Helper.Api.Towers;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.TowerSets;

namespace StarshipEnterprise;

public class StarshipEnterprise : ModTower<Starfleet>
{
    public override string BaseTower => throw new System.NotImplementedException();

    public override int Cost => 1701;

    public override int TopPathUpgrades => 0;

    public override int MiddlePathUpgrades => 0;

    public override int BottomPathUpgrades => 0;

    public override int ShopTowerCount => 1;

    public override string DisplayName => "USS Enterprise"; // TODO or just starship?

    public override string Description => "Equipped with Phasers, Photon Torpedoes, and Deflector Shields. " +
                                          "Costs $1,701 regardless of difficulty.";

    public override void ModifyBaseTowerModel(TowerModel towerModel)
    {
    }
}