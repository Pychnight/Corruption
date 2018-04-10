using Microsoft.Xna.Framework;
using OTAPI.Tile;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TShockAPI;

namespace Corruption
{
	public static class TileFunctions
	{
		public const int TileSize = 16;
		public const int HalfTileSize = TileSize / 2;

		public static bool DefaultTileFilterFunc(int column, int row)
		{
			var tile = Main.tile[column, row];
			var isActive = tile.active();
			//if( isActive &&
			//	( tile.type >= Tile.Type_Solid && tile.type <= Tile.Type_SlopeUpLeft || tile.wall != 0 ) ) // 0 - 5
			//{
			//	results.Add(new Point(col, row));
			//}

			if( !isActive )
				return false;

			var isEmpty = WorldGen.TileEmpty(column, row);

			if( !isEmpty || tile.wall != 0 || tile.liquid != 0 )
			{
				return true;

				//if(WorldGen.SolidOrSlopedTile(col,row))
				//{
				//	results.Add(new Point(col, row)); 
				//}
				//else
				//{

				//}
			}

			return false;
		}

		public static List<Point> GetOverlappedTiles(Rectangle bounds)
		{
			return GetOverlappedTiles(bounds, DefaultTileFilterFunc);
		}

		public static List<Point> GetOverlappedTiles(Rectangle bounds, Func<int, int, bool> filterFunc)
		{
			var min = bounds.TopLeft().ToTileCoordinates();
			var max = bounds.BottomRight().ToTileCoordinates();
			var tileCollisions = GetNonEmptyTiles(min.X, min.Y, max.X, max.Y, filterFunc);

			return tileCollisions;
		}

		public static List<Point> GetNonEmptyTiles(int minColumn, int minRow, int maxColumn, int maxRow)
		{
			return GetNonEmptyTiles(minColumn, minRow, maxColumn, maxRow, DefaultTileFilterFunc);
		}

		public static List<Point> GetNonEmptyTiles(int minColumn, int minRow, int maxColumn, int maxRow, Func<int, int, bool> filterFunc)
		{
			var results = new List<Point>();

			if( filterFunc == null )
				return results;

			//clip to tileset
			minColumn = Math.Max(minColumn, 0);
			minRow = Math.Max(minRow, 0);
			maxColumn = Math.Min(maxColumn, Main.tile.Width - 1);
			maxRow = Math.Min(maxRow, Main.tile.Height - 1);

			for( var row = minRow; row <= maxRow; row++ )
			{
				for( var col = minColumn; col <= maxColumn; col++ )
				{
					if( filterFunc(col, row) )
					{
						results.Add(new Point(col, row));
					}
				}
			}

			return results;//.AsReadOnly();
		}

		public static bool InTileMapBounds(int column, int row)
		{
			if( column < 0 || column > Main.maxTilesX )
				return false;

			if( row < 0 || row > Main.maxTilesY )
				return false;

			return true;
		}

		public static int TileX(float worldX)
		{
			return (int)( worldX / TileSize );
		}

		public static int TileY(float worldY)
		{
			return (int)( worldY / TileSize );
		}

		public static int WorldX(int tileX)
		{
			return tileX * TileSize;
		}

		public static int WorldY(int tileY)
		{
			return tileY * TileSize;
		}

		/// <summary>
		///     Gets the tile at the specified coordinates.
		/// </summary>
		/// <param name="x">The X coordinate, which must be in bounds.</param>
		/// <param name="y">The Y coordinate, which must be in bounds.</param>
		/// <returns>The tile.</returns>
		public static ITile GetTile(int x, int y) => Main.tile[x, y];

		//
		//public static bool SolidTile(ITile tile)
		//{
		//	return tile.active() && tile.type < Main.maxTileSets && Main.tileSolid[tile.type];
		//}

		public static bool IsSolidTile(int column, int row)
		{
			return WorldGen.SolidTile(column, row);
		}

		public static bool IsSolidOrSlopedTile(int column, int row)
		{
			return WorldGen.SolidOrSlopedTile(column, row);
		}

		public static bool IsWallTile(int column, int row)
		{
			var tile = GetTile(column, row);
			return tile.wall > 0;
		}


		public static bool IsLiquidTile(int column, int row)
		{
			var tile = GetTile(column, row);
			return tile.liquid > 0;
		}

		//public static bool IsWaterTile(int column, int row)
		//{
		//	var tile = GetTile(column, row);
		//	return tile.liquid > 0 && tile.liquidType() == 0;
		//}

		//public static bool IsTileLiquid(ITile tile)
		//{
		//	return tile.active() && tile.liquid 
		//}

		public static void SetTile(int column, int row, int type)
		{
			try
			{
				if( Main.tile[column, row]?.active() == true )
				{
					Main.tile[column, row].ResetToType((ushort)type);
					TSPlayer.All.SendTileSquare(column, row);
				}
			}
			catch( IndexOutOfRangeException rex )
			{
				//CustomNpcsPlugin.Instance.LogPrint($"Tried to SetTile on an invalid index.", TraceLevel.Error);
			}
		}

		public static void KillTile(int column, int row)
		{
			try
			{
				if( Main.tile[column, row]?.active() == true )
				{
					WorldGen.KillTile(column, row);
					TSPlayer.All.SendTileSquare(column, row);
				}
			}
			catch( IndexOutOfRangeException rex )
			{
				//CustomNpcsPlugin.Instance.LogPrint($"Tried to KillTile on an invalid index.", TraceLevel.Error);
			}
		}

		public static void RadialKillTile(int x, int y, int radius)
		{
			var box = new Rectangle(x - radius, y - radius, radius * 2, radius * 2);
			var hits = GetOverlappedTiles(box);
			var tileCenterOffset = new Vector2(HalfTileSize, HalfTileSize);
			var center = new Vector2(x, y);

			foreach( var hit in hits )
			{
				var tile = Main.tile[hit.X, hit.Y];

				//ignore walls and trees and such.
				if( tile.collisionType < 1 )
					continue;

				var tileCenter = new Vector2(hit.X * TileSize, hit.Y * TileSize);
				tileCenter += tileCenterOffset;

				var dist = tileCenter - center;

				if( dist.LengthSquared() <= ( radius * radius ) )
				{
					KillTile(hit.X, hit.Y);
				}
			}
		}

		//
		//public static void RadialKillTile(Vector2 position, int radius)
		//{
		//	RadialKillTile((int)position.X, (int)position.Y, radius);
		//}

		public static void RadialSetTile(int x, int y, int radius, int type)
		{
			var box = new Rectangle(x - radius, y - radius, radius * 2, radius * 2);
			var hits = GetOverlappedTiles(box);
			var tileCenterOffset = new Vector2(HalfTileSize, HalfTileSize);
			var center = new Vector2(x, y);

			foreach( var hit in hits )
			{
				var tile = Main.tile[hit.X, hit.Y];

				//ignore walls and trees and such.
				if( tile.collisionType < 1 )
					continue;

				var tileCenter = new Vector2(hit.X * TileSize, hit.Y * TileSize);
				tileCenter += tileCenterOffset;

				var dist = tileCenter - center;

				if( dist.LengthSquared() <= ( radius * radius ) )
				{
					SetTile(hit.X, hit.Y, type);
				}
			}
		}

		public static int CountBlocks(int x, int y, int x2, int y2, int id)
		{
			var count = 0;
			var minX = Math.Min(x, x2);
			var maxX = Math.Max(x, x2);
			var minY = Math.Min(y, y2);
			var maxY = Math.Max(y, y2);

			for( var i = minX; i <= maxX; i++ )
			{
				for( var j = minY; j <= maxY; j++ )
				{
					if( MatchesBlock(i, j, id) )
						count = count + 1;
				}
			}
			return count;
		}

		// Counts the number of blocks in the area matching the ID and frames. (Replaced in Corruption.TileFunctions. )
		public static int CountBlocksWithFrames(int x, int y, int x2, int y2, int id, int frameX, int frameY)
		{
			var count = 0;
			var minX = Math.Min(x, x2);
			var maxX = Math.Max(x, x2);
			var minY = Math.Min(y, y2);
			var maxY = Math.Max(y, y2);
			
			for( var i = minX; i<= maxX; i++)
			{ 
				for( var j = minY; j<= maxY; j++)
				{
					if( MatchesBlock(i, j, id)) //script error? no overload exists for 5 args -->  frameX, frameY) )
						count = count + 1;
				}
			}
			return count;
		}

		// Counts the number of walls in the area matching the ID.
		public static int CountWalls(int x, int y, int x2, int y2, int id)
		{
			var count = 0;
			var minX = Math.Min(x, x2);
			var maxX = Math.Max(x, x2);
			var minY = Math.Min(y, y2);
			var maxY = Math.Max(y, y2);
			
			for( var i = minX; i <= maxX; i++ )
			{ 
				for( var j = minY; j<= maxY; j++ )
				{
					if( MatchesWall(i, j, id))
						count = count + 1;
				}
			}
			return count;
		}

		// Determines if the block at the coordinates matches the ID. (Replaced in Corruption.TileFunctions. )
		public static bool MatchesBlock(int x, int y, object id)
		{
			throw new NotImplementedException("development stub. id may be int or string, fix.");

			var tile = GetTile(x, y);

			if( id == "air" )
				return !tile.active() && tile.liquid == 0;
			else if( id == "water" )
				return !tile.active() && tile.liquid > 0 && tile.liquidType() == 0;
			else if( id == "lava" )
				return !tile.active() && tile.liquid > 0 && tile.liquidType() == 1;
			else if( id == "honey" )
				return !tile.active() && tile.liquid > 0 && tile.liquidType() == 2;

			//FIXME!!! below needs to be uncommented after we sort typing in this method.
			//else
			//	return tile.active() && GetTileType(tile) == id;

			//remove line below..
			return false;
		}

		// Determines if the block at the coordinates matches the ID and frames. (Replaced in Corruption.TileFunctions. )
		public static bool MatchesBlockWithFrames(int x, int y, int id, int frameX, int frameY)
		{
			var tile = GetTile(x, y);
			return tile.active() && GetTileType(tile) == id && tile.frameX == frameX && tile.frameY == frameY;
		}

		// Determines if the wall at the coordinates matches the ID. (Replaced in Corruption.TileFunctions. )
		public static bool MatchesWall(int x, int y, int id)
		{
			var tile = GetTile(x, y);
			return tile.type == id;
		}

		// Sets the block at the coordinates to the ID.
		public static void SetBlock(int x, int y, string id)
		{
			var tile = GetTile(x, y);
			if( id == "air" )
			{
				tile.active(false);
				tile.liquid = 0;
				tile.type = 0;
			}
			else if( id == "water" )
			{
				tile.active(false);
				tile.liquid = 255;
				tile.liquidType(0);
				tile.type = 0;
			}
			else if( id == "lava")
			{
				tile.active(false);
				tile.liquid = 255;
				tile.liquidType(1);
				tile.type = 0;
			}
			else if( id == "honey")
			{
				tile.active(false);
				tile.liquid = 255;
				tile.liquidType(2);
				tile.type = 0;
			}
			else
			{ 
				tile.active(true);
				tile.liquid = 0;
				SetTileType(tile, id);
			}
		}

		// Sets the blocks in the area.
		public static void SetBlocks(int x, int y, int x2, int y2, string id)
		{
			var minX = Math.Min(x, x2);
			var maxX = Math.Max(x, x2);
			var minY = Math.Min(y, y2);
			var maxY = Math.Max(y, y2);
			for( var i = minX; i <= maxX; i++ )
			{
				for( var j = minY; j <= maxY; j++ )
				{
					SetBlock(i, j, id);
				}
			}
		}

		// Sets the walls at the coordinates.
		public static void SetWall(int x, int y, int id)
		{
			var tile = GetTile(x, y);
			tile.wall = (byte)id;
		}

		// Sets the walls in the area.
		public static void SetWalls(int x, int y, int x2, int y2, int id)
		{
			var minX = Math.Min(x, x2);
			var maxX = Math.Max(x, x2);
			var minY = Math.Min(y, y2);
			var maxY = Math.Max(y, y2);
			for( var i = minX; i <= maxX; i++ )
			{
				for( var j = minY; j <= maxY; j++ )
				{
					SetWall(i, j, id);
				}
			}
		}

		private static void SetTileType(object tile, object id)
		{
			throw new NotImplementedException("development stub.");
		}

		private static int GetTileType(ITile tile)
		{
			throw new NotImplementedException("development stub.");
			return 0;
		}
	}
}
