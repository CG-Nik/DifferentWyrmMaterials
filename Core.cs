using Alta;
using Alta.Caves;
using Alta.Networking;
using HarmonyLib;
using MelonLoader;
using System.Collections;
using System.Reflection;
using UnityEngine;
using CustomDistributionAPI;

[assembly: MelonInfo(typeof(DifferentWyrmMaterials.Core), "DifferentWyrmMaterials", "1.0.0", "CGNik", null)]
[assembly: MelonGame("Alta", "A Township Tale")]
[assembly: MelonPriority(-100)]

namespace DifferentWyrmMaterials
{
    public class Core : MelonMod
    {
        public static Distribution wyrmMaterialDistribution;
        public static Distribution crystalWyrmMaterialDistribution;

        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Initialized.");
        }

        public override void OnLateInitializeMelon()
        {
            wyrmMaterialDistribution = GameObject.Instantiate(Distribution.All.Where(dist => dist.Hash == 49220u).First());
            typeof(HashedGeneralValue<Distribution>).GetField("hash", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(wyrmMaterialDistribution, 49221);
            wyrmMaterialDistribution.name = "Wyrm Material Distribution";
            CustomDistributionAPI.Core.RegisterDistribution(wyrmMaterialDistribution);
            Distribution.Item item_wyrmFaceLeather = new Distribution.Item();
            typeof(Distribution.BaseItem).GetField("topic", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(item_wyrmFaceLeather, PhysicalMaterial.All.Where(mat => mat.Hash == 63538u).First());
            typeof(Distribution.BaseItem).GetField("baseValue", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(item_wyrmFaceLeather, 1f);
            typeof(Distribution.BaseItem).GetField("noAttributeValue", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(item_wyrmFaceLeather, 1f);
            typeof(Distribution.BaseItem).GetField("multipliers", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(item_wyrmFaceLeather, new AttributeCurveRange[] { });
            wyrmMaterialDistribution.GetType().GetField("items", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(wyrmMaterialDistribution, new List<Distribution.Item> { item_wyrmFaceLeather });
            crystalWyrmMaterialDistribution = GameObject.Instantiate(wyrmMaterialDistribution);
            typeof(HashedGeneralValue<Distribution>).GetField("hash", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(crystalWyrmMaterialDistribution, 49222);
            crystalWyrmMaterialDistribution.name = "Crystal Wyrm Material Distribution";
            CustomDistributionAPI.Core.RegisterDistribution(crystalWyrmMaterialDistribution);
            GameObject wyrm = (GameObject)Resources.Load("network prefabs/creatures/monsters/wyrms/Wyrm");
            PhysicalMaterialPart physicalMaterialPart_wyrm = wyrm.GetComponent<PhysicalMaterialPart>();
            typeof(PhysicalMaterialPart).GetField("materialDistribution", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(physicalMaterialPart_wyrm, Core.wyrmMaterialDistribution);
            GameObject crystalWyrm = (GameObject)Resources.Load("network prefabs/creatures/monsters/wyrms/Crystal Wyrm");
            PhysicalMaterialPart physicalMaterialPart_crystalWyrm = crystalWyrm.GetComponent<PhysicalMaterialPart>();
            typeof(PhysicalMaterialPart).GetField("materialDistribution", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(physicalMaterialPart_crystalWyrm, Core.crystalWyrmMaterialDistribution);
            GameObject wyrmTrial = (GameObject)Resources.Load("network prefabs/creatures/monsters/trial monster spawners/Wyrm (Trial)");
            PhysicalMaterialPart physicalMaterialPart_wyrmTrial = wyrmTrial.GetComponent<PhysicalMaterialPart>();
            typeof(PhysicalMaterialPart).GetField("materialDistribution", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(physicalMaterialPart_wyrmTrial, Core.wyrmMaterialDistribution);
            GameObject crystalWyrmTrial = (GameObject)Resources.Load("network prefabs/creatures/monsters/trial monster spawners/Crystal Wyrm (Trial)");
            PhysicalMaterialPart physicalMaterialPart_crystalWyrmTrial = crystalWyrmTrial.GetComponent<PhysicalMaterialPart>();
            typeof(PhysicalMaterialPart).GetField("materialDistribution", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(physicalMaterialPart_crystalWyrmTrial, Core.crystalWyrmMaterialDistribution);
        }
    }
}