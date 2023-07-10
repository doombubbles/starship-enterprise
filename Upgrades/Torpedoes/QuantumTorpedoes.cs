using BTD_Mod_Helper.Api.Towers;
using Il2CppAssets.Scripts.Models.Towers;

namespace StarshipEnterprise.Upgrades.Torpedoes;

public class QuantumTorpedoes : ModUpgrade<StarshipEnterprise>
{
    public override int Path => BOTTOM;

    public override int Tier => 3;

    public override int Cost => 500;

    public override void ApplyUpgrade(TowerModel towerModel)
    {
    }

}