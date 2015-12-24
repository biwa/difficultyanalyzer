
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
using CodeImp.DoomBuilder.GZBuilder.Geometry;
using CodeImp.DoomBuilder.Geometry;
using CodeImp.DoomBuilder.VisualModes;
using CodeImp.DoomBuilder.Controls;

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

		#endregion

		#region ================== Properties

		public MenusForm MenusForm { get { return menusform; } }

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
			LoadSettings();

			if (difficultyanalyzer == null)
			{
				difficultyanalyzer = new DifficultyAnalyzer();
				difficultyanalyzer.Setup();
				docker = new Docker("difficultyanalyzerdocker", "Difficulty Analyzer", difficultyanalyzer);
				General.Interface.AddDocker(docker);
			}

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

		public override void OnActionBegin(CodeImp.DoomBuilder.Actions.Action action)
		{
			base.OnActionBegin(action);

			string[] monitoractions = {
				"builder_visualedit", "builder_classicedit"
			};

			//if (monitoractions.Contains(action.Name))
			{
				// updateafteraction = true;
				// updateafteractionname = action.Name;
			}
			//else
			//	updateafteraction = false;
		}

		public override void OnActionEnd(CodeImp.DoomBuilder.Actions.Action action)
		{
			string[] monitoractions = {
				"builder_visualedit", "builder_classicedit"
			};

			Debug.Print("Action: " + action.Name);

			if(monitoractions.Contains(action.Name))
				AnalyzeDifficulty();
		}

		#region ================== Actions

		[BeginAction("difficultyanalyzer")]
		public void AnalyzeDifficulty()
		{
			Dictionary<int, Dictionary<string, int>> thingcount = new Dictionary<int, Dictionary<string, int>>();
			string[] difficultyflags = new string[] { "1", "2", "4" };

			if (docker == null)
				return;

			/*
			if (General.Interface.ActiveDockerTabName != docker.Title)
				return;
			*/

			General.Map.Map.BeginAddRemove();
			Thing comparething = General.Map.Map.CreateThing();
			General.Map.Map.EndAddRemove();

			Dictionary<string, int> totalhitpoints = new Dictionary<string, int>() {
				{ difficultyflags[0], 0 },
				{ difficultyflags[1], 0 },
				{ difficultyflags[2], 0 }
			};

			Dictionary<string, int> totalhealth = new Dictionary<string, int>() {
				{ difficultyflags[0], 0 },
				{ difficultyflags[1], 0 },
				{ difficultyflags[2], 0 }
			};

			Dictionary<string, int> totalarmor = new Dictionary<string, int>() {
				{ difficultyflags[0], 0 },
				{ difficultyflags[1], 0 },
				{ difficultyflags[2], 0 }
			};

			Dictionary<string, Dictionary<string, int>> totalammo = new Dictionary<string, Dictionary<string, int>>() {
				{ "bullets", new Dictionary<string, int> {
					{ difficultyflags[0], 0 },
					{ difficultyflags[1], 0 },
					{ difficultyflags[2], 0 }
				} },
				{ "shells", new Dictionary<string, int> {
					{ difficultyflags[0], 0 },
					{ difficultyflags[1], 0 },
					{ difficultyflags[2], 0 }
				} },
				{ "rockets", new Dictionary<string, int> {
					{ difficultyflags[0], 0 },
					{ difficultyflags[1], 0 },
					{ difficultyflags[2], 0 }
				} },
				{ "cells", new Dictionary<string, int> { 
					{ difficultyflags[0], 0 },
					{ difficultyflags[1], 0 },
					{ difficultyflags[2], 0 }
				} }
			};

			// General.Map.Config.ThingFlagsCompare

			var watch = Stopwatch.StartNew();

			foreach (string df in difficultyflags)
			{
				foreach (string sf in difficultyflags)
				{
					comparething.SetFlag(sf, false);
				}

				comparething.SetFlag(df, true);

				comparething.SetFlag("16", false); // Multiplayer only
				comparething.SetFlag("32", false); // Not coop
				comparething.SetFlag("64", false); // Not Deathmatch


				foreach (Thing t in General.Map.Map.Things)
				{
					bool countthing = false;

					if (t == comparething) continue;
					if (!config.Root.Contains(t.Type.ToString())) continue;

					if (!thingcount.ContainsKey(t.Type))
						thingcount.Add(t.Type, new Dictionary<string, int>());

					if (!thingcount[t.Type].ContainsKey(df))
							thingcount[t.Type].Add(df, 0);

					if (difficultyanalyzer.GameType == 0) { // Single player
						comparething.SetFlag("16", false); // Multiplayer only
						comparething.SetFlag("32", false); // Not coop
						comparething.SetFlag("64", false); // Not Deathmatch

						if (FlagsOverlap(comparething, t))
							countthing = true;
					}
					else if (difficultyanalyzer.GameType == 1) // Coop
					{
						bool insp = false;
						bool incoop = false;

						// Single player
						comparething.SetFlag("16", false); // Multiplayer only
						comparething.SetFlag("32", false); // Not Deathmatch
						comparething.SetFlag("64", false); // Not Coop

						if (FlagsOverlap(comparething, t))
							insp = true;

						// Multiplayer only
						comparething.SetFlag("16", true); // Multiplayer only
						comparething.SetFlag("32", false); // Not Deathmatch
						comparething.SetFlag("64", false); // Not coop

						if (FlagsOverlap(comparething, t))
							incoop = true;

						if (insp || incoop)
							countthing = true;
					}

					if(countthing)
						thingcount[t.Type][df]++;
				}
			}

			watch.Stop();

			DataGridView dgv = difficultyanalyzer.table;

			foreach (int type in thingcount.Keys)
			{
				int hitpoints = config.ReadSetting(type.ToString() + ".hitpoints", 0);
				totalhitpoints[difficultyflags[0]] += hitpoints * thingcount[type][difficultyflags[0]];
				totalhitpoints[difficultyflags[1]] += hitpoints * thingcount[type][difficultyflags[1]];
				totalhitpoints[difficultyflags[2]] += hitpoints * thingcount[type][difficultyflags[2]];

				int health = config.ReadSetting(type.ToString() + ".health", 0);
				totalhealth[difficultyflags[0]] += health * thingcount[type][difficultyflags[0]];
				totalhealth[difficultyflags[1]] += health * thingcount[type][difficultyflags[1]];
				totalhealth[difficultyflags[2]] += health * thingcount[type][difficultyflags[2]];

				int armor = config.ReadSetting(type.ToString() + ".armor", 0);
				totalarmor[difficultyflags[0]] += armor * thingcount[type][difficultyflags[0]];
				totalarmor[difficultyflags[1]] += armor * thingcount[type][difficultyflags[1]];
				totalarmor[difficultyflags[2]] += armor * thingcount[type][difficultyflags[2]];
	
				foreach (string ammotype in totalammo.Keys)
				{
					int amount = config.ReadSetting(type.ToString() + "." + ammotype, 0);

					totalammo[ammotype][difficultyflags[0]] += amount * thingcount[type][difficultyflags[0]];
					totalammo[ammotype][difficultyflags[1]] += amount * thingcount[type][difficultyflags[1]];
					totalammo[ammotype][difficultyflags[2]] += amount * thingcount[type][difficultyflags[2]];
				}
			}

			dgv.Rows.Clear();

			foreach (string category in categories)
			{
				int rowindex = dgv.Rows.Count;
				string categoryname = "";

				foreach (int type in thingcount.Keys)
				{
					// Ignore if the maps doesn't contain things of this type
					if (thingcount[type][difficultyflags[0]] <= 0 && thingcount[type][difficultyflags[1]] <= 0 && thingcount[type][difficultyflags[2]] <= 0)
						continue;

					if (config.ReadSetting(type.ToString() + ".category", "") != category)
						continue;

					categoryname = config.ReadSetting(type.ToString() + ".category", "");

					dgv.Rows.Add(new string[] {
						config.ReadSetting(type.ToString() + ".name", type.ToString()),
						thingcount[type][difficultyflags[0]].ToString(),
						thingcount[type][difficultyflags[1]].ToString(),
						thingcount[type][difficultyflags[2]].ToString(),
					});
				}

				// Add the header to the top of the category, but only if there are things in this category
				if (rowindex != dgv.Rows.Count)
				{
					dgv.Rows.Insert(rowindex, new string[] { categoryname, "", "", "" });
					dgv.Rows[rowindex].DefaultCellStyle.BackColor = Color.LightGray;
					// dgv.Rows[rowindex].DefaultCellStyle.Font = new Font(dgv.Rows[0].DefaultCellStyle.Font, FontStyle.Bold);
				}
			}

			// Add totals to the grid
			dgv.Rows.Add(new string[] { "Totals", "", "", "" });
			dgv.Rows[dgv.Rows.Count-1].DefaultCellStyle.BackColor = Color.LightGray;

			// Hitpoints
			dgv.Rows.Add(new string[] {
				"Hitpoints",
				totalhitpoints[difficultyflags[0]].ToString("n0"),
				totalhitpoints[difficultyflags[1]].ToString("n0"),
				totalhitpoints[difficultyflags[2]].ToString("n0")
			});

			// Ammo
			foreach (string ammotype in totalammo.Keys)
			{
				dgv.Rows.Add(new string[] {
					CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ammotype),
					string.Format("{0:n0} ({1:n0})", totalammo[ammotype][difficultyflags[0]], totalammo[ammotype][difficultyflags[0]]*2),
					totalammo[ammotype][difficultyflags[1]].ToString("n0"),
					string.Format("{0:n0} ({1:n0})", totalammo[ammotype][difficultyflags[2]], totalammo[ammotype][difficultyflags[2]]*2),
				});
			}

			// Health
			dgv.Rows.Add(new string[] {
				"Health",
				totalhealth[difficultyflags[0]].ToString("n0"),
				totalhealth[difficultyflags[1]].ToString("n0"),
				totalhealth[difficultyflags[2]].ToString("n0")
			});

			// Armor
			dgv.Rows.Add(new string[] {
				"Armor",
				totalarmor[difficultyflags[0]].ToString("n0"),
				totalarmor[difficultyflags[1]].ToString("n0"),
				totalarmor[difficultyflags[2]].ToString("n0")
			});

			// Update the ratios
			float maxdamage = (totalammo["bullets"][difficultyflags[0]] * 10 + totalammo["shells"][difficultyflags[0]] * 70 + totalammo["rockets"][difficultyflags[0]] * 200 + totalammo["cells"][difficultyflags[0]] * 20) * 2 * (difficultyanalyzer.PlayerAccuracy / 100.0f);
			difficultyanalyzer.ITYTDRatio = maxdamage > 0 ? (float)totalhitpoints[difficultyflags[0]] / maxdamage : 0.0f;

			maxdamage = (totalammo["bullets"][difficultyflags[0]] * 10 + totalammo["shells"][difficultyflags[0]] * 70 + totalammo["rockets"][difficultyflags[0]] * 200 + totalammo["cells"][difficultyflags[0]] * 20) * (difficultyanalyzer.PlayerAccuracy / 100.0f);
			difficultyanalyzer.HNTRRatio = maxdamage > 0 ? (float)totalhitpoints[difficultyflags[0]] / maxdamage : 0.0f;

			maxdamage = (totalammo["bullets"][difficultyflags[1]] * 10 + totalammo["shells"][difficultyflags[1]] * 70 + totalammo["rockets"][difficultyflags[1]] * 200 + totalammo["cells"][difficultyflags[1]] * 20) * (difficultyanalyzer.PlayerAccuracy / 100.0f);
			difficultyanalyzer.HMPRatio = maxdamage > 0 ? (float)totalhitpoints[difficultyflags[1]] / maxdamage : 0.0f;

			maxdamage = (totalammo["bullets"][difficultyflags[2]] * 10 + totalammo["shells"][difficultyflags[2]] * 70 + totalammo["rockets"][difficultyflags[2]] * 200 + totalammo["cells"][difficultyflags[2]] * 20) * (difficultyanalyzer.PlayerAccuracy / 100.0f);
			difficultyanalyzer.UVRatio = maxdamage > 0 ? (float)totalhitpoints[difficultyflags[2]] / maxdamage : 0.0f;

			maxdamage = (totalammo["bullets"][difficultyflags[2]] * 10 + totalammo["shells"][difficultyflags[2]] * 70 + totalammo["rockets"][difficultyflags[2]] * 200 + totalammo["cells"][difficultyflags[2]] * 20) * 2 * (difficultyanalyzer.PlayerAccuracy / 100.0f);
			difficultyanalyzer.NMRatio = maxdamage > 0 ? (float)totalhitpoints[difficultyflags[2]] / maxdamage : 0.0f;

			General.Map.Map.BeginAddRemove();
			comparething.Dispose();
			General.Map.Map.EndAddRemove();

			var elapsed = watch.ElapsedMilliseconds;

			Debug.Print("##### AnalyzeDifficulty: " + elapsed.ToString() + " ms");
		}

		#endregion

		#region ================== Methods

		// Use the same settings as the BuilderModes plugin
		private void LoadSettings()
		{
			Assembly asm = Assembly.GetExecutingAssembly();

			// Make configuration
			config = new Configuration();

			categories = new List<string>();
			
			// Find a resource named UDMF.cfg
			string[] resnames = asm.GetManifestResourceNames();
			foreach(string rn in resnames)
			{
				// Found it?
				if(rn.EndsWith("doom.cfg", StringComparison.InvariantCultureIgnoreCase))
				{
					// Get a stream from the resource
					Stream cfg = asm.GetManifestResourceStream(rn);
					StreamReader cfgreader = new StreamReader(cfg, Encoding.ASCII);

					// Load configuration from stream
					config.InputConfiguration(cfgreader.ReadToEnd());

					break;
				}
			}

			foreach (ThingCategory tc in General.Map.Data.ThingCategories)
			{
				foreach (ThingTypeInfo tti in tc.Things)
				{
					if (config.Root.Contains(tti.Index.ToString()))
					{
						config.WriteSetting(tti.Index.ToString() + ".name", tti.Title);
						config.WriteSetting(tti.Index.ToString() + ".category", tti.Category.Title);

						if (!categories.Contains(tti.Category.Title))
							categories.Add(tti.Category.Title);
					}
				}
			}
		}

		// Checks if the flags of two things overlap (i.e. if they show up at the same time)
		private static bool FlagsOverlap(Thing t1, Thing t2)
		{
			if (General.Map.Config.ThingFlagsCompare.Count < 1) return true; //mxd. Otherwise, no things will ever overlap when ThingFlagsCompare is empty
			int overlappinggroups = 0;
			int totalgroupscount = General.Map.Config.ThingFlagsCompare.Count; //mxd. Some groups can be ignored when unset...

			// Go through all flags in all groups and check if they overlap
			foreach (KeyValuePair<string, Dictionary<string, ThingFlagsCompare>> group in General.Map.Config.ThingFlagsCompare)
			{
				foreach (ThingFlagsCompare tfc in group.Value.Values)
				{
					int compareresult = tfc.Compare(t1, t2); //mxd
					if (compareresult > 0)
					{
						overlappinggroups++;
						break;
					}

					//mxd. Some groups can be ignored when unset...
					if (compareresult == 0 && tfc.IgnoreGroupWhenUnset)
					{
						totalgroupscount--;
					}
				}
			}

			// All groups have to overlap for the things to show up at the same time
			return (totalgroupscount > 0 && overlappinggroups == totalgroupscount);
		}

		#endregion

	}
}
