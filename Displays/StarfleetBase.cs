using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Unity.Display;

namespace StarshipEnterprise.Displays;

public class StarfleetBase : ModDisplay
{
    public override string BaseDisplay => "43873ddc40185ac438cb0da6e60e327f"; // PhoenixBase.prefab

    public override void ModifyDisplayNode(UnityDisplayNode node)
    {
        // node.PrintInfo();
        // node.SaveMeshTexture();
        
        SetMeshTexture(node, Name);
        
        node.gameObject.transform.Find("Sparks").gameObject.SetActive(false);
    }
}