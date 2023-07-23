using BTD_Mod_Helper.Api.Display;
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
using Il2CppAssets.Scripts.Utils;
using StarshipEnterprise.Displays;

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
        towerModel.AddBehavior(new AbilityModel(Name, DisplayName, Description, -1, 0, IconReference, 60f, new Model[]
        {
            new ActivateRateSupportZoneModel(Name, Name, true, .5f, towerModel.radius, 1, true, 20f, null,
                buffIcon.BuffLocsName, buffIcon.BuffIconName, new[]
                {
                    new FilterInBaseTowerIdModel("", new[]
                    {
                        Path.Tower
                    })
                }, false),
            new CreateSoundOnAbilityModel("", new SoundModel("", new AudioSourceReference
            {
                guidRef = "c72781a0643d41c4b976110d1516fabc" // ActivatedTurboChargeSound.prefab
            }), null, null)
        }, false, false, Id, 0f, 0, -1, false, false));
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