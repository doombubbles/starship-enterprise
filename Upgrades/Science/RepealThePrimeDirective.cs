using System;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;

namespace StarshipEnterprise.Upgrades.Science;

public class RepealThePrimeDirective : CareerPathUpgrade<Science>
{
    public override int Cost => 500;
    public override int Tier => 5;

    public override string Description =>
        base.Description + "Monkeys have permanent access to even more advanced Starfleet technology.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        if (tier >= 6) return;

        towerModel.AddBehavior(new RateSupportModel(Name, 1f, true, Name, true, 99, null, "", ""));
    }

    [HarmonyPatch(typeof(RateSupportModel.RateSupportMutator), nameof(RateSupportModel.RateSupportMutator.Mutate))]
    internal static class RateSupportMutator_Mutate
    {
        [HarmonyPostfix]
        private static void Postfix(RateSupportModel.RateSupportMutator __instance, Model model)
        {
            if (__instance.id != nameof(RepealThePrimeDirective) || !model.Is(out TowerModel towerModel)) return;

            
             Science.PrimeDirectiveTech(towerModel, Math.Clamp(towerModel.tier, 0, 5));
        }
    }
}