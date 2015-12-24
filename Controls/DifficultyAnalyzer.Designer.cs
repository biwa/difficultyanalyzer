namespace CodeImp.DoomBuilder.DifficultyAnalyzer
{
	partial class DifficultyAnalyzer
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.table = new System.Windows.Forms.DataGridView();
			this.thing = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.easy = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.medium = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.hard = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.gametype = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.nmratio = new System.Windows.Forms.Label();
			this.uvratio = new System.Windows.Forms.Label();
			this.hmpratio = new System.Windows.Forms.Label();
			this.hntrratio = new System.Windows.Forms.Label();
			this.itytdratio = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.playeraccuracylabel = new System.Windows.Forms.Label();
			this.playeraccuracy = new System.Windows.Forms.TrackBar();
			((System.ComponentModel.ISupportInitialize)(this.table)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.playeraccuracy)).BeginInit();
			this.SuspendLayout();
			// 
			// table
			// 
			this.table.AllowUserToAddRows = false;
			this.table.AllowUserToDeleteRows = false;
			this.table.AllowUserToResizeRows = false;
			this.table.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.table.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.table.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.table.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.thing,
            this.easy,
            this.medium,
            this.hard});
			this.table.Location = new System.Drawing.Point(4, 31);
			this.table.Name = "table";
			this.table.ReadOnly = true;
			this.table.RowHeadersVisible = false;
			this.table.Size = new System.Drawing.Size(253, 613);
			this.table.TabIndex = 0;
			this.table.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.table_SortCompare);
			// 
			// thing
			// 
			this.thing.FillWeight = 142.132F;
			this.thing.HeaderText = "Thing";
			this.thing.Name = "thing";
			// 
			// easy
			// 
			this.easy.FillWeight = 85.95601F;
			this.easy.HeaderText = "Easy";
			this.easy.Name = "easy";
			// 
			// medium
			// 
			this.medium.FillWeight = 85.95601F;
			this.medium.HeaderText = "Medium";
			this.medium.Name = "medium";
			// 
			// hard
			// 
			this.hard.FillWeight = 85.95601F;
			this.hard.HeaderText = "Hard";
			this.hard.Name = "hard";
			// 
			// gametype
			// 
			this.gametype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.gametype.FormattingEnabled = true;
			this.gametype.Items.AddRange(new object[] {
            "Single player"});
			this.gametype.Location = new System.Drawing.Point(4, 4);
			this.gametype.Name = "gametype";
			this.gametype.Size = new System.Drawing.Size(116, 21);
			this.gametype.TabIndex = 1;
			this.gametype.SelectedIndexChanged += new System.EventHandler(this.gametype_SelectedIndexChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.nmratio);
			this.groupBox1.Controls.Add(this.uvratio);
			this.groupBox1.Controls.Add(this.hmpratio);
			this.groupBox1.Controls.Add(this.hntrratio);
			this.groupBox1.Controls.Add(this.itytdratio);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(4, 650);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(156, 86);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Difficulty ratios";
			// 
			// nmratio
			// 
			this.nmratio.AutoSize = true;
			this.nmratio.Location = new System.Drawing.Point(114, 68);
			this.nmratio.Name = "nmratio";
			this.nmratio.Size = new System.Drawing.Size(22, 13);
			this.nmratio.TabIndex = 9;
			this.nmratio.Text = "1.0";
			// 
			// uvratio
			// 
			this.uvratio.AutoSize = true;
			this.uvratio.Location = new System.Drawing.Point(114, 55);
			this.uvratio.Name = "uvratio";
			this.uvratio.Size = new System.Drawing.Size(22, 13);
			this.uvratio.TabIndex = 8;
			this.uvratio.Text = "1.0";
			// 
			// hmpratio
			// 
			this.hmpratio.AutoSize = true;
			this.hmpratio.Location = new System.Drawing.Point(114, 42);
			this.hmpratio.Name = "hmpratio";
			this.hmpratio.Size = new System.Drawing.Size(22, 13);
			this.hmpratio.TabIndex = 7;
			this.hmpratio.Text = "1.0";
			// 
			// hntrratio
			// 
			this.hntrratio.AutoSize = true;
			this.hntrratio.Location = new System.Drawing.Point(114, 29);
			this.hntrratio.Name = "hntrratio";
			this.hntrratio.Size = new System.Drawing.Size(22, 13);
			this.hntrratio.TabIndex = 6;
			this.hntrratio.Text = "1.0";
			// 
			// itytdratio
			// 
			this.itytdratio.AutoSize = true;
			this.itytdratio.Location = new System.Drawing.Point(114, 16);
			this.itytdratio.Name = "itytdratio";
			this.itytdratio.Size = new System.Drawing.Size(22, 13);
			this.itytdratio.TabIndex = 5;
			this.itytdratio.Text = "1.0";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 68);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(61, 13);
			this.label5.TabIndex = 4;
			this.label5.Text = "Nightmare!:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 55);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(76, 13);
			this.label4.TabIndex = 3;
			this.label4.Text = "Ultra-Violence:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 42);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(78, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Hurt me plenty:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 29);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(98, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Hey, not too rough:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(102, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "I\'m too young to die:";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.playeraccuracylabel);
			this.groupBox2.Controls.Add(this.playeraccuracy);
			this.groupBox2.Location = new System.Drawing.Point(166, 650);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(90, 86);
			this.groupBox2.TabIndex = 14;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Accuracy";
			// 
			// playeraccuracylabel
			// 
			this.playeraccuracylabel.AutoSize = true;
			this.playeraccuracylabel.Location = new System.Drawing.Point(6, 42);
			this.playeraccuracylabel.Name = "playeraccuracylabel";
			this.playeraccuracylabel.Size = new System.Drawing.Size(33, 13);
			this.playeraccuracylabel.TabIndex = 15;
			this.playeraccuracylabel.Text = "100%";
			this.playeraccuracylabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// playeraccuracy
			// 
			this.playeraccuracy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.playeraccuracy.Location = new System.Drawing.Point(39, 16);
			this.playeraccuracy.Maximum = 100;
			this.playeraccuracy.Minimum = 1;
			this.playeraccuracy.Name = "playeraccuracy";
			this.playeraccuracy.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.playeraccuracy.Size = new System.Drawing.Size(45, 65);
			this.playeraccuracy.TabIndex = 14;
			this.playeraccuracy.TickFrequency = 20;
			this.playeraccuracy.TickStyle = System.Windows.Forms.TickStyle.Both;
			this.playeraccuracy.Value = 1;
			this.playeraccuracy.Scroll += new System.EventHandler(this.playeraccuracy_Scroll);
			// 
			// DifficultyAnalyzer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.gametype);
			this.Controls.Add(this.table);
			this.Name = "DifficultyAnalyzer";
			this.Size = new System.Drawing.Size(259, 739);
			((System.ComponentModel.ISupportInitialize)(this.table)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.playeraccuracy)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.DataGridView table;
		private System.Windows.Forms.ComboBox gametype;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label nmratio;
		private System.Windows.Forms.Label uvratio;
		private System.Windows.Forms.Label hmpratio;
		private System.Windows.Forms.Label hntrratio;
		private System.Windows.Forms.Label itytdratio;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label playeraccuracylabel;
		private System.Windows.Forms.TrackBar playeraccuracy;
		private System.Windows.Forms.DataGridViewTextBoxColumn thing;
		private System.Windows.Forms.DataGridViewTextBoxColumn easy;
		private System.Windows.Forms.DataGridViewTextBoxColumn medium;
		private System.Windows.Forms.DataGridViewTextBoxColumn hard;

	}
}
