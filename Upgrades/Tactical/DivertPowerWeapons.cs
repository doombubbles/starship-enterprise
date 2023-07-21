using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors.Abilities.Behaviors;

namespace StarshipEnterprise.Upgrades.Tactical;

public class DivertPowerWeapons : CareerPathUpgrade<Tactical>
{
    public override int Cost => 500;
    public override int Tier => 4;

    public override string DisplayName => "Divert Power: Weapons";

    public override string Description =>
        base.Description + "Ability: Divert power from all non essential systems and give it to Weapons.";

    public override bool Ability => true;

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.AddBehavior(new AbilityModel(Name, DisplayName, Description, -1, 0, IconReference, 60f, new Model[]
        {
            new TurboModel(Name, 20f, .5f, null, 0, 0, false)
        }, false, false, Id, 0f, 0, -1, true, false));
    }

    [HarmonyPatch(typeof(TurboMutator), nameof(TurboMutator.Mutate))]
    internal static class TurboMutator_Mutate
    {
        [HarmonyPostfix]
        private static void Postfix(TurboMutator __instance, Model model)
        {
            if (__instance.id != nameof(DivertPowerWeapons)) return;

            model.GetDescendants<PathMovementModel>().ForEach(movementModel => movementModel.speed /= 5);
            model.GetDescendants<FighterMovementModel>().ForEach(movementModel => movementModel.maxSpeed /= 5);
        }
    }
}