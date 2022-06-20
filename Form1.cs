using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace H3RealTimeLogger
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListBox lbxAllLogs;
		private System.Windows.Forms.Button btnConnect;
		private System.Windows.Forms.CheckBox chkOnboard;
		private System.Windows.Forms.CheckBox chkRIR1;
		private System.Windows.Forms.CheckBox chkRIR2;
		private System.Windows.Forms.CheckBox chkO2;
		private System.Windows.Forms.CheckBox chkFaults;
		private System.Windows.Forms.CheckBox chkTWA;
		private System.Windows.Forms.CheckBox chkPPM;
		private System.Windows.Forms.DateTimePicker dtStart;
		private System.Windows.Forms.Label lblDTStart;
		private System.Windows.Forms.ListBox lbxComPorts;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Panel pnlModuleSelect;
		private System.Windows.Forms.Panel pnlLogTypes;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.Button btnExportFilteredData;
		private System.Windows.Forms.TextBox txtVersion;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			for(int i=0; i < 30; i++)
			{
				this.lbxComPorts.Items.Add("COM"+(i+1).ToString());
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}

				if ( this.cl != null)
					cl.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lbxAllLogs = new System.Windows.Forms.ListBox();
			this.btnConnect = new System.Windows.Forms.Button();
			this.chkOnboard = new System.Windows.Forms.CheckBox();
			this.chkRIR1 = new System.Windows.Forms.CheckBox();
			this.chkRIR2 = new System.Windows.Forms.CheckBox();
			this.chkO2 = new System.Windows.Forms.CheckBox();
			this.chkFaults = new System.Windows.Forms.CheckBox();
			this.chkTWA = new System.Windows.Forms.CheckBox();
			this.chkPPM = new System.Windows.Forms.CheckBox();
			this.dtStart = new System.Windows.Forms.DateTimePicker();
			this.lblDTStart = new System.Windows.Forms.Label();
			this.lbxComPorts = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.pnlModuleSelect = new System.Windows.Forms.Panel();
			this.pnlLogTypes = new System.Windows.Forms.Panel();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.btnExportFilteredData = new System.Windows.Forms.Button();
			this.txtVersion = new System.Windows.Forms.TextBox();
			this.pnlModuleSelect.SuspendLayout();
			this.pnlLogTypes.SuspendLayout();
			this.SuspendLayout();
			// 
			// lbxAllLogs
			// 
			this.lbxAllLogs.BackColor = System.Drawing.Color.PaleGreen;
			this.lbxAllLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lbxAllLogs.ItemHeight = 16;
			this.lbxAllLogs.Location = new System.Drawing.Point(8, 184);
			this.lbxAllLogs.Name = "lbxAllLogs";
			this.lbxAllLogs.ScrollAlwaysVisible = true;
			this.lbxAllLogs.Size = new System.Drawing.Size(568, 228);
			this.lbxAllLogs.TabIndex = 0;
			// 
			// btnConnect
			// 
			this.btnConnect.BackColor = System.Drawing.Color.SkyBlue;
			this.btnConnect.Location = new System.Drawing.Point(96, 8);
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.Size = new System.Drawing.Size(72, 40);
			this.btnConnect.TabIndex = 1;
			this.btnConnect.Text = "Connect";
			this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
			// 
			// chkOnboard
			// 
			this.chkOnboard.BackColor = System.Drawing.Color.MediumSlateBlue;
			this.chkOnboard.Checked = true;
			this.chkOnboard.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkOnboard.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.chkOnboard.Location = new System.Drawing.Point(8, 16);
			this.chkOnboard.Name = "chkOnboard";
			this.chkOnboard.Size = new System.Drawing.Size(96, 24);
			this.chkOnboard.TabIndex = 2;
			this.chkOnboard.Text = "Onboard";
			this.chkOnboard.CheckedChanged += new System.EventHandler(this.chkOnboard_CheckedChanged);
			// 
			// chkRIR1
			// 
			this.chkRIR1.BackColor = System.Drawing.Color.MediumSlateBlue;
			this.chkRIR1.Checked = true;
			this.chkRIR1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkRIR1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.chkRIR1.Location = new System.Drawing.Point(104, 16);
			this.chkRIR1.Name = "chkRIR1";
			this.chkRIR1.TabIndex = 3;
			this.chkRIR1.Text = "Remote IR 1";
			this.chkRIR1.CheckedChanged += new System.EventHandler(this.chkRIR1_CheckedChanged);
			// 
			// chkRIR2
			// 
			this.chkRIR2.BackColor = System.Drawing.Color.MediumSlateBlue;
			this.chkRIR2.Checked = true;
			this.chkRIR2.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkRIR2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.chkRIR2.Location = new System.Drawing.Point(104, 40);
			this.chkRIR2.Name = "chkRIR2";
			this.chkRIR2.TabIndex = 4;
			this.chkRIR2.Text = "Remote IR 2";
			this.chkRIR2.CheckedChanged += new System.EventHandler(this.chkRIR2_CheckedChanged);
			// 
			// chkO2
			// 
			this.chkO2.BackColor = System.Drawing.Color.MediumSlateBlue;
			this.chkO2.Checked = true;
			this.chkO2.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkO2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.chkO2.Location = new System.Drawing.Point(8, 40);
			this.chkO2.Name = "chkO2";
			this.chkO2.Size = new System.Drawing.Size(96, 24);
			this.chkO2.TabIndex = 5;
			this.chkO2.Text = "O2 Module";
			this.chkO2.CheckedChanged += new System.EventHandler(this.chkO2_CheckedChanged);
			// 
			// chkFaults
			// 
			this.chkFaults.Checked = true;
			this.chkFaults.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkFaults.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.chkFaults.Location = new System.Drawing.Point(8, 72);
			this.chkFaults.Name = "chkFaults";
			this.chkFaults.Size = new System.Drawing.Size(112, 24);
			this.chkFaults.TabIndex = 8;
			this.chkFaults.Text = "Faults/Alarms";
			this.chkFaults.CheckedChanged += new System.EventHandler(this.chkFaults_CheckedChanged);
			// 
			// chkTWA
			// 
			this.chkTWA.Checked = true;
			this.chkTWA.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTWA.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.chkTWA.Location = new System.Drawing.Point(8, 48);
			this.chkTWA.Name = "chkTWA";
			this.chkTWA.TabIndex = 7;
			this.chkTWA.Text = "TWA";
			this.chkTWA.CheckedChanged += new System.EventHandler(this.chkTWA_CheckedChanged);
			// 
			// chkPPM
			// 
			this.chkPPM.Checked = true;
			this.chkPPM.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkPPM.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.chkPPM.Location = new System.Drawing.Point(8, 24);
			this.chkPPM.Name = "chkPPM";
			this.chkPPM.TabIndex = 6;
			this.chkPPM.Text = "PPM";
			this.chkPPM.CheckedChanged += new System.EventHandler(this.chkPPM_CheckedChanged);
			// 
			// dtStart
			// 
			this.dtStart.CustomFormat = "MM/dd/yyyy   HH:mm";
			this.dtStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtStart.Location = new System.Drawing.Point(216, 40);
			this.dtStart.MinDate = new System.DateTime(2001, 1, 1, 0, 0, 0, 0);
			this.dtStart.Name = "dtStart";
			this.dtStart.Size = new System.Drawing.Size(144, 20);
			this.dtStart.TabIndex = 9;
			this.dtStart.ValueChanged += new System.EventHandler(this.dtStart_ValueChanged);
			// 
			// lblDTStart
			// 
			this.lblDTStart.Location = new System.Drawing.Point(176, 40);
			this.lblDTStart.Name = "lblDTStart";
			this.lblDTStart.Size = new System.Drawing.Size(40, 16);
			this.lblDTStart.TabIndex = 11;
			this.lblDTStart.Text = "Start ";
			this.lblDTStart.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// lbxComPorts
			// 
			this.lbxComPorts.Location = new System.Drawing.Point(8, 8);
			this.lbxComPorts.Name = "lbxComPorts";
			this.lbxComPorts.Size = new System.Drawing.Size(80, 43);
			this.lbxComPorts.TabIndex = 13;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(184, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(176, 32);
			this.label1.TabIndex = 14;
			this.label1.Text = "Filtering Options:";
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.MediumSlateBlue;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(8, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 24);
			this.label2.TabIndex = 15;
			this.label2.Text = "Modules";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(0, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(96, 16);
			this.label3.TabIndex = 16;
			this.label3.Text = "Log Types";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// pnlModuleSelect
			// 
			this.pnlModuleSelect.BackColor = System.Drawing.Color.MediumSlateBlue;
			this.pnlModuleSelect.Controls.Add(this.chkOnboard);
			this.pnlModuleSelect.Controls.Add(this.chkRIR2);
			this.pnlModuleSelect.Controls.Add(this.label2);
			this.pnlModuleSelect.Controls.Add(this.chkRIR1);
			this.pnlModuleSelect.Controls.Add(this.chkO2);
			this.pnlModuleSelect.Location = new System.Drawing.Point(192, 88);
			this.pnlModuleSelect.Name = "pnlModuleSelect";
			this.pnlModuleSelect.Size = new System.Drawing.Size(208, 64);
			this.pnlModuleSelect.TabIndex = 17;
			// 
			// pnlLogTypes
			// 
			this.pnlLogTypes.BackColor = System.Drawing.Color.MediumSlateBlue;
			this.pnlLogTypes.Controls.Add(this.chkFaults);
			this.pnlLogTypes.Controls.Add(this.chkPPM);
			this.pnlLogTypes.Controls.Add(this.label3);
			this.pnlLogTypes.Controls.Add(this.chkTWA);
			this.pnlLogTypes.Location = new System.Drawing.Point(40, 56);
			this.pnlLogTypes.Name = "pnlLogTypes";
			this.pnlLogTypes.Size = new System.Drawing.Size(128, 96);
			this.pnlLogTypes.TabIndex = 18;
			// 
			// saveFileDialog1
			// 
			this.saveFileDialog1.CreatePrompt = true;
			this.saveFileDialog1.DefaultExt = "csv";
			this.saveFileDialog1.Filter = "Txt files|*.txt";
			// 
			// btnExportFilteredData
			// 
			this.btnExportFilteredData.BackColor = System.Drawing.Color.Orange;
			this.btnExportFilteredData.Enabled = false;
			this.btnExportFilteredData.Location = new System.Drawing.Point(432, 48);
			this.btnExportFilteredData.Name = "btnExportFilteredData";
			this.btnExportFilteredData.Size = new System.Drawing.Size(128, 40);
			this.btnExportFilteredData.TabIndex = 19;
			this.btnExportFilteredData.Text = "Export Filtered Data";
			this.btnExportFilteredData.Click += new System.EventHandler(this.btnExportFilteredData_Click);
			// 
			// txtVersion
			// 
			this.txtVersion.Location = new System.Drawing.Point(8, 160);
			this.txtVersion.Name = "txtVersion";
			this.txtVersion.ReadOnly = true;
			this.txtVersion.Size = new System.Drawing.Size(568, 20);
			this.txtVersion.TabIndex = 20;
			this.txtVersion.Text = "";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(584, 422);
			this.Controls.Add(this.txtVersion);
			this.Controls.Add(this.btnExportFilteredData);
			this.Controls.Add(this.pnlLogTypes);
			this.Controls.Add(this.pnlModuleSelect);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lbxComPorts);
			this.Controls.Add(this.lblDTStart);
			this.Controls.Add(this.dtStart);
			this.Controls.Add(this.btnConnect);
			this.Controls.Add(this.lbxAllLogs);
			this.Name = "Form1";
			this.Text = "ThermalGas Haloguard Real Time Log Downloader";
			this.pnlModuleSelect.ResumeLayout(false);
			this.pnlLogTypes.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		RTLogCommLayer cl;
		RTLogContainer rtContainer;

		protected void updateConnectButtons(bool bConnected)
		{
			if (bConnected)
			{
				this.btnConnect.Text = "Disconnect";
				this.btnConnect.BackColor = Color.LightCoral;
				// export filtered data only available after at least one connect.
				this.btnExportFilteredData.Enabled = true;
			}
			else
			{
				this.btnConnect.Text = "Connect";
				this.btnConnect.BackColor = Color.LightBlue;
				this.txtVersion.Text = "";
			}
		}

		/// <summary>
		/// Connect to the H3 Unit
		/// </summary>
		protected void Connect()
		{
			if (rtContainer != null )
				rtContainer = null;
			if (cl != null )
				cl.Dispose();
			cl = null;

			// clear all items in the list box
			this.lbxAllLogs.Items.Clear();
			try
			{
				cl = new RTLogCommLayer((ulong)this.lbxComPorts.SelectedIndex + 1);
				rtContainer = new RTLogContainer(5000, cl);
				this.rtContainer.NewFilteredEvent += new H3RealTimeLogger.RTLogContainer.NewFilteredEventHandler(this.FilteredEventHandler);

			}
			catch //(Exception e)
			{
				MessageBox.Show("Could not connect.", "Error");	
				this.updateConnectButtons(false);
				return;
			}
			System.Diagnostics.Debug.WriteLine("Attempting Connect");
			try 
			{
				string myVersion = this.cl.GetVersion();
				this.txtVersion.Text = "Haloguard Version: H3/IR " + myVersion;
			}
			catch ( Exception e )
			{
				System.Diagnostics.Debug.WriteLine("error: " + e.ToString());
				this.Disconnect();
				MessageBox.Show("Error Establishing Communication with the Device", "Error");
				return;
			}

			this.updateConnectButtons(true);
			// update filters
			if ( rtContainer.FilterIncludeFaults )
				this.chkFaults.Checked = true;
			else 
				this.chkFaults.Checked = false;
			
			if ( rtContainer.FilterIncludeO2 )
				this.chkO2.Checked = true;
			else
				this.chkO2.Checked = false;

			if ( rtContainer.FilterIncludeOnboard )
				this.chkOnboard.Checked = true;
			else
				this.chkOnboard.Checked = false;

			if ( rtContainer.FilterIncludePPM )
				this.chkPPM.Checked = true;
			else
				this.chkPPM.Checked = false;
			
			if ( rtContainer.FilterIncludeRIR1 )
				this.chkRIR1.Checked = true;
			else
				this.chkRIR1.Checked = false;

			if ( rtContainer.FilterIncludeRIR2 )
				this.chkRIR2.Checked = true;
			else
				this.chkRIR2.Checked = false;

			if ( rtContainer.FilterIncludeTWA )
				this.chkTWA.Checked = true;
			else
				this.chkTWA.Checked = false;

			this.dtStart.Value = this.rtContainer.FilterTimeBegin;
			//this.dtEnd.Value = this.rtContainer.FilterTimeEnd;
			}

		/// <summary>
		/// Disconnect from the H3 Unit
		/// </summary>
		protected void Disconnect()
		{
			if (cl != null)
				cl.Dispose();
			cl = null;	
		
			this.updateConnectButtons(false);
		}

		public void FilteredEventHandler( RTLogBase rtlb)
		{
			this.lbxAllLogs.Items.Add(rtlb.DisplayString);
			// limit to 512 in this list box
			if (this.lbxAllLogs.Items.Count > 512)
			{
				this.lbxAllLogs.Items.RemoveAt(0);
			}		
			this.lbxAllLogs.SelectedIndex = this.lbxAllLogs.Items.Count-1;			
		}

		/// <summary>
		/// Update the list box with the newly-filtered data
		/// </summary>
		void updateFilteredData()
		{
			this.lbxAllLogs.Items.Clear();
			this.rtContainer.ReFilterLogs();
		}

		private void btnConnect_Click(object sender, System.EventArgs e)
		{
			if (this.cl != null)
				this.Disconnect();
			else
				this.Connect();
		}
	
		#region Filter State Changes

		private void dtStart_ValueChanged(object sender, System.EventArgs e)
		{
			this.rtContainer.FilterTimeBegin = this.dtStart.Value;
			this.updateFilteredData();
		}

//		private void dtEnd_ValueChanged(object sender, System.EventArgs e)
//		{
//			this.rtContainer.FilterTimeEnd = this.dtEnd.Value;
//			this.updateFilteredData();
//		}

		private void chkPPM_CheckedChanged(object sender, System.EventArgs e)
		{
			this.rtContainer.FilterIncludePPM = this.chkPPM.Checked;		
			this.updateFilteredData();
		}

		private void chkTWA_CheckedChanged(object sender, System.EventArgs e)
		{
			this.rtContainer.FilterIncludeTWA = this.chkTWA.Checked;
			this.updateFilteredData();
		}

		private void chkFaults_CheckedChanged(object sender, System.EventArgs e)
		{
			this.rtContainer.FilterIncludeFaults = this.chkFaults.Checked;
			this.updateFilteredData();	
		}

		private void chkOnboard_CheckedChanged(object sender, System.EventArgs e)
		{
			this.rtContainer.FilterIncludeOnboard = this.chkOnboard.Checked;
			this.updateFilteredData();
		}

		private void chkO2_CheckedChanged(object sender, System.EventArgs e)
		{
			this.rtContainer.FilterIncludeO2 = this.chkO2.Checked;
			this.updateFilteredData();
		}

		private void chkRIR1_CheckedChanged(object sender, System.EventArgs e)
		{
			this.rtContainer.FilterIncludeRIR1 = this.chkRIR1.Checked;
			this.updateFilteredData();
		}

		private void chkRIR2_CheckedChanged(object sender, System.EventArgs e)
		{
			this.rtContainer.FilterIncludeRIR2 = this.chkRIR2.Checked;
			this.updateFilteredData();
		}
		#endregion

		private void btnExportFilteredData_Click(object sender, System.EventArgs e)
		{
			if (rtContainer == null)
				return;

			DialogResult dr = this.saveFileDialog1.ShowDialog();

			// if not OK, then return
			if ( dr != DialogResult.OK )
			{
				return;
			}

			try 
			{
				this.rtContainer.ExportLogs(true, this.saveFileDialog1.FileName);
			}
			catch (Exception ex )
			{
				MessageBox.Show(ex.ToString(), "Error Saving File");	

			}
		}
	}
}
