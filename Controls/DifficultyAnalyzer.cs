using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CodeImp.DoomBuilder.DifficultyAnalyzer
{
	public partial class DifficultyAnalyzer : UserControl
	{
		public int GameType { get { return gametype.SelectedIndex; } }
		public float ITYTDRatio { get { return float.Parse(itytdratio.Text); } set { itytdratio.Text = value.ToString("0.000"); } }
		public float HNTRRatio { get { return float.Parse(hntrratio.Text); } set { hntrratio.Text = value.ToString("0.000"); } }
		public float HMPRatio { get { return float.Parse(hmpratio.Text); } set { hmpratio.Text = value.ToString("0.000"); } }
		public float UVRatio { get { return float.Parse(uvratio.Text); } set { uvratio.Text = value.ToString("0.000"); } }
		public float NMRatio { get { return float.Parse(nmratio.Text); } set { nmratio.Text = value.ToString("0.000"); } }
		public int PlayerAccuracy { get { return playeraccuracy.Value; } }

		public DifficultyAnalyzer()
		{
			InitializeComponent();

			playeraccuracy.Value = 100;
		}

		public void Setup()
		{
			gametype.SelectedIndex = 0;
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
			BuilderPlug.Me.AnalyzeDifficulty();
		}

		private void playeraccuracy_Scroll(object sender, EventArgs e)
		{
			playeraccuracylabel.Text = playeraccuracy.Value.ToString() + "%";
			BuilderPlug.Me.AnalyzeDifficulty();
		}
	}
}
