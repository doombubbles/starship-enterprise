﻿using System.Collections.Generic;
using BTD_Mod_Helper.Api.Towers;
using Il2CppAssets.Scripts.Models.TowerSets;

namespace StarshipEnterprise;

public class Starfleet : ModTowerSet
{
    public override int GetTowerStartIndex(List<TowerDetailsModel> towerSet) => 0;
}