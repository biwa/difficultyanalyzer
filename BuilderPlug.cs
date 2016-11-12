
#region ================== Copyright (c) 2015 Boris Iwanski

/*
 * Copyright (c) 2015 Boris Iwanski
 * This program is released under GNU General Public License
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 */

#endregion

#region ================== Namespaces

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Diagnostics;
using CodeImp.DoomBuilder.Windows;
using CodeImp.DoomBuilder.IO;
using CodeImp.DoomBuilder.Map;
using CodeImp.DoomBuilder.Rendering;
using CodeImp.DoomBuilder.Geometry;
using System.Drawing;
using CodeImp.DoomBuilder.Editing;
using CodeImp.DoomBuilder.Plugins;
using CodeImp.DoomBuilder.Actions;
using CodeImp.DoomBuilder.Types;
using CodeImp.DoomBuilder.Config;
using CodeImp.DoomBuilder.Data;
using CodeImp.DoomBuilder.BuilderModes;
using CodeImp.DoomBuilder.VisualModes;
using CodeImp.DoomBuilder.Controls;
using CodeImp.DoomBuilder.GZBuilder.Data;

#endregion

namespace CodeImp.DoomBuilder.DifficultyAnalyzer
{
	//
	// MANDATORY: The plug!
	// This is an important class to the Doom Builder core. Every plugin must
	// have exactly 1 class that inherits from Plug. When the plugin is loaded,
	// this class is instantiated and used to receive events from the core.
	// Make sure the class is public, because only public classes can be seen
	// by the core.
	//

	public class BuilderPlug : Plug
	{
		#region ================== Variables

		private MenusForm menusform;
		private Configuration config;
		private DifficultyAnalyzer difficultyanalyzer;
		private Docker docker;
		private List<AmmunitionType> ammunitiontypes;
		private Dictionary<int, SkillType> skilltypes;
		private List<GameMode> gamemodes;
		private List<Total> totals;
		private string mapformat;
		private string analyzegamemodetitle;
		private bool configisvalid;

		#endregion

		#region ================== Properties

		public MenusForm MenusForm { get { return menusform; } }
		public string AnalyzeGameModeTitle { get { return analyzegamemodetitle; } set { analyzegamemodetitle = value; } }
		public Dictionary<int, SkillType> SkillTypes { get { return skilltypes; } }
		public bool ConfigIsValid { get { return configisvalid; } }

		#endregion

		#region ================== Constants

		private const int TotalsIndexHealth = 0;
		private const int TotalsIndexArmor = 1;
		private const int TotalsIndexEnemyHitpoints = 2;
		private const int TotalsIndexAmmunition = 3;

		#endregion

		// Static instance. We can't use a real static class, because BuilderPlug must
		// be instantiated by the core, so we keep a static reference. (this technique
		// should be familiar to object-oriented programmers)
		private static BuilderPlug me;

		// Static property to access the BuilderPlug
		public static BuilderPlug Me { get { return me; } }

		// This plugin relies on some functionality that wasn't there in older versions
		public override int MinimumRevision { get { return 1310; } }

		List<string> categories;

		// This event is called when the plugin is initialized
		public override void OnInitialize()
		{
			base.OnInitialize();

			// This binds the methods in this class that have the BeginAction
			// and EndAction attributes with their actions. Without this, the
			// attributes are useless. Note that in classes derived from EditMode
			// this is not needed, because they are bound automatically when the
			// editing mode is engaged.
			General.Actions.BindMethods(this);

			// menusform = new MenusForm();

			// TODO: Add DB2 version check so that old DB2 versions won't crash
			// General.ErrorLogger.Add(ErrorType.Error, "zomg!");

			// Keep a static reference
			me = this;
		}

		// This is called when the plugin is terminated
		public override void Dispose()
		{
			base.Dispose();

			// This must be called to remove bound methods for actions.
			General.Actions.UnbindMethods(this);
		}

		public override void OnMapOpenEnd()
		{
			configisvalid = LoadSettings();

			if (difficultyanalyzer == null)
			{
				difficultyanalyzer = new DifficultyAnalyzer();
				docker = new Docker("difficultyanalyzerdocker", "Difficulty Analyzer", difficultyanalyzer);
				General.Interface.AddDocker(docker);
			}

			if (configisvalid)
			{
				difficultyanalyzer.Setup(skilltypes);
				difficultyanalyzer.UpdateGameModes(gamemodes);
			}

			difficultyanalyzer.ShowUI(configisvalid);

			AnalyzeDifficulty();
		}

		public override void OnMapNewEnd()
		{
			OnMapOpenEnd();
		}

		public override void OnMapCloseBegin()
		{
			// If we have a Tag Explorer panel, remove it
			if (difficultyanalyzer != null)
			{
				General.Interface.RemoveDocker(docker);
				docker = null;
				difficultyanalyzer.Dispose();
				difficultyanalyzer = null;
			}
		}

		public override void OnUndoCreated()
		{
			AnalyzeDifficulty();
		}

		public override void OnPasteEnd(PasteOptions options)
		{
			AnalyzeDifficulty();
		}

		public override void OnUndoEnd()
		{
			AnalyzeDifficulty();
		}

		public override void OnRedoEnd()
		{
			AnalyzeDifficulty();
		}

		public override void OnActionEnd(CodeImp.DoomBuilder.Actions.Action action)
		{
			string[] monitoractions = {
				"builder_visualedit",
				"builder_classicedit",
				"builder_deleteitem",
				"builder_classicselect",
				"builder_clearselection",
				"buildermodes_classicpasteproperties"
			};

			if(monitoractions.Contains(action.Name))
				AnalyzeDifficulty();
		}

		#region ================== Actions

		[BeginAction("difficultyanalyzer")]
		public void AnalyzeDifficulty()
		{
			Dictionary<int, Dictionary<int, int>> thingcount = new Dictionary<int, Dictionary<int, int>>();
			List<Thing> things;
			string[] difficultyflags = new string[] { "1", "2", "4" };

			if (docker == null || configisvalid == false)
				return;

			if (General.Map.Map.SelectedSectorsCount > 0)
			{
				things = new List<Thing>();

				foreach (Thing t in General.Map.Map.Things)
					if (t.Sector != null && t.Sector.Selected)
						things.Add(t);
			}
			else
			{
				things = General.Map.Map.Things.ToList();
			}

			Total difficultyratio = new Total("Difficulty ratio", skilltypes.Count);

			totals = new List<Total>();
			totals.Add(new Total("Health", skilltypes.Count));
			totals.Add(new Total("Armor", skilltypes.Count));
			totals.Add(new Total("Enemy hitpoints", skilltypes.Count));

			foreach (AmmunitionType at in ammunitiontypes)
				totals.Add(new Total(at.title, skilltypes.Count));

			foreach (Thing t in things)
			{
				// if (t == comparething) continue;

				if (!config.SettingExists("things." + t.Type.ToString())) continue;

				if (!thingcount.ContainsKey(t.Type))
					thingcount.Add(t.Type, new Dictionary<int, int>());

				foreach (KeyValuePair<int, SkillType> kvp in skilltypes)
				{
					if (!thingcount[t.Type].ContainsKey(kvp.Key))
						thingcount[t.Type].Add(kvp.Key, 0);

					if (GetGameModeByTitle(analyzegamemodetitle).ThingFlagsMatch(t, kvp.Key))
					{
						thingcount[t.Type][kvp.Key]++;

						totals[TotalsIndexHealth].skill[kvp.Key] += config.ReadSetting(string.Format("things.{0}.health", t.Type), 0);
						totals[TotalsIndexArmor].skill[kvp.Key] += config.ReadSetting(string.Format("things.{0}.armor", t.Type), 0);
						totals[TotalsIndexEnemyHitpoints].skill[kvp.Key] += config.ReadSetting(string.Format("things.{0}.hitpoints", t.Type), 0);


						for (int i = 0; i < ammunitiontypes.Count; i++)
							totals[TotalsIndexAmmunition + i].skill[kvp.Key] += config.ReadSetting(string.Format("things.{0}.{1}", t.Type, ammunitiontypes[i].name), 0) * kvp.Value.ammomulti;
					}
				}
			}

			foreach (KeyValuePair<int, SkillType> kvp in skilltypes)
			{
				double maxdamage = 0.0f;

				for (int i = 0; i < ammunitiontypes.Count; i++)
					maxdamage += totals[TotalsIndexAmmunition + i].skill[kvp.Key] * ammunitiontypes[i].dpu / (100.0f / difficultyanalyzer.PlayerAccuracy);

				difficultyratio.skill[kvp.Key] = maxdamage > 0.0f ? (float)Math.Round(totals[TotalsIndexEnemyHitpoints].skill[kvp.Key] / maxdamage, 3, MidpointRounding.AwayFromZero) : 0.0f;
			}

			totals.Add(difficultyratio);

			difficultyanalyzer.UpdateTable(config, thingcount, totals);
		}

		#endregion

		#region ================== Methods

		// Use the same settings as the BuilderModes plugin
		private bool LoadSettings()
		{
			Assembly asm = Assembly.GetExecutingAssembly();
			bool foundconfig = false;

			// Make configuration
			config = new Configuration();

			categories = new List<string>();

			string gamename = "";

			switch (General.Map.Config.GameType)
			{
				case GameType.DOOM:
					gamename = "doom";
					break;
				case GameType.HERETIC:
					gamename = "heretic";
					break;
				case GameType.HEXEN:
					gamename = "hexen";
					break;
				case GameType.STRIFE:
					gamename = "strife";
					break;
			}

			// Find a resource named UDMF.cfg
			string[] resnames = asm.GetManifestResourceNames();
			foreach(string rn in resnames)
			{
				// Found it?
				if(rn.EndsWith(string.Format("game_{0}.cfg", gamename), StringComparison.InvariantCultureIgnoreCase))
				{
					// Get a stream from the resource
					Stream cfg = asm.GetManifestResourceStream(rn);
					StreamReader cfgreader = new StreamReader(cfg, Encoding.ASCII);

					// Load configuration from stream
					config.InputConfiguration(cfgreader.ReadToEnd(), true);

					foundconfig = true;

					break;
				}
			}

			if (!foundconfig)
				return false;

			// Auto-detect map format based on configuration
			foundconfig = false;
			IDictionary mapformats = config.ReadSetting("mapformats", new Hashtable());

			foreach (DictionaryEntry m in mapformats)
			{
				string cfgidentifier = config.ReadSetting(string.Format("mapformats.{0}.cfgidentifier", m.Key.ToString()), "");
				Debug.WriteLine("##### " + cfgidentifier);
				Match result = Regex.Match(General.Map.Config.EngineName, cfgidentifier);

				if (result.Success)
				{
					mapformat = m.Key.ToString();
					Debug.WriteLine("##### detected format: " + mapformat);
					foundconfig = true;
					break;
				}
			}

			if (!foundconfig)
				return false;

			// Load game modes
			gamemodes = new List<GameMode>();
			IDictionary modes = config.ReadSetting(string.Format("mapformats.{0}.gamemodes", mapformat), new Hashtable());

			foreach (DictionaryEntry m in modes)
			{
				Debug.WriteLine(string.Format("Game mode: {0}", m.Key.ToString()));
				gamemodes.Add(new GameMode(config, string.Format("mapformats.{0}.gamemodes.{1}", mapformat, m.Key.ToString())));
			}

			// Load skill types
			skilltypes = new Dictionary<int, SkillType>();
			IDictionary types = config.ReadSetting("skills", new Hashtable());

			foreach (DictionaryEntry t in types)
			{
				if (!((ListDictionary)t.Value).Contains("name") || !((ListDictionary)t.Value).Contains("flag"))
					continue;

				string name = config.ReadSetting(string.Format("skills.{0}.name", t.Key), "undefined");
				string shortname = config.ReadSetting(string.Format("skills.{0}.shortname", t.Key), "");
				float ammomulti = config.ReadSetting(string.Format("skills.{0}.ammomulti", t.Key), 1.0f);
				string flag = config.ReadSetting(string.Format("skills.{0}.flag", t.Key), "");

				skilltypes.Add(Int32.Parse(t.Key.ToString()), new SkillType(name, shortname, ammomulti, flag));
			}

			// Load ammunition types
			ammunitiontypes = new List<AmmunitionType>();
			types = config.ReadSetting("ammunition", new Hashtable());

			foreach (DictionaryEntry t in types)
			{
				if (!((ListDictionary)t.Value).Contains("title") || !((ListDictionary)t.Value).Contains("dpu"))
					continue;

				string title = ((ListDictionary)t.Value)["title"].ToString();
				int dpu = int.Parse(((ListDictionary)t.Value)["dpu"].ToString());
				
				ammunitiontypes.Add(new AmmunitionType(t.Key.ToString(), title, dpu));
			}

			// Load things
			foreach (ThingCategory tc in General.Map.Data.ThingCategories)
			{
				foreach (ThingTypeInfo tti in tc.Things)
				{
					if(config.SettingExists("things." + tti.Index.ToString()))
					// if (config.Root.Contains("things." + tti.Index.ToString()))
					{
						config.WriteSetting("things." + tti.Index.ToString() + ".name", tti.Title);
						config.WriteSetting("things." + tti.Index.ToString() + ".category", tti.Category.Title);

						if (!categories.Contains(tti.Category.Title))
							categories.Add(tti.Category.Title);
					}
				}
			}

			return true;
		}

		private GameMode GetGameModeByTitle(string title)
		{
			foreach (GameMode gm in gamemodes)
				if (gm.Title == title)
					return gm;

			return null;
		}

		#endregion

	}
}
