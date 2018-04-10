using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace Corruption
{
	public static class NpcFunctions
	{
		/// <summary>
		/// Counts the number of NPCs in the area matching the name. (Uses GivenOrTypeName)
		/// </summary>
		public static int CountNpcs(int x, int y, int x2, int y2, string name)
		{
			var count = 0;
			var minX = Math.Min(x, x2);
			var maxX = Math.Max(x, x2);
			var minY = Math.Min(y, y2);
			var maxY = Math.Max(y, y2);

			for( var i = 0; i<200; i++ )
			{ 
				var npc = Main.npc[i];
				if( npc.active && npc.position.X > 16 * minX && npc.position.X< 16 * maxX &&
				   npc.position.Y> 16 * minY && npc.position.Y< 16 * maxY && npc.GivenOrTypeName == name)
				{
					count = count + 1;
				}
			}
			return count;
		}
		/// <summary>
		/// Finds an NPC by name. (Uses GivenOrTypeName) 
		/// </summary>
		/// <param name="name">GivenOrTypeName to find.</param>
		/// <returns>NPC if found, null if not.</returns>
		public static NPC FindNpcByName(string name)
		{ 
			for( var i = 0; i < 200; i++ )
			{
				var npc = Main.npc[i];
				if( npc.active && npc.GivenOrTypeName == name )
					return npc;
			}

			return null;
		}

		/// <summary>
		/// Finds an NPC by type. 
		/// </summary>
		/// <param name="type">int type of NPC.</param>
		/// <returns>NPC if found, null if not.</returns>
		public static NPC FindNpcByType(int type)
		{
			for( var i = 0; i<200; i++ )
			{
				var npc = Main.npc[i];
				if( npc.active && npc.netID == type )
					return npc;
			}

			return null;
		}
	}
}
