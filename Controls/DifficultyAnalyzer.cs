using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CodeImp.DoomBuilder.IO;
using CodeImp.DoomBuilder.Config;
// using CodeImp.DoomBuilder.Config;

namespace CodeImp.DoomBuilder.DifficultyAnalyzer
{
	public partial class DifficultyAnalyzer : UserControl
	{
		public string GameModeTitle { get { return gametype.SelectedItem.ToString(); } }
		public int PlayerAccuracy { get { return playeraccuracy.Value; } }

		public DifficultyAnalyzer()
		{
			InitializeComponent();

			playeraccuracy.Value = 100;
		}

		public void Setup(Dictionary<int, SkillType> skilltypes)
		{
			foreach (KeyValuePair<int, SkillType> kvp in skilltypes.OrderBy(o => o.Key))
			{
				table.Columns.Add(string.Format("skill{0}", kvp.Key), kvp.Value.shortname);
			}
		}

		public void ShowUI(bool show)
		{
			if (show)
			{
				invalidconfigpanel.SendToBack();
				invalidconfigpanel.Visible = false;
			}
			else
			{
				invalidconfigpanel.BringToFront();				
				invalidconfigpanel.Visible = true;
			}
		}

		public void UpdateGameModes(List<GameMode> gamemodes)
		{
			gametype.Items.Clear();

			foreach (GameMode gm in gamemodes)
				gametype.Items.Add(gm.Title);

			gametype.SelectedIndex = 0;
		}

		public void UpdateTable(Configuration config, Dictionary<int, Dictionary<int, int>> things, List<Total> totals)
		{
			Dictionary<string, List<List<string>>> data = new Dictionary<string, List<List<string>>>();
			List<DataGridViewRow> rows = new List<DataGridViewRow>();
			
			table.SuspendLayout();
			table.Rows.Clear();

			foreach (KeyValuePair<int, Dictionary<int, int>> kvp in things)
			{
				string name = config.ReadSetting(string.Format("things.{0}.name", kvp.Key), "undefined");
				string category = config.ReadSetting(string.Format("things.{0}.category", kvp.Key), "undefined");
				List<string> counts = new List<string>() { name };

				if (!data.ContainsKey(category))
					data.Add(category, new List<List<string>>());

				foreach (int c in kvp.Value.OrderBy(o => o.Key).ToDictionary(pair => pair.Key, pair => pair.Value).Values)
				{
					counts.Add(c.ToString());
				}

				data[category].Add(counts);
			}

			// Go through the defined categories so that they are in a sensible order
			foreach (ThingCategory tc in General.Map.Data.ThingCategories)
			{
				// Skip the category if the map does not contain any of its things
				if (!data.ContainsKey(tc.Title))
					continue;

				// Skip the category if none of the things in it show up on the selected game mode
				if (data[tc.Title].Sum(o => o.Skip(1).Sum(p => Int32.Parse(p))) == 0)
					continue;

				// Add Header
				AddTableHeader(tc.Title);

				// Order by thing name...
				foreach (List<string> counts in data[tc.Title].OrderBy(o => o[0]))
					// ... and add the counts to the table if the sum of all difficulties is greater than 0
					if (counts.Skip(1).Sum(o => Int32.Parse(o)) > 0)
					{
						table.Rows.Add(counts.ToArray());
					}
			}

			// Add totals
			AddTableHeader("Totals");

			foreach (Total t in totals)
			{
				table.Rows.Add(t.ToStringArray());
			}

			table.ResumeLayout();
		}

		private void AddTableHeader(string title)
		{
			List<string> header = new List<string>() { title };

			foreach (KeyValuePair<int, SkillType> kvp in BuilderPlug.Me.SkillTypes.OrderBy(o => o.Key))
				header.Add(kvp.Value.shortname);

			table.Rows.Add(header.ToArray());
			table.Rows[table.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightGray;
		}

		private void table_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
		{
			int a = e.CellValue1.ToString() == "" ? 0 : int.Parse(e.CellValue1.ToString());
			int b = e.CellValue2.ToString() == "" ? 0 : int.Parse(e.CellValue2.ToString());

			e.SortResult = a.CompareTo(b);

			e.Handled = true;
		}

		private void gametype_SelectedIndexChanged(object sender, EventArgs e)
		{
			BuilderPlug.Me.AnalyzeGameModeTitle = gametype.SelectedItem.ToString();
			BuilderPlug.Me.AnalyzeDifficulty();
		}

		private void playeraccuracy_Scroll(object sender, EventArgs e)
		{
			playeraccuracylabel.Text = playeraccuracy.Value.ToString() + "%";
			BuilderPlug.Me.AnalyzeDifficulty();
		}

		private void export_Click(object sender, EventArgs e)
		{
			exportdialog.ShowDialog();
		}

		private void exportdialog_FileOk(object sender, CancelEventArgs e)
		{
			string output = "";

			foreach (DataGridViewRow row in table.Rows)
			{
				foreach (DataGridViewCell cell in row.Cells)
				{
					output += cell.Value.ToString() + ";";
				}

				output = output.Remove(output.Length - 1);
				output += "\n";
			}

			File.WriteAllText(exportdialog.FileName, output);
		}
	}
}
