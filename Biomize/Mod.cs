using BepInEx;
using BepInEx.Logging;
using Biomize.Utils;
using Biomize.Utils.Biome;
using ECCLibrary;
using ECCLibrary.Examples;
using HarmonyLib;
using mset;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Handlers;
using Nautilus.Utility;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using static CraftNode;

namespace Biomize
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("com.snmodding.nautilus")]
    [BepInDependency("com.lee23.ecclibrary","2.0.0")]
    public class Mod : BaseUnityPlugin
    {
        public new static ManualLogSource Logger { get;private set; }

        public static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

        public static AssetBundle AssetBundle = AssetBundleLoadingUtils.LoadFromAssetsFolder(Assembly, "biomize");
        private void Awake()
        {
            Logger = base.Logger;
            BiomeSpawnEntry[] deepCrashSpawnEntries = new BiomeSpawnEntry[]
            {
                new BiomeSpawnEntry(TechType.BoneShark,3,1f),
                new BiomeSpawnEntry(TechType.Mesmer,3,1f),
                new BiomeSpawnEntry(TechType.Biter,3,1f),
            };
            PrefabInfo deepCrashBiomeInfo = WorldBiomeUtils.RegisterBiomePrefab("deepCrashZone",BiomeUtils.CreateBiomeSettings(new Vector3(16, 16, 16),1,new Color(1,1,1,1),0.12f,new Color(1,1,1,1),0,18,1,1,24),new BiomeHandler.SkyReference("SkyCrashZone"));
            WorldBiomeUtils.PlaceBiomePrefab(deepCrashBiomeInfo, new Vector3(0, 0, 0), new Vector3(128, 128, 128));
            WorldBiomeUtils.RegisterBiomeTypeAndSpawns("deepCrashZone", new List<EntitySlot.Type> { EntitySlot.Type.Creature }, deepCrashSpawnEntries, new Vector3(0, 0, 0), new Vector3(128, 128, 128), 128);
            SpawnInfo southCragReaper = new SpawnInfo(TechType.ReaperLeviathan, new Vector3(311f, -218, -1568));
            SpawnInfo eastCragReaper = new SpawnInfo(TechType.ReaperLeviathan, new Vector3(506.7f, -226, -1334));
            SpawnInfo westCragReaper = new SpawnInfo(TechType.ReaperLeviathan, new Vector3(-110,-117,-1134));
            SpawnInfo deepCragReaper = new SpawnInfo(TechType.ReaperLeviathan, new Vector3(243f, -210, -1330));

            CoordinatedSpawnsHandler.RegisterCoordinatedSpawns(new List<SpawnInfo>
            {
                southCragReaper, eastCragReaper, westCragReaper, deepCragReaper
            });
            SpawnInfo trenchGhostLeviathan = new SpawnInfo(TechType.GhostLeviathan, new Vector3(-988, -280,-546));
            CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(trenchGhostLeviathan);

            InitializePrefabs();
            Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }

        private void InitializePrefabs()
        {
        }
    }
}