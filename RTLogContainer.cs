using System;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.IO;

namespace H3RealTimeLogger
{
	/// <summary>
	/// Summary description for RTLogContainer.
	/// </summary>
	public class RTLogContainer 
	{
		public ArrayList allLogs, filteredLogs;
		int maxLogs = 1000;

		public RTLogContainer(int maximumLogs, RTLogCommLayer commLayer)
		{
			// set the maximum logs
			if (maximumLogs > 10)
				this.maxLogs = maximumLogs;

			// create the array lists
			this.allLogs = new ArrayList();
			this.filteredLogs = new ArrayList();
	
			// hook the new Log Event from the Comm Layer
			commLayer.NewLogEvent += new RTLogCommLayer.LogEventHandler(this.newLogEvent);
		}

		public delegate void NewFilteredEventHandler( RTLogBase rtlb);
		public event NewFilteredEventHandler NewFilteredEvent;
		
		/// <summary>
		/// Event Handler for a new Real Time Log Event
		/// </summary>
		/// <param name="rtb"></param>
		public void newLogEvent(RTLogBase rtb)
		{
			this.allLogs.Add(rtb);

			// only keep a certain number of logs
			if (this.allLogs.Count >= maxLogs)
			{
				this.allLogs.RemoveRange(0, this.allLogs.Count - maxLogs + 1);
			}
			this.filterLog(rtb);
		}

		/// <summary>
		/// Add an event to the filtered log
		/// </summary>
		/// <param name="rtb"></param>
		protected void newFilteredLogEvent(RTLogBase rtb)
		{
			this.filteredLogs.Add(rtb);
			if (this.filteredLogs.Count >= maxLogs)
			{
				this.filteredLogs.RemoveRange(0, this.filteredLogs.Count - maxLogs + 1);
			}

			if (this.NewFilteredEvent != null )
				this.NewFilteredEvent(rtb);
		}

		/// <summary>
		/// Refilter all logs in the filtered arraylist
		/// </summary>
		public void ReFilterLogs()
		{
			this.filteredLogs = new ArrayList();

			if ( this.allLogs.Count != 0 )
				this.allLogs.Sort();

			foreach(RTLogBase rtb in this.allLogs)
			{
				this.filterLog(rtb);
			}

			if ( this.filteredLogs.Count != 0 )
				this.filteredLogs.Sort();
		}


		DateTime filterDTBegin = DateTime.Now;
		/// <summary>
		/// Filter Date/Time to begin inclusion of the filter
		/// </summary>
		public DateTime FilterTimeBegin 
		{
			get { return this.filterDTBegin; }
			set { this.filterDTBegin = value; }
		}
		DateTime filterDTEnd   = DateTime.MaxValue; //DateTime.Now.AddYears(1);	
		/// <summary>
		/// Filter Date/Time to end includsion in the filter
		/// </summary>
		public DateTime FilterTimeEnd
		{
			get { return this.filterDTEnd; }
			set { this.filterDTEnd = value; }
		}
	

		// which modules to enable the filter for
		int moduleMask = 0xff;

		/// <summary>
		/// Filter boolean that is true if the filter is to show the onboard modules
		/// and false if it is to not include the onboard modules
		/// </summary>
		public bool FilterIncludeOnboard
		{
			get 
			{ 
				if ( (this.moduleMask & 0x1) != 0)
					return true;
				else
					return false;
			}
			set
			{
				if (value)
					this.moduleMask |= 0x01;
				else
					this.moduleMask &= ~0x01;
			}
		}
		
		/// <summary>
		/// Filter boolean that is true if the filter is to include remote IR module 1
		/// and false if it is to exclude remote IR module 1
		/// </summary>
		public bool FilterIncludeRIR1
		{
			get 
			{ 
				if ( (this.moduleMask & 0x2) != 0)
					return true;
				else
					return false;
			}
			set
			{
				if (value)
					this.moduleMask |= 0x02;
				else
					this.moduleMask &= ~0x02;
			}
		}
		
		/// <summary>
		/// Filter boolean that is true if the filter is to include remote IR module 2
		/// and false if it is to exclude remote IR module 2
		/// </summary>
		public bool FilterIncludeRIR2
		{
			get 
			{ 
				if ( (this.moduleMask & 0x4) != 0)
					return true;
				else
					return false;
			}
			set
			{
				if (value)
					this.moduleMask |= 0x04;
				else
					this.moduleMask &= ~0x04;
			}
		}
		
		/// <summary>
		/// Filter boolean that is true if the filter is to include the Oxygen Sensors
		/// and false if it is to exclude the oxygen sensors
		/// </summary>
		public bool FilterIncludeO2
		{
			get 
			{ 
				if ( (this.moduleMask & 0x8) != 0)
					return true;
				else
					return false;
			}
			set
			{
				if (value)
					this.moduleMask |= 0x08;
				else
					this.moduleMask &= ~0x08;
			}
		}
		
		/// <summary>
		/// Filter boolean that is true if the filter is to include PPM Logs
		/// and false if it is to exclude the PPM Logs
		/// </summary>
		public bool FilterIncludePPM
		{
			get 
			{ 
				if ( (this.moduleMask & 0x10) != 0)
					return true;
				else
					return false;
			}
			set
			{
				if (value)
					this.moduleMask |= 0x10;
				else
					this.moduleMask &= ~0x10;
			}
		}
		
		/// <summary>
		/// Filter boolean that is true if the filter is to include TWA Logs
		/// and false if it is to exclude the TWA Logs
		/// </summary>
		public bool FilterIncludeTWA
		{
			get 
			{ 
				if ( (this.moduleMask & 0x20) != 0)
					return true;
				else
					return false;
			}
			set
			{
				if (value)
					this.moduleMask |= 0x20;
				else
					this.moduleMask &= ~0x20;
			}
		}
		
		/// <summary>
		/// Filter boolean that is true if the filter is to include Fault/Alarm Logging
		/// and false if it is to exclude the Fault/Alarm Logging
		/// </summary>
		public bool FilterIncludeFaults
		{
			get 
			{ 
				if ( (this.moduleMask & 0x40) != 0)
					return true;
				else
					return false;
			}
			set
			{
				if (value)
					this.moduleMask |= 0x40;
				else
					this.moduleMask &= ~0x40;
			}
		}
		

	
		/// <summary>
		/// Determine if a given log meets the requirements of the filter and add it
		/// to the filter list.
		/// </summary>
		/// <param name="rtb"></param>
		protected void filterLog( RTLogBase rtb )
		{	
			// check bounds of time range
			if (rtb.TimeOccurred < this.FilterTimeBegin ||
				rtb.TimeOccurred > this.FilterTimeEnd)
			{
				return;
			}
					
			if ( rtb is RTLogPPM )
			{
				// check filter on PPM logs
				if ( this.FilterIncludePPM )
				{
					int port = rtb.PortNumber;
					if ( port <= 8 && !this.FilterIncludeOnboard)
						return;
					if ( port > 8 && port <= 16 && !this.FilterIncludeRIR1 )
						return;
					if ( port > 16 && port <= 24 && !this.FilterIncludeRIR2 )
						return;	
					if ( port > 24 )
						return;
				}
				else
					return;
			}
			else if ( rtb is RTLogAlarm )
			{
				// check filter on alarms
				if (!this.FilterIncludeFaults )
					return;

				int port = rtb.PortNumber;
				if ( port <= 8 && !this.FilterIncludeOnboard)
					return;
				if ( port > 8 && port <= 16 && !this.FilterIncludeRIR1 )
					return;
				if ( port > 16 && port <= 24 && !this.FilterIncludeRIR2 )
					return;	
				if ( port > 24 && !this.FilterIncludeO2)
					return;
			}
			else if ( rtb is RTLogTWA )
			{
				// check filter on TWAs
				int port = rtb.PortNumber;
				if ( !this.FilterIncludeTWA )
					return;
				if ( port <= 8 && !this.FilterIncludeOnboard)
					return;
				if ( port > 8 && port <= 16 && !this.FilterIncludeRIR1 )
					return;
				if ( port > 16 && port <= 24 && !this.FilterIncludeRIR2 )
					return;	
				if ( port > 24 )
					return;
			}
			else if ( rtb is RTLogO2 )
			{
				// check filter on O2's
				if ( !this.FilterIncludeO2 )
					return;
			}
			else if (rtb is RTLogSysStatus )
			{
				// check filter on system status's (onboard, faults)
				if (!this.FilterIncludeFaults )
					return;

				if (!this.FilterIncludeOnboard )
					return;

			}
			else if ( rtb is RTLogRIR )
			{
				// check Filter on Remote IRs
				RTLogRIR logRIR = rtb as RTLogRIR;
				if ( !this.FilterIncludeFaults )
					return;
				if (logRIR.UnitNumber != 1 && logRIR.UnitNumber != 2 )
					return;
				if ( logRIR.UnitNumber == 1 && !this.FilterIncludeRIR1)
					return;
				if ( logRIR.UnitNumber == 2 && !this.FilterIncludeRIR2)
					return;				
			}
			else
			{
				return;
			}

			/// add this filtered log event
			this.newFilteredLogEvent(rtb);
		}

		/// <summary>
		/// Export Logs to a file
		/// optionally will export filtered logs or all logs
		/// </summary>
		/// <param name="filteredLogs"></param>
		/// <param name="filename"></param>
		public void ExportLogs(bool filteredLogs, string filename)
		{
			StreamWriter csvFile = new StreamWriter(filename);

			ArrayList sourceList;

			if ( filteredLogs )
				sourceList = this.filteredLogs;
			else
				sourceList = this.allLogs;

			// sort the logs
			if ( sourceList.Count != 0 )
				sourceList.Sort();

			for(int i=0; i < sourceList.Count; i++)
			{
				RTLogBase rtb = (RTLogBase) sourceList[i];

				csvFile.WriteLine("{0},{1},{2}", rtb.TimeOccurred.ToShortDateString(), 
					rtb.TimeOccurred.ToLongTimeString(), rtb.ExportString);
			}

			csvFile.Close();

		}
	}
}
