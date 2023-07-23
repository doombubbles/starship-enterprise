using System;
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

public class RepealThePrimeDirective : CareerPathUpgrade<Science>
{
    public override int Cost => 71770;
    public override int Tier => 5;

    public override string Description =>
        base.Description + "Monkeys have permanent access to even more advanced Starfleet technology. Ability now increases its attack speed.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        if (tier >= 6) return;

        var ability = towerModel.FindDescendant<AbilityModel>(nameof(RefuseThePrimeDirective)).SetName(Name);

        var buff = ability.GetBehavior<ActivateRateSupportZoneModel>();
        buff.mutatorId = Name;

        var buffIcon = GetInstance<BuffIconRepealThePrimeDirective>();
        towerModel.AddBehavior(new RateSupportModel(Name, 1f, true, Name, true, -1, null, buffIcon.BuffLocsName,
            buffIcon.BuffIconName));
    }

    public class BuffIconRepealThePrimeDirective : ModBuffIcon
    {
    }


    [HarmonyPatch(typeof(RateMutator), nameof(RateMutator.Mutate))]
    internal static class RateMutator_Mutate
    {
        [HarmonyPostfix]
        private static bool Prefix(RateSupportModel.RateSupportMutator __instance, Model model, ref bool __result)
        {
            if (__instance.id != nameof(RepealThePrimeDirective) || !model.Is(out TowerModel towerModel)) return true;
            
            __result = Science.PrimeDirectiveTech(towerModel, 5);

            towerModel.FindDescendants<WeaponModel>("Phaser").ForEach(w => w.Rate *= .75f);
            towerModel.FindDescendants<WeaponModel>("PhotonTorpedo").ForEach(w => w.Rate *= .75f);
            towerModel.FindDescendants<WeaponModel>("PhaseCannon").ForEach(w => w.Rate *= .75f);

            return false;
        }
    }

    [HarmonyPatch(typeof(RateSupportModel.RateSupportMutator), nameof(RateSupportModel.RateSupportMutator.Mutate))]
    internal static class RateSupportMutator_Mutate
    {
        [HarmonyPostfix]
        private static void Postfix(RateSupportModel.RateSupportMutator __instance, Model model, ref bool __result)
        {
            if (__instance.id != nameof(RepealThePrimeDirective) || !model.Is(out TowerModel towerModel)) return;

            __result = Science.PrimeDirectiveTech(towerModel, 5);
        }
    }
}