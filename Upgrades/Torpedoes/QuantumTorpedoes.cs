using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using StarshipEnterprise.Displays;
using StarshipEnterprise.Displays.Weapons;

namespace StarshipEnterprise.Upgrades.Torpedoes;

public class QuantumTorpedoes : ModUpgrade<StarshipEnterprise>
{
    public override int Path => MIDDLE;

    public override int Tier => 5;

    public override int Cost => 50000;

    public override string Description =>
        "Upgrade Photon Torpedoes to powerful Quantum Torpedoes. Explosion damage, radius and pierce increased.";

    public override void ApplyUpgrade(TowerModel towerModel)
    {
        towerModel.FindDescendants<WeaponModel>("PhotonTorpedo").ForEach(weapon =>
        {
            var torpedoProj = weapon.projectile;
            torpedoProj.ApplyDisplay<QuantumTorpedo>();

            var explosion = torpedoProj.GetDescendant<ProjectileModel>();

            torpedoProj.GetDescendants<DamageModel>().ForEach(model => model.damage *= 5);
            explosion.radius *= 1.5f;
            explosion.pierce *= 2;

            var effect = torpedoProj.GetBehavior<CreateEffectOnContactModel>().effectModel;
            effect.ApplyDisplay<QuantumTorpedoExplosion>();
        });
    }
}