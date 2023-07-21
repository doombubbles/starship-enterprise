using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;

namespace StarshipEnterprise.Upgrades.Science;

public class ReverseThePrimeDirective : CareerPathUpgrade<Science>
{
    public override int Cost => 111111;
    public override int Tier => 6;

    public override string Description =>
        base.Description + "Give all Monkeys the most advanced Starfleet technology possible.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.AddBehavior(new RateSupportModel(Name, 1f, true, Name, true, 99, null, "", ""));
    }

    [HarmonyPatch(typeof(RateSupportModel.RateSupportMutator), nameof(RateSupportModel.RateSupportMutator.Mutate))]
    internal static class RateSupportMutator_Mutate
    {
        [HarmonyPostfix]
        private static void Postfix(RateSupportModel.RateSupportMutator __instance, Model model)
        {
            if (__instance.id != nameof(ReverseThePrimeDirective) || !model.Is(out TowerModel towerModel)) return;

            Science.PrimeDirectiveTech(towerModel, 6);
        }
    }
}