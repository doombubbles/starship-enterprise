using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Simulation.Towers.Projectiles.Behaviors;

namespace StarshipEnterprise.Upgrades.Phasers;

public class PhaseVariance : ModUpgrade<StarshipEnterprise>
{
    public override int Path => TOP;
    public override int Tier => 1;
    public override int Cost => 700;
    public override string Icon => Name;

    public override string Description => "Phaser shots can critically strike Bloons for 10 damage.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        var weapon = towerModel.FindDescendant<WeaponModel>("Phaser");
        weapon.AddBehavior(new CritMultiplierModel("", 10, 10, 10,
            new DisplayModel("", CreatePrefabReference(null), 0, DisplayCategory.Projectile), true));

        var projectile = towerModel.FindDescendant<ProjectileModel>("Phaser");
        projectile.AddBehavior(new ShowTextOnHitModel("Phaser",
            CreatePrefabReference("6eaf39977c73cf340b1ce55689e7a4e2"), 1, false, ""));
    }
    
    /// <summary>
    /// Fix Crit text position so it's on the Bloon and not the start of the line emission
    /// </summary>
    [HarmonyPatch(typeof(ShowTextOnHit), nameof(ShowTextOnHit.Collide))]
    internal static class ShowTextOnHit_Collide
    {
        [HarmonyPrefix]
        private static bool Prefix(ShowTextOnHit __instance, Bloon bloon)
        {
            var model = __instance.showTextOnHitModel;
            if (!model.name.Contains("Phaser") || string.IsNullOrEmpty(model.text)) return true;

            __instance.Sim.CreateTextEffect(bloon.Position, model.assetId, model.lifespan, model.text, false);

            return false;
        }
    }
}
