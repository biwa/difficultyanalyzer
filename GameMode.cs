using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using CodeImp.DoomBuilder.IO;
using CodeImp.DoomBuilder.Map;

namespace CodeImp.DoomBuilder.DifficultyAnalyzer
{
	public class GameMode
	{
		private string title;
		private Dictionary<int, Dictionary<string, bool>> flags;

		public string Title { get { return title; } }
		public Dictionary<int, Dictionary<string, bool>> Flags { get { return flags; } }

		public GameMode(string name)
		{
			this.title = name;
			flags = new Dictionary<int, Dictionary<string, bool>>();
		}

		public GameMode(Configuration cfg, string root)
		{
			flags = new Dictionary<int, Dictionary<string, bool>>();

			title = cfg.ReadSetting(string.Format("{0}.title", root), "undefined");

			IDictionary sections = cfg.ReadSetting(string.Format("{0}.skillflags", root), new Hashtable());

			foreach (DictionaryEntry section in sections)
				foreach (DictionaryEntry flagpair in (ICollection)section.Value)
					SetFlag(Int32.Parse(section.Key.ToString()), flagpair.Key.ToString(), (bool)flagpair.Value);
			
		}

		public void SetFlag(int skill, string flag, bool on)
		{
			if (!flags.ContainsKey(skill))
				flags.Add(skill, new Dictionary<string, bool>());

			if (!flags[skill].ContainsKey(flag))
				flags[skill].Add(flag, on);
			else
				flags[skill][flag] = on;
		}

		public void SetThingFlags(Thing thing, int skill)
		{
			foreach (KeyValuePair<string, bool> entry in thing.GetFlags())
				thing.SetFlag(entry.Key, false);

			foreach (KeyValuePair<string, bool> entry in flags[skill])
				thing.SetFlag(entry.Key, entry.Value);
		}

		public bool ThingFlagsMatch(Thing thing, int skill)
		{
			foreach (KeyValuePair<string, bool> entry in flags[skill])
				if (thing.IsFlagSet(entry.Key) != flags[skill][entry.Key])
					return false;

			return true;
		}
	}
}
