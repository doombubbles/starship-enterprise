using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Weapons;

namespace StarshipEnterprise.Upgrades.Torpedoes;

public class FasterArming : ModUpgrade<StarshipEnterprise>
{
    public override int Path => MIDDLE;

    public override int Tier => 1;

    public override int Cost => 800;

    public override string Description => "Torpedo take less time to reload time.";

    public override string Icon => Name;
    
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.FindDescendant<WeaponModel>("PhotonTorpedo").Rate *= .8f;
    }
}