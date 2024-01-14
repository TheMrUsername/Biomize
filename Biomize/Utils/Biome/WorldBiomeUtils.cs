using Nautilus.Assets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Handlers;
using Nautilus.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using static UWE.FreezeTime;

namespace Biomize.Utils.Biome
{
    public class WorldBiomeUtils
    {
        public static PrefabInfo RegisterBiomePrefab(string biomeName, WaterscapeVolume.Settings waterSettings, BiomeHandler.SkyReference sky )
        {
            BiomeHandler.RegisterBiome(biomeName, waterSettings, sky);
            PrefabInfo biomePrefabInfo = PrefabInfo.WithTechType(biomeName+"Biome");
            CustomPrefab biomePrefab = new CustomPrefab(biomePrefabInfo);
            AtmosphereVolumeTemplate biomeVolumeTemplate = new AtmosphereVolumeTemplate(biomePrefabInfo, AtmosphereVolumeTemplate.VolumeShape.Sphere, biomeName);
            biomePrefab.SetGameObject(biomeVolumeTemplate);
            biomePrefab.Register();
            return biomePrefabInfo;
        }
        public static void PlaceBiomePrefab(PrefabInfo info,Vector3 position, Vector3 scale)
        {
            CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(info.ClassID,position,Quaternion.identity,scale));
            ConsoleCommandsHandler.AddBiomeTeleportPosition(info.ClassID,position);
        }
        public static void RegisterBiomeTypeAndSpawns(string biomeName, List<EntitySlot.Type> allowedSlots, BiomeSpawnEntry[] biomeSpawnEntries, Vector3 position, Vector3 scale, int spawns)
        {

            BiomeType biomeSlot = EnumHandler.AddEntry<BiomeType>(biomeName);
            var biomeSlotInfo = PrefabInfo.WithTechType(biomeName);
            var biomeSlotPrefab = new CustomPrefab(biomeSlotInfo);
            biomeSlotPrefab.SetGameObject(() =>
            {
                var prefab = new GameObject(biomeSlotInfo.ClassID);
                prefab.AddComponent<PrefabIdentifier>().ClassId = biomeSlotInfo.ClassID;
                prefab.AddComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Near;
                var entitySlot = prefab.AddComponent<EntitySlot>();
                entitySlot.biomeType = biomeSlot;
                entitySlot.allowedTypes = allowedSlots;
                Mod.Logger.LogInfo("Registering entity slots for " + biomeSlot);
                return prefab;
            });
            biomeSlotPrefab.Register();
            foreach (BiomeSpawnEntry spawnEntry in biomeSpawnEntries)
            {
                LootDistributionHandler.AddLootDistributionData(CraftData.GetClassIdForTechType(spawnEntry.Creature),new LootDistributionData.BiomeData { biome = biomeSlot, count = spawnEntry.Count, probability = spawnEntry.Probability });
            }
            var random = new System.Random(491393);
            float spawnRadius = (scale.x + scale.y + scale.z) / 3;
            for (int i = 0; i < spawns; i++)
            {
                float distance = (float)random.NextDouble() * spawnRadius;
                float theta = (float)random.NextDouble() * Mathf.PI;
                float phi = (float)random.NextDouble() * 2f * Mathf.PI;
                Vector3 randomPos = new Vector3(Mathf.Sin(theta) * Mathf.Cos(phi), Mathf.Sin(theta) * Mathf.Sin(phi), Mathf.Cos(theta)) * distance;
                Vector3 finalSpawnPos = position + randomPos;
                Mod.Logger.LogInfo("Register biome spawn at " + finalSpawnPos.ToString());
                CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(biomeSlotInfo.ClassID, finalSpawnPos));
            }
            
        }
    }
}
