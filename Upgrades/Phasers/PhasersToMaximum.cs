using System.Linq;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;

namespace StarshipEnterprise.Upgrades.Phasers;

public class PhasersToMaximum : ModUpgrade<StarshipEnterprise>
{
    public override int Path => TOP;
    public override int Tier => 5;
    public override int Cost => 50000;

    public override string Description =>
        "Sets Phasers to their maximum power setting, dealing immense damage and crits.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        foreach (var damageModel in towerModel.FindDescendants<ProjectileModel>("Phaser")
                     .SelectMany(model => model.FindDescendants<DamageModel>()))
        {
            damageModel.damage *= 2;
        }

        towerModel.GetDescendants<CritMultiplierModel>().ForEach(model => model.damage *= 2);
    }
}