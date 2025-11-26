using Server;
using Server.Commands;
using Server.Items;
using Server.Targeting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bittiez.Point
{
	public class Point
	{
		public static void Initialize()
		{
			CommandSystem.Register("Point", AccessLevel.Player, new CommandEventHandler(point_OnCommand));
		}

		[Usage("Point")]
		[Description("Points at an object")]
		private static void point_OnCommand(CommandEventArgs e)
		{
			e.Mobile.SendMessage("What do you want to point at?");
			e.Mobile.Target = new PointTarget();
		}
	}


	public class PointTarget : Target
	{
		private static string objOver = "*{0} points here*";
		public PointTarget()
			: base(20, true, TargetFlags.None)
		{
		}

		protected override void OnTarget(Mobile m, object targeted)
		{
			string pointedAt = "the ground";

			if (targeted is Item)
			{
				pointedAt = ((Item)targeted).Name;
				if (pointedAt == "")
					pointedAt = "this";
				pointedAt = ((Item)targeted).Amount + " " + pointedAt;
				((Item)targeted).PublicOverheadMessage(Server.Network.MessageType.Regular, 674, true, string.Format(objOver, m.Name));
			}
			else if (targeted is Mobile)
			{
				pointedAt = ((Mobile)targeted).Name;
				((Mobile)targeted).PublicOverheadMessage(Server.Network.MessageType.Regular, 674, true, string.Format(objOver, m.Name));
			}
			else if (targeted is StaticTarget)
			{
				StaticTarget st = ((StaticTarget)targeted);
				pointedAt = st.Name;
				TheArrow theArrow = new TheArrow(m.GetDirectionTo(st.Location), st.Location, m.Map);
				theArrow.PublicOverheadMessage(Server.Network.MessageType.Regular, 674, true, string.Format(objOver, m.Name));

			}
			else if (targeted is LandTarget)
			{
				LandTarget lt = ((LandTarget)targeted);
				Point3D loc = lt.Location;
				loc.Z += 3;
				new TheArrow(m.GetDirectionTo(lt.Location), loc, m.Map);
			}
			else
			{
				Console.WriteLine("[Point Command] Object type [" + targeted.GetType().ToString() + "] needs to be added in.");
			}


			m.PublicOverheadMessage(Server.Network.MessageType.Regular, 674, true, "*points at " + pointedAt + "*");

		}

		private class TheArrow : Item
		{
			public TheArrow(Direction d, Point3D mtw, Map m)
			{
				LootType = LootType.Blessed;
				Movable = false;
				Name = "arrow";
				int IDD;
				switch (d)
				{
					default:
					case Direction.Up: IDD = 0x206A; mtw.X += 1; break;
					case Direction.North: IDD = 0x206B; mtw.Y += 1; break;
					case Direction.Right: IDD = 0x206C; mtw.X -= 1; mtw.Y += 1; break;
					case Direction.Down: IDD = 0x206E; mtw.X -= 1; break;
					case Direction.South: IDD = 0x206F; mtw.Y -= 1; break;
					case Direction.Left: IDD = 0x2070; mtw.X += 1; mtw.Y -= 1; break;
					case Direction.West: IDD = 0x2071; mtw.X += 1; break;
					case Direction.East: IDD = 0x206D; mtw.X -= 1; break;
				}


				this.ItemID = IDD;
				this.MoveToWorld(mtw, m);
				Start_Timer(TimeSpan.FromSeconds(10));
			}

			public void Start_Timer(TimeSpan s)
			{
				Timer.DelayCall(s, new TimerCallback(Timer_Callback));
			}


			public void Timer_Callback()
			{
				this.Delete();
			}


			#region Serialize
			public TheArrow(Serial serial)
				: base(serial)
			{
			}
			public override void Deserialize(GenericReader reader)
			{
				base.Deserialize(reader);
				Timer.DelayCall(TimeSpan.FromSeconds(20), new TimerCallback(Timer_Callback));
			}
			public override void Serialize(GenericWriter writer)
			{
				base.Serialize(writer);
			}
			#endregion
		}
	}
}