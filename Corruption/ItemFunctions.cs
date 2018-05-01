using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TShockAPI;
using TShockAPI.Localization;

namespace Corruption
{
	public static class ItemFunctions
	{
		public static string GetItemNameFromId(int type)
		{
			return EnglishLanguage.GetItemNameById(type);
		}

		public static int? GetItemIdFromName(string name)
		{
			var item = TShock.Utils.GetItemByName(name).FirstOrDefault();
			return item?.type;
		}

		/// <summary>
		///     Places a chest at the specified coordinates.
		/// </summary>
		/// <param name="x">The X coordinate, which must be within the bounds of the world.</param>
		/// <param name="y">The Y coordinate, which must be within the bounds of the world.</param>
		/// <param name="style">The style.</param>
		public static void PlaceChest(int x, int y, int style)
		{
			var chestId = Chest.FindChest(x, y - 1);
			if( chestId != -1 )
			{
				return;
			}

			chestId = WorldGen.PlaceChest(x, y, style: style);
			if( chestId != -1 )
			{
				TSPlayer.All.SendData((PacketTypes)34, "", 0, x, y, style, chestId);
				return;
			}

			for( var i = x; i < x + 2; ++i )
			{
				for( var j = y - 1; j < y + 2; ++j )
				{
					var tile = Main.tile[i, j];
					if( j == y + 1 )
					{
						tile.active(true);
						tile.type = 0;
					}
					else
					{
						tile.active(false);
					}
				}
			}
			TSPlayer.All.SendTileSquare(x, y, 3);

			chestId = WorldGen.PlaceChest(x, y, style: style);
			if( chestId != -1 )
			{
				TSPlayer.All.SendData((PacketTypes)34, "", 0, x, y, style, chestId);
			}
		}

		/// <summary>
		///     Puts an item into the chest at the specified coordinates.
		/// </summary>
		/// <param name="x">The X coordinate, which must be within the bounds of the world.</param>
		/// <param name="y">The Y coordinate, which must be within the bounds of the world.</param>
		/// <param name="type">The type.</param>
		/// <param name="stack">The stack.</param>
		/// <param name="prefix">The prefix.</param>
		public static void PutItemIntoChest(int x, int y, int type, int stack = 1, byte prefix = 0)
		{
			var chestId = Chest.FindChest(x, y - 1);
			if( chestId == -1 )
			{
				return;
			}

			var chest = Main.chest[chestId];
			for( var i = 0; i < Chest.maxItems; ++i )
			{
				var item = chest.item[i];
				if( item.netID == 0 )
				{
					item.netID = type;
					item.stack = stack;
					item.prefix = prefix;
					TSPlayer.All.SendData(PacketTypes.ChestItem, "", chestId, i);
					return;
				}
			}
		}

		/// <summary>
		///     Puts an item into the chest at the specified coordinates.
		/// </summary>
		/// <param name="x">The X coordinate, which must be within the bounds of the world.</param>
		/// <param name="y">The Y coordinate, which must be within the bounds of the world.</param>
		/// <param name="itemType">The item type.</param>
		/// <param name="stack">The stack.</param>
		/// <param name="prefix">The prefix.</param>
		public static void PutItemIntoChest(int x, int y, string itemType, int stack = 1, byte prefix = 0)
		{
			var id = ItemFunctions.GetItemIdFromName(itemType);

			if( id == null )
			{
				//CustomQuestsPlugin.Instance.LogPrint($"Can't put item in chest. No id found for '{itemType}'.", TraceLevel.Error);
				return;
			}

			PutItemIntoChest(x, y, (int)id, stack, prefix);
		}
	}
}
