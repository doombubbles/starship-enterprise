using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Unity;

namespace StarshipEnterprise.Upgrades.Engineering;

public class EjectTheWarpCore : CareerPathUpgrade<Engineering>
{
    public override int Cost => 500;
    public override int Tier => 4;

    public override bool Ability => true;

    public override string Description =>
        base.Description + "Ability: Eject the Warp Core, creating an Explosion that annihilates all Bloons on Screen.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        var ability = Game.instance.model.GetTower(TowerType.MonkeyAce, 0, 5, 0).GetAbility().Duplicate(Name);

        ability.displayName = DisplayName;
        ability.description = Description;
        
        // TODO visuals

        towerModel.AddBehavior(ability);
    }
}