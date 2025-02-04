using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Simulation.Bloons.Behaviors;
using static BTD_Mod_Helper.Api.Enums.BloonTag;

namespace StarshipEnterprise.Upgrades.Torpedoes;

public class ChronitonTorpedoes : ModUpgrade<StarshipEnterprise>
{
    public override int Path => MIDDLE;

    public override int Tier => 4;

    public override int Cost => 12345;

    public override string Description =>
        "Imbue Torpedoes with Chroniton particles, giving them more pierce and making them greatly slow hit Bloons.";

    public override string Icon => Name;
    
    public override void Register()
    {
        base.Register();

        BadImmunity.mutatorImmunityIds = BadImmunity.mutatorImmunityIds.AddTo(Name);
    }

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.FindDescendants<ProjectileModel>("PhotonTorpedo").ForEach(proj =>
        {
            proj.pierce *= 1.5f;
            
            proj.AddBehavior(new SlowModel("", .25f, 5, Name, 9999, "", true, false, null, false,
                false, false, 0));

            proj.AddBehavior(new SlowModifierForTagModel(Moab, Moab, Name, 1, false, false, .5f, false));
            proj.AddBehavior(new SlowModifierForTagModel(Bfb, Bfb, Name, 1, false, false, .3f, false));
            proj.AddBehavior(new SlowModifierForTagModel(Ddt, Ddt, Name, 1, false, false, .3f, false));
            proj.AddBehavior(new SlowModifierForTagModel(Zomg, Zomg, Name, 1, false, false, .1f, false));
            
            proj.UpdateCollisionPassList();
        });
    }
}