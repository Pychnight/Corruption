using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corruption.TEdit
{
	public static class ITileExtensions
	{
		/// <summary>
		/// Copies members from a TEdit style Tile to an OTAPI.Tile.ITile. 
		/// </summary>
		/// <param name="dst"></param>
		/// <param name="src"></param>
		public static void CopyFrom(this OTAPI.Tile.ITile dst, Tile src)
		{
			//still needs support for wires.

			dst.type = src.Type;
			dst.wall = src.Wall;
			dst.liquid = (byte)src.LiquidType;

			//dst.sTileHeader = src.

			dst.active(src.IsActive);
			dst.inActive(src.InActive);
			dst.actuator(src.Actuator);
			dst.color(src.TileColor);
			dst.wallColor(src.WallColor);

			switch( src.BrickStyle )
			{
				case BrickStyle.Full:
					dst.halfBrick(false);
					break;

				case BrickStyle.HalfBrick:
					dst.halfBrick(true);
					break;

				case BrickStyle.SlopeTopLeft:
				case BrickStyle.SlopeTopRight:
				case BrickStyle.SlopeBottomRight:
				case BrickStyle.SlopeBottomLeft:
					dst.slope((byte)src.BrickStyle);
					break;
			}

			dst.frameX = src.U;
			dst.frameY = src.V;
		}
	}
}
