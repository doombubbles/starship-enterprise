using System.Linq;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Unity;
using PathsPlusPlus;

namespace StarshipEnterprise.Upgrades.Torpedoes;

public class TransphasicTorpedoes : UpgradePlusPlus<PhotonTorpedoPath>
{
    public override int Tier => 6;

    public override int Cost => 275000;

    public override string Description =>
        "Torpedoes now phase through Bloon layers, dealing massive damage to all layers at the same time.";

    public override string Container => UpgradeContainerPlatinum;

    public override string Icon => Name;
    
    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.FindDescendants<ProjectileModel>("PhotonTorpedo").ForEach(model =>
        {
            foreach (var (tag, layerNumbers) in Game.instance.model.bloons.GroupBy(bloonModel => bloonModel.baseId,
                         bloonModel => bloonModel.layerNumber))
            {
                model.AddBehavior(new DamageModifierForBloonTypeModel(tag, tag, layerNumbers.First(), 0, true));
            }
            model.AddBehavior(new StripChildrenModel("", ""));

            model.hasDamageModifiers = true;
        });
    }
}