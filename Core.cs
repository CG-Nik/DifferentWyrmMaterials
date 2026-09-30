using Alta;
using Alta.Caves;
using Alta.Networking;
using HarmonyLib;
using MelonLoader;
using System.Collections;
using System.Reflection;
using UnityEngine;

[assembly: MelonInfo(typeof(DifferentWyrmMaterials.Core), "DifferentWyrmMaterials", "1.0.0", "CGNik", null)]
[assembly: MelonGame("Alta", "A Township Tale")]
[assembly: MelonPriority(-100)]

namespace DifferentWyrmMaterials
{
    public class InitializePatch
    {
        internal static void Postfix(NetworkPrefab __instance)
        {
            switch (__instance.Hash)
            {
                case 21642u: // This is the non-trial Wyrm
                case 6004u: // This is the trial Wyrm
                    PhysicalMaterialPart physicalMaterialPart_Wyrm = __instance.gameObject.GetComponent<PhysicalMaterialPart>();
                    typeof(PhysicalMaterialPart).GetField("materialDistribution", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(physicalMaterialPart_Wyrm, Core.wyrmMaterialDistribution);
                    break;
                case 37392u: // This is the non-trial Crystal Wyrm
                case 48128u: // This is the trial Crystal Wyrm
                    PhysicalMaterialPart physicalMaterialPart_CrystalWyrm = __instance.gameObject.GetComponent<PhysicalMaterialPart>();
                    typeof(PhysicalMaterialPart).GetField("materialDistribution", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(physicalMaterialPart_CrystalWyrm, Core.crystalWyrmMaterialDistribution);
                    break;
                default:
                    break;
            }
        }
    }

    public class Core : MelonMod
    {
        public static Distribution wyrmMaterialDistribution;
        public static Distribution crystalWyrmMaterialDistribution;
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Initialized.");
        }

        public static void AddToDistribution(Distribution distribution, UnityEngine.Object topic, float baseValue, float noAttributeValue, AttributeCurveRange[] multipliers)
        {
            IList items = (IList)distribution.GetType().GetField("items", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(distribution);
            Distribution.Item item = new Distribution.Item();
            typeof(Distribution.BaseItem).GetField("topic", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(item, topic);
            typeof(Distribution.BaseItem).GetField("baseValue", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(item, baseValue);
            typeof(Distribution.BaseItem).GetField("noAttributeValue", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(item, noAttributeValue);
            typeof(Distribution.BaseItem).GetField("multipliers", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(item, multipliers);
            items.Add(item);
        }

        public static void RegisterDistribution(Distribution distribution)
        {
            Distribution.CheckItems();
            Dictionary<uint, Distribution> items = (Dictionary<uint, Distribution>)typeof(HashedGeneralValue<Distribution>).GetField("items", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
            items.Add(distribution.Hash, distribution);
        }

        public override void OnLateInitializeMelon()
        {
            wyrmMaterialDistribution = GameObject.Instantiate(Distribution.All.Where(dist => dist.Hash == 49220u).First());
            typeof(HashedGeneralValue<Distribution>).GetField("hash", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(wyrmMaterialDistribution, 49221);
            wyrmMaterialDistribution.name = "Wyrm Material Distribution";
            RegisterDistribution(wyrmMaterialDistribution);
            Distribution.Item item_wyrmFaceLeather = new Distribution.Item();
            typeof(Distribution.BaseItem).GetField("topic", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(item_wyrmFaceLeather, PhysicalMaterial.All.Where(mat => mat.Hash == 63538u).First());
            typeof(Distribution.BaseItem).GetField("baseValue", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(item_wyrmFaceLeather, 1f);
            typeof(Distribution.BaseItem).GetField("noAttributeValue", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(item_wyrmFaceLeather, 1f);
            typeof(Distribution.BaseItem).GetField("multipliers", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(item_wyrmFaceLeather, new AttributeCurveRange[] { });
            wyrmMaterialDistribution.GetType().GetField("items", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(wyrmMaterialDistribution, new List<Distribution.Item> { item_wyrmFaceLeather });
            crystalWyrmMaterialDistribution = GameObject.Instantiate(wyrmMaterialDistribution);
            typeof(HashedGeneralValue<Distribution>).GetField("hash", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(crystalWyrmMaterialDistribution, 49222);
            crystalWyrmMaterialDistribution.name = "Crystal Wyrm Material Distribution";
            RegisterDistribution(crystalWyrmMaterialDistribution);
            HarmonyInstance.Patch(AccessTools.Method(typeof(NetworkPrefab), "Initialize"), postfix: new HarmonyMethod(typeof(InitializePatch), nameof(InitializePatch.Postfix)));
        }
    }
}