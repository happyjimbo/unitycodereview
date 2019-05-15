namespace MatchTileGrid
{
    public interface ISpecialTilesModel
    {
        bool createSpecialTile { get; set; }
        void AddSpecialTileCharge(SpecialTileCharge specialTileCharge);
        void IncreaseSpecialTileCharge(MatchTileType type, int amount = 1);
        SpecialTileCharge GetSpecialTileCharge(MatchTileType type);
        MatchTileSpecialType GetMatchTileSpecialType(string str);
    }
}