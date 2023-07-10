using BTD_Mod_Helper.Api.Towers;
using Il2CppAssets.Scripts.Models.Towers;

namespace StarshipEnterprise.Upgrades.Phasers;

public class TetryonBeams : ModUpgrade<StarshipEnterprise>
{
    public override int Path => TOP;

    public override int Tier => 4;

    public override int Cost => 500;
    
    public override void ApplyUpgrade(TowerModel towerModel)
    {
    }
}