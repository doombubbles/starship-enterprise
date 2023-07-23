using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Weapons;

namespace StarshipEnterprise.Upgrades.Tactical;

public class AttackPatternAlpha : CareerPathUpgrade<Tactical>
{
    public override int Cost => 1000;
    public override int Tier => 1;

    public override string Description =>
        base.Description + "Increase the attack speed of all of Enterprise's weapons.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.GetDescendants<WeaponModel>().ForEach(weapon => weapon.Rate *= .8f);
    }
}