using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biomize.Utils.Biome
{
    public class BiomeSpawnEntry
    {
        public TechType Creature { get; set; }
        public int Count { get; set; }
        public float Probability { get; set; }
        public BiomeSpawnEntry(TechType creature, int count,float probability)
        {
            Creature = creature;
            Count = count;
            Probability = probability;

        }
    }
}
