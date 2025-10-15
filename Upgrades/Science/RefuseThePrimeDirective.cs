using System;
using System.Linq;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Helpers;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Audio;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.TowerFilters;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors.Abilities.Behaviors;
using Il2CppNinjaKiwi.Common.ResourceUtils;
using StarshipEnterprise.Displays;

namespace StarshipEnterprise.Upgrades.Science;

public class RefuseThePrimeDirective : CareerPathUpgrade<Science>
{
    public override int Cost => 17770;
    public override int Tier => 4;

    public override bool Ability => true;

    public override string Description =>
        base.Description + "Ability: Grant all Monkeys temporary access to advanced Starfleet technology.";

    public override void ApplyUpgrade(TowerModel towerModel, int tier)
    {
        var buffIcon = GetInstance<BuffIconRefuseThePrimeDirective>();

        towerModel.AddBehavior(new AbilityHelper(Name)
        {
            DisplayName = DisplayName,
            Description = Description,
            Animation = -1,
            IconReference = IconReference,
            Cooldown = 60,
            Behaviors =
            [
                new ActivateRateSupportZoneModel("", Name, true, 1f, 99999, 99999, false, 20f,
                    new DisplayModel("", CreatePrefabReference<PrimeDirectiveBuff>(), 0, DisplayCategory.Buff),
                    buffIcon.BuffLocsName, buffIcon.BuffIconName, new TowerFilterModel[]
                    {
                        new FilterInBaseTowerIdModel("", Science.PrimeDirectiveModes.Keys.ToArray())
                    }, false),
                new CreateSoundOnAbilityModel("", new SoundModel("", new AudioClipReference
                {
                    guidRef =
                        "8c509ff34947707469192054a463f6b7" // Assets/Monkeys/EngineerMonkey/SoundPrefabs/AbilityOverclock.prefab
                }), null, null)
            ],
            AddedViaUpgrade = Id
        });
    }

    public class BuffIconRefuseThePrimeDirective : ModBuffIcon
    {
    }

    [HarmonyPatch(typeof(RateMutator), nameof(RateMutator.Mutate))]
    internal static class RateMutator_Mutate
    {
        [HarmonyPostfix]
        private static void Postfix(RateSupportModel.RateSupportMutator __instance, Model model, ref bool __result)
        {
            if (__instance.id != nameof(RefuseThePrimeDirective) || !model.Is(out TowerModel towerModel)) return;

            __result = Science.PrimeDirectiveTech(towerModel, Math.Clamp(towerModel.tier, 0, 4));
        }
    }
}