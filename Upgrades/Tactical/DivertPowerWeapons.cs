using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Helpers;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Audio;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.TowerFilters;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors.Abilities.Behaviors;
using Il2CppNinjaKiwi.Common.ResourceUtils;

namespace StarshipEnterprise.Upgrades.Tactical;

public class DivertPowerWeapons : CareerPathUpgrade<Tactical>
{
    public override int Cost => 10000;
    public override int Tier => 4;

    public override string DisplayName => "Divert Power: Weapons";

    public override string Description =>
        base.Description + "Ability: Divert power from all non essential systems and give it to Weapons.";

    public override bool Ability => true;

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        var buffIcon = GetInstance<BuffIconDivertPowerWeapons>();
        towerModel.AddBehavior(AbilityModel.Create(new()
        {
            name = Name,
            displayName = DisplayName,
            description = Description,
            animation = -1,
            icon = IconReference,
            Cooldown = 60,
            behaviors =
            [
                ActivateRateSupportZoneModel.Create(new()
                {
                    name = Name,
                    mutatorId = Name,
                    isUnique = true,
                    rateModifier = .5f,
                    range = towerModel.radius,
                    maxNumTowersModified = 1,
                    canEffectThisTower = true,
                    lifespan = 20f,
                    buffLocsName = buffIcon.BuffLocsName,
                    buffIconName = buffIcon.BuffIconName,
                    filters =
                    [
                        FilterInBaseTowerIdModel.Create(new()
                        {
                            baseIds = [Path.Tower]
                        })
                    ]
                }),
                CreateSoundOnAbilityModel.Create(new()
                {
                    sound = SoundModel.Create(new()
                    {
                        assetId = new AudioClipReference
                        {
                            guidRef = "c72781a0643d41c4b976110d1516fabc"
                        }
                    })
                })
            ],
            addedViaUpgrade = Id
        }));
    }

    public class BuffIconDivertPowerWeapons : ModBuffIcon
    {
    }

    [HarmonyPatch(typeof(RateMutator), nameof(RateMutator.Mutate))]
    internal static class RateMutator_Mutate
    {
        [HarmonyPostfix]
        private static void Postfix(RateMutator __instance, Model model)
        {
            if (__instance.id != nameof(DivertPowerWeapons)) return;

            model.GetDescendants<PathMovementModel>().ForEach(movementModel => movementModel.speed /= 10);
            model.GetDescendants<FighterMovementModel>().ForEach(movementModel => movementModel.maxSpeed /= 10);
        }
    }
}