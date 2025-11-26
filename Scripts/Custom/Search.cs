using Server;
using Server.Commands;
using System.Text;

namespace Bittiez.Search
{
	public class Search
	{
		public const string URL = "https://duckduckgo.com/?q=";
		public const string TERM_SEPERATOR = "+";

		public static void Initialize()
		{
			CommandSystem.Register("Search", AccessLevel.Player, new CommandEventHandler(search_OnCommand));
		}

		[Usage("Search")]
		[Description("Will create a deed to open a url searching the web for the search terms specified.")]
		private static void search_OnCommand(CommandEventArgs e)
		{
			if (e.ArgString.Length < 1)
			{
				e.Mobile.SendMessage("This command should be used as [Search search terms here");
				return;
			}
			SearchDeed wd = new SearchDeed(e.ArgString, e.Mobile);
			e.Mobile.AddToBackpack(wd);
		}
	}

	public class SearchDeed : Item
	{
		private string URL, SEARCH;
		private Mobile FROM;
		public SearchDeed(string Search, Mobile From)
			: base(0x14F0)
		{
			this.SEARCH = Search;
			this.FROM = From;

			string[] split = this.SEARCH.Split(' ');
			StringBuilder searchFormat = new StringBuilder();
			foreach (string s in split)
			{
				searchFormat.Append(s + Bittiez.Search.Search.TERM_SEPERATOR);
			}

			URL = Bittiez.Search.Search.URL + searchFormat.ToString();

			Name = "Search for: " + this.SEARCH;
		}
		public SearchDeed(Serial serial) { }

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);
			list.Add(string.Format("Recommended by: {0}<br>{1}<br>Double click to open the link.", this.FROM.Name, this.URL));
		}

		public override void OnDoubleClick(Mobile from)
		{
			from.LaunchBrowser(this.URL);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);

			//v 0
			writer.Write(this.URL);
			writer.Write(this.FROM);
			writer.Write(this.SEARCH);
		}
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int v = reader.ReadInt();

			if (v >= 0)
			{
				this.URL = reader.ReadString();
				this.FROM = reader.ReadMobile();
				this.SEARCH = reader.ReadString();
			}
		}
	}
}
