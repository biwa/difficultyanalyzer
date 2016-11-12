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
			this.gametype = new System.Windows.Forms.ComboBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.playeraccuracylabel = new System.Windows.Forms.Label();
			this.playeraccuracy = new System.Windows.Forms.TrackBar();
			this.exportdialog = new System.Windows.Forms.SaveFileDialog();
			this.export = new System.Windows.Forms.Button();
			this.invalidconfigpanel = new System.Windows.Forms.Panel();
			this.invalidconfigpanellabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.table)).BeginInit();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.playeraccuracy)).BeginInit();
			this.invalidconfigpanel.SuspendLayout();
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
			this.table.ColumnHeadersVisible = false;
			this.table.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.thing});
			this.table.Location = new System.Drawing.Point(4, 31);
			this.table.Name = "table";
			this.table.ReadOnly = true;
			this.table.RowHeadersVisible = false;
			this.table.Size = new System.Drawing.Size(254, 632);
			this.table.TabIndex = 0;
			this.table.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.table_SortCompare);
			// 
			// thing
			// 
			this.thing.FillWeight = 142.132F;
			this.thing.HeaderText = "Thing";
			this.thing.Name = "thing";
			this.thing.ReadOnly = true;
			// 
			// gametype
			// 
			this.gametype.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gametype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.gametype.FormattingEnabled = true;
			this.gametype.Location = new System.Drawing.Point(4, 4);
			this.gametype.Name = "gametype";
			this.gametype.Size = new System.Drawing.Size(172, 21);
			this.gametype.TabIndex = 1;
			this.gametype.SelectedIndexChanged += new System.EventHandler(this.gametype_SelectedIndexChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.playeraccuracylabel);
			this.groupBox2.Controls.Add(this.playeraccuracy);
			this.groupBox2.Location = new System.Drawing.Point(4, 669);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(253, 67);
			this.groupBox2.TabIndex = 14;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Player aim accuracy";
			// 
			// playeraccuracylabel
			// 
			this.playeraccuracylabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.playeraccuracylabel.AutoSize = true;
			this.playeraccuracylabel.Location = new System.Drawing.Point(204, 31);
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
			this.playeraccuracy.Location = new System.Drawing.Point(6, 16);
			this.playeraccuracy.Maximum = 100;
			this.playeraccuracy.Minimum = 1;
			this.playeraccuracy.Name = "playeraccuracy";
			this.playeraccuracy.Size = new System.Drawing.Size(192, 45);
			this.playeraccuracy.TabIndex = 14;
			this.playeraccuracy.TickFrequency = 20;
			this.playeraccuracy.TickStyle = System.Windows.Forms.TickStyle.Both;
			this.playeraccuracy.Value = 1;
			this.playeraccuracy.Scroll += new System.EventHandler(this.playeraccuracy_Scroll);
			// 
			// exportdialog
			// 
			this.exportdialog.Filter = "Comma Separated Values (CSV)|*.csv";
			this.exportdialog.FileOk += new System.ComponentModel.CancelEventHandler(this.exportdialog_FileOk);
			// 
			// export
			// 
			this.export.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.export.Location = new System.Drawing.Point(182, 3);
			this.export.Name = "export";
			this.export.Size = new System.Drawing.Size(75, 22);
			this.export.TabIndex = 15;
			this.export.Text = "Export";
			this.export.UseVisualStyleBackColor = true;
			this.export.Click += new System.EventHandler(this.export_Click);
			// 
			// invalidconfigpanel
			// 
			this.invalidconfigpanel.Controls.Add(this.invalidconfigpanellabel);
			this.invalidconfigpanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.invalidconfigpanel.Location = new System.Drawing.Point(0, 0);
			this.invalidconfigpanel.Name = "invalidconfigpanel";
			this.invalidconfigpanel.Size = new System.Drawing.Size(260, 739);
			this.invalidconfigpanel.TabIndex = 16;
			// 
			// invalidconfigpanellabel
			// 
			this.invalidconfigpanellabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.invalidconfigpanellabel.Location = new System.Drawing.Point(0, 0);
			this.invalidconfigpanellabel.Name = "invalidconfigpanellabel";
			this.invalidconfigpanellabel.Size = new System.Drawing.Size(260, 739);
			this.invalidconfigpanellabel.TabIndex = 0;
			this.invalidconfigpanellabel.Text = "Difficulty for this game and/or\r\nmap format can not by analyzed";
			this.invalidconfigpanellabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// DifficultyAnalyzer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.export);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.gametype);
			this.Controls.Add(this.table);
			this.Controls.Add(this.invalidconfigpanel);
			this.Name = "DifficultyAnalyzer";
			this.Size = new System.Drawing.Size(260, 739);
			((System.ComponentModel.ISupportInitialize)(this.table)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.playeraccuracy)).EndInit();
			this.invalidconfigpanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.DataGridView table;
		private System.Windows.Forms.ComboBox gametype;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label playeraccuracylabel;
		private System.Windows.Forms.TrackBar playeraccuracy;
		private System.Windows.Forms.SaveFileDialog exportdialog;
		private System.Windows.Forms.Button export;
		private System.Windows.Forms.DataGridViewTextBoxColumn thing;
		private System.Windows.Forms.Panel invalidconfigpanel;
		private System.Windows.Forms.Label invalidconfigpanellabel;

	}
}
