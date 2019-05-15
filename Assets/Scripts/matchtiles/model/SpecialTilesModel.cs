using System;
using System.Collections.Generic;

namespace MatchTileGrid
{
    public class SpecialTilesModel : ISpecialTilesModel
    {
        public bool createSpecialTile { get; set; }

        private Dictionary<MatchTileType, SpecialTileCharge> specialTileCharges = new Dictionary<MatchTileType, SpecialTileCharge> ();

        public void AddSpecialTileCharge(SpecialTileCharge specialTileCharge)
        {
            specialTileCharges.Add (specialTileCharge.type, specialTileCharge);
        }

        public void IncreaseSpecialTileCharge(MatchTileType type, int amount = 1)
        {
            specialTileCharges [type].currentCharge += amount;
        }

        public SpecialTileCharge GetSpecialTileCharge(MatchTileType type)
        {
            return specialTileCharges [type];
        }

        public MatchTileSpecialType GetMatchTileSpecialType(string str)
        {
            MatchTileSpecialType[] types = (MatchTileSpecialType[]) Enum.GetValues (typeof(MatchTileSpecialType));
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i].ToString() == str)
                {
                    return types [i];
                }
            }
            return MatchTileSpecialType.Null;
        }   
    }
}