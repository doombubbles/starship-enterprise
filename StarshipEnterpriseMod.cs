using BTD_Mod_Helper;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.ModOptions;
using HarmonyLib;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Simulation.Input;
using Il2CppAssets.Scripts.Simulation.SMath;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using MelonLoader;
using StarshipEnterprise;
using UnityEngine;
[assembly: MelonInfo(typeof(StarshipEnterpriseMod), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace StarshipEnterprise;

public class StarshipEnterpriseMod : BloonsTD6Mod
{
    public static readonly ModSettingHotkey EnterpriseHotkey = new(KeyCode.E, HotkeyModifier.Shift);

    // TODO better way to affect TowerManager.CreateTower

    private static bool roundNextValue;

    public override void OnFixedUpdate()
    {
        roundNextValue = false;
    }

    [HarmonyPatch(typeof(TowerInventory), nameof(TowerInventory.GetTowerCost))]
    internal static class TowerInventory_GetTowerCost
    {
        [HarmonyPostfix]
        private static void Postfix(TowerModel tower, ref float __result)
        {
            if (tower.baseId == ModContent.TowerID<StarshipEnterprise>() && __result > 0)
            {
                __result = 1701;
            }
        }
    }

    [HarmonyPatch(typeof(TowerInventory), nameof(TowerInventory.IsFreeTowerAvailable))]
    internal static class TowerInventory_IsFreeTowerAvailable
    {
        [HarmonyPostfix]
        private static void Postfix(TowerModel towerModel, bool __result)
        {
            roundNextValue = towerModel.baseId == ModContent.TowerID<StarshipEnterprise>() && !__result;
        }
    }

    [HarmonyPatch(typeof(Math), nameof(Math.RoundToNearestInt))]
    internal static class Math_RoundToNearestInt
    {
        [HarmonyPrefix]
        private static bool Prefix(ref int __result, int nearestIntValue)
        {
            var result = true;
            if (roundNextValue && nearestIntValue == 5)
            {
                __result = 1701;
                result = false;
            }

            roundNextValue = false;
            return result;
        }
    }
}