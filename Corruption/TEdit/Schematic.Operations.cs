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
		public Rectangle TileBounds => new Rectangle(0, 0, Width, Height);

		public void Paste(int x, int y)
		{
			//clip
			var worldRect = new Rectangle(0, 0, Main.maxTilesX, Main.maxTilesY);
			var schematicRect = new Rectangle(x, y, Width, Height);
			var clipRect = schematicRect;

			worldRect.Intersects(ref schematicRect, out var intersects);

			if( !intersects )
				return;

			//int columnMin	= Math.Max(0, x);
			//int rowMin		= Math.Max(0, y);
			//int columnMax	= Math.Min(worldRect.Right - 1, x + Width);
			//int rowMax		= Math.Min(worldRect.Bottom - 1, y + Height);

			//starting position within schematic to read from.
			var readColumnStart = clipRect.Left - x;
			var readRowStart = clipRect.Top - y;

			var readRow = readRowStart;

			for( var row = clipRect.Top; row < clipRect.Bottom; row++ )
			{
				var readColumn = readColumnStart;

				for( var column = clipRect.Left; column < clipRect.Right; column++ )
				{
					var readTile = Tiles[readColumn, readRow];

					//TileFunctions.SetTile(column, row, 1);

					//if(Main.tile[column,row]==null)
					//{
					//	Debugger.Break();
					//}

					//Main.tile[column, row].ResetToType(readTile.Type);
					Main.tile[column, row].CopyFrom(readTile);
					TSPlayer.All.SendTileSquare(column, row);

					//wall 

					//tile
					//if( Main.tile[column, row]?.active() == true )
					{
						//Main.tile[column, row].ResetToType((ushort)type);
						//TSPlayer.All.SendTileSquare(column, row);
					}


					readColumn++;
				}

				readRow++;
			}
		}
	}
}
