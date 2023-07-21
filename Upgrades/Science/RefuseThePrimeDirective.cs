using System;
using System.Linq;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.TowerFilters;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors.Abilities.Behaviors;

namespace StarshipEnterprise.Upgrades.Science;

public class RefuseThePrimeDirective : CareerPathUpgrade<Science>
{
    public override int Cost => 500;
    public override int Tier => 4;

    public override bool Ability => true;

    public override string Description =>
        base.Description + "Ability: Grant all Monkeys temporary access to advanced Starfleet technology.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        if (tier >= 5) return;

        var ability = new AbilityModel(Name, DisplayName, Description, -1, 0, IconReference, 60f, new Model[]
        {
            new ActivateRateSupportZoneModel(Name, Name, true, 1f, 99999, 99999, false, 20f, null, "", "",
                new TowerFilterModel[]
                {
                    new FilterInBaseTowerIdModel("", Science.PrimeDirectiveModes.Keys.ToArray())
                }, false)
        }, false, false, Id, 0, 0, -1, true, false);

        towerModel.AddBehavior(ability);
    }

    [HarmonyPatch(typeof(RateMutator), nameof(RateMutator.Mutate))]
    internal static class RateMutator_Mutate
    {
        [HarmonyPostfix]
        private static void Postfix(RateSupportModel.RateSupportMutator __instance, Model model)
        {
            if (__instance.id != nameof(RefuseThePrimeDirective) || !model.Is(out TowerModel towerModel)) return;

            Science.PrimeDirectiveTech(towerModel, Math.Clamp(towerModel.tier, 0, 4));
        }
    }
}