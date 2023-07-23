using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors.Abilities.Behaviors;

namespace StarshipEnterprise.Upgrades.Science;

public class ReverseThePrimeDirective : CareerPathUpgrade<Science>
{
    public override int Cost => 171170;
    public override int Tier => 6;

    public override string Description =>
        base.Description + "Give all Monkeys the most advanced Starfleet technology possible.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        var ability = towerModel.FindDescendant<AbilityModel>(nameof(RefuseThePrimeDirective)).SetName(Name);

        var buff = ability.GetBehavior<ActivateRateSupportZoneModel>();
        buff.mutatorId = Name;
        buff.rateModifier = .5f;
        
        var buffIcon = GetInstance<BuffIconReverseThePrimeDirective>();
        towerModel.AddBehavior(new RateSupportModel(Name, 1f, true, Name, true, -1, null, buffIcon.BuffLocsName,
            buffIcon.BuffIconName));
    }

    public class BuffIconReverseThePrimeDirective : ModBuffIcon
    {
    }

    [HarmonyPatch(typeof(RateMutator), nameof(RateMutator.Mutate))]
    internal static class RateMutator_Mutate
    {
        [HarmonyPostfix]
        private static bool Prefix(RateSupportModel.RateSupportMutator __instance, Model model, ref bool __result)
        {
            if (__instance.id != nameof(ReverseThePrimeDirective) || !model.Is(out TowerModel towerModel)) return true;

            __result = Science.PrimeDirectiveTech(towerModel, 6);
            
            towerModel.FindDescendants<WeaponModel>("Phaser").ForEach(w => w.Rate *= .5f);
            towerModel.FindDescendants<WeaponModel>("PhotonTorpedo").ForEach(w => w.Rate *= .5f);
            towerModel.FindDescendants<WeaponModel>("PhaseCannon").ForEach(w => w.Rate *= .5f);

            __result = true;
            return false;
        }
    }
    
    [HarmonyPatch(typeof(RateSupportModel.RateSupportMutator), nameof(RateSupportModel.RateSupportMutator.Mutate))]
    internal static class RateSupportMutator_Mutate
    {
        [HarmonyPostfix]
        private static void Postfix(RateSupportModel.RateSupportMutator __instance, Model model, ref bool __result)
        {
            if (__instance.id != nameof(ReverseThePrimeDirective) || !model.Is(out TowerModel towerModel)) return;

           __result = Science.PrimeDirectiveTech(towerModel, 6);
        }
    }
}