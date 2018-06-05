using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TShockAPI;

namespace Corruption.TEdit
{
	public partial class Schematic
	{
		public void Paste(int x, int y)
		{
			//clip
			var worldRect = new Rectangle(0, 0, Main.maxTilesX, Main.maxTilesY);
			var schematicRect = new Rectangle(x, y, Width, Height);
			var clippedRect = schematicRect;

			worldRect.Intersects(ref clippedRect, out var intersects);

			if( !intersects )
				return;

			//starting position within schematic to read from.
			var readColumnStart = clippedRect.Left - x;
			var readRowStart = clippedRect.Top - y;

			var readRow = readRowStart;

			for( var row = clippedRect.Top; row < clippedRect.Bottom; row++ )
			{
				var readColumn = readColumnStart;

				for( var column = clippedRect.Left; column < clippedRect.Right; column++ )
				{
					var readTile = Tiles[readColumn, readRow];
					//TileFunctions.SetTile(column, row, 1);
					
					//Main.tile[column, row].ResetToType(readTile.Type);
					Main.tile[column, row].CopyFrom(readTile);
					//TSPlayer.All.SendTileSquare(column, row, 1);
										
					readColumn++;
				}

				readRow++;
			}

			//try to send update, using the pasted schematics center point, and radius
			SendTileSquare(TSPlayer.All, ref clippedRect);
		}

		//should we expose this publicly?
		internal static void SendTileSquare(TSPlayer player, ref Rectangle rectangle )
		{
			//try to send update, using the pasted schematics center point, and radius
			var updateX = rectangle.Center.X;
			var updateY = rectangle.Center.Y;
			var updateSize = rectangle.Width > rectangle.Height ? rectangle.Width : rectangle.Height;

			TSPlayer.All.SendTileSquare(updateX, updateY, updateSize);
			//TSPlayer.All.SendData(PacketTypes.TileSendSquare, "", updateSize, updateX, updateY, 0, 0);
		}
	}
}
