using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons;

namespace StarshipEnterprise.Upgrades.Phasers;

public class FireAtWill : ModUpgrade<StarshipEnterprise>
{
    public override int Path => TOP;
    public override int Tier => 3;
    public override int Cost => 500;

    public override string Description => "Adds additional Phasers that fire at Strong and Close Bloons.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        var phaser = towerModel.FindDescendant<AttackModel>("Phaser");

        var phaserStrong = phaser.Duplicate("PhaserStrong");

        phaserStrong.RemoveBehaviors<TargetSupplierModel>();
        phaserStrong.RemoveChildDependant(phaserStrong.targetProvider);
        phaserStrong.targetProvider = new TargetStrongAirUnitModel("", false, false);
        phaserStrong.AddBehavior(phaserStrong.targetProvider);

        phaserStrong.GetChild<WeaponModel>().ejectX = -10;
        phaserStrong.GetChild<WeaponModel>().Rate *= 1.41f;
        
        towerModel.AddBehavior(phaserStrong);
        
        var phaserClose = phaser.Duplicate("PhaserClose");
        
        phaserClose.RemoveBehaviors<TargetSupplierModel>();
        phaserClose.RemoveChildDependant(phaserClose.targetProvider);
        phaserClose.targetProvider = new TargetCloseAirUnitModel("", false, false);
        phaserClose.AddBehavior(phaserClose.targetProvider);
        
        phaserClose.GetChild<WeaponModel>().ejectX = 10;
        phaserClose.GetChild<WeaponModel>().Rate = 1.73f;
        
        towerModel.AddBehavior(phaserClose);
    }
}