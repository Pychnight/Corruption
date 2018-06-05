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
		/// <summary>
		/// Pastes a Schematic into the world.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
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
					Main.tile[column, row].CopyFrom(readTile);
					//TSPlayer.All.SendTileSquare(column, row, 1);
										
					readColumn++;
				}

				readRow++;
			}

			//try to send update, using the pasted schematics center point, and radius
			SendTileSquare(TSPlayer.All, ref clippedRect);
		}
		
		/// <summary>
		/// Creates a Schematic, from existing tiles in the world. 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="columns"></param>
		/// <param name="rows"></param>
		/// <returns></returns>
		public static Schematic Grab(int x, int y, int columns, int rows)
		{
			//clip
			var worldRect = new Rectangle(0, 0, Main.maxTilesX, Main.maxTilesY);
			var schematicRect = new Rectangle(x, y, columns, rows);
			var clippedRect = schematicRect;

			worldRect.Intersects(ref clippedRect, out var intersects);

			var result = new Schematic(clippedRect.Width,clippedRect.Height);

			result.IsGrabbed = true;
			result.GrabbedX = clippedRect.Left;
			result.GrabbedY = clippedRect.Top;

			if( !intersects )
				return result;

			//starting position within schematic to read from.
			var writeColumnStart = clippedRect.Left - x;
			var writeRowStart = clippedRect.Top - y;

			var writeRow = writeRowStart;

			for( var row = clippedRect.Top; row < clippedRect.Bottom; row++ )
			{
				var writeColumn = writeColumnStart;

				for( var column = clippedRect.Left; column < clippedRect.Right; column++ )
				{
					//var readTile = Tiles[readColumn, readRow];
					var itile = Main.tile[column, row];
					var tile = new Tile();

					tile.CopyFrom(itile);
					result.Tiles[writeColumn, writeRow] = tile;

					writeColumn++;
				}

				writeRow++;
			}

			//chests

			//signs

			return result;
		}

		/// <summary>
		/// Used to replace modified tiles after Paste operations.
		/// </summary>
		public void Restore()
		{
			if( IsGrabbed )
			{
				Paste(GrabbedX, GrabbedY);
			}
		}
		
		/// <summary>
		/// Shortcut that combines Grab() and Paste() into a single method.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public Schematic GrabPaste(int x, int y)
		{
			var grabbed = Grab(x, y, Width, Height);

			Paste(x, y);

			return grabbed;
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
