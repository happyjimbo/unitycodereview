using System;
using IoC;
using Command;

namespace MatchTileGrid
{
	public class SetUpSpecialTileChargeCommand : ICommand
	{
		[Inject] public ISpecialTilesModel specialTilesModel { private get; set; }

		public void Execute ()
		{
			Create (MatchTileType.MatchToken_A, MatchTileSpecialType.Special_Horizontal, 10);
			Create (MatchTileType.MatchToken_B, MatchTileSpecialType.Special_Vertical, 10);
			Create (MatchTileType.MatchToken_C, MatchTileSpecialType.Special_Surrounding, 10);
			Create (MatchTileType.MatchToken_D, MatchTileSpecialType.Special_Matching, 10);
			Create (MatchTileType.MatchToken_E, MatchTileSpecialType.Special_Horizontal, 10);
			Create (MatchTileType.MatchToken_F, MatchTileSpecialType.Special_Vertical, 10);
			Create (MatchTileType.MatchToken_G, MatchTileSpecialType.Special_Surrounding, 10);
		}

		private void Create(MatchTileType type, MatchTileSpecialType specialType,  int chargeNeeded)
		{
			SpecialTileCharge specialTileCharge = new SpecialTileCharge ();
			specialTileCharge.type = type;
			specialTileCharge.specialType = specialType;
			specialTileCharge.chargeNeeded = chargeNeeded;

			specialTilesModel.AddSpecialTileCharge (specialTileCharge);
		}
	}
}