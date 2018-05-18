using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace Corruption
{
	public static class ProjectileFunctions
	{
		/// <summary>
		/// Counts the number of Projectiles in the area matching the name.
		/// </summary>
		public static int CountProjectiles(int x, int y, int x2, int y2, string name)
		{
			var count = 0;
			var minX = Math.Min(x, x2);
			var maxX = Math.Max(x, x2);
			var minY = Math.Min(y, y2);
			var maxY = Math.Max(y, y2);
			
			for( var i = 0; i < Main.maxProjectiles; i++ )
			{
				var proj = Main.projectile[i];
				if( proj !=null )
				{
					if( proj.active && proj.position.X > 16 * minX && proj.position.X < 16 * maxX &&
					   proj.position.Y > 16 * minY && proj.position.Y < 16 * maxY && proj.Name == name )
					{
						count++;
					}
				}
			}

			return count;
		}

		/// <summary>
		/// Counts the number of Projectiles in the area matching the type.
		/// </summary>
		public static int CountProjectiles(int x, int y, int x2, int y2, int type)
		{
			var count = 0;
			var minX = Math.Min(x, x2);
			var maxX = Math.Max(x, x2);
			var minY = Math.Min(y, y2);
			var maxY = Math.Max(y, y2);

			for( var i = 0; i < Main.maxProjectiles; i++ )
			{
				var proj = Main.projectile[i];
				if( proj != null )
				{
					if( proj.active && proj.position.X > 16 * minX && proj.position.X < 16 * maxX &&
					   proj.position.Y > 16 * minY && proj.position.Y < 16 * maxY && proj.type == type )
					{
						count++;
					}
				}
			}

			return count;
		}
	}
}
