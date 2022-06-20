using System;
using System.Diagnostics;

namespace H3RealTimeLogger
{
	public class RTLogBase : IComparable
	{
		/// <summary>
		/// Method to parse and instantiate the proper RTLogBase object 
		/// </summary>
		/// <param name="logString">string from the data line</param>
		/// <returns>RTLogBase or derived object or null if failure</returns>	
		public static RTLogBase CreateRTLog(string logString)
		{
			string [] tokens = null;

			tokens = logString.Split(":".ToCharArray(), 6);

			if (tokens == null || tokens.Length < 1)
				return null;

			if (tokens[0].CompareTo("PPM") == 0)
			{
				Debug.WriteLine("PPM Data");
				return new RTLogPPM(tokens);
			}
			else if (tokens[0].CompareTo("TWA") == 0)
			{
				Debug.WriteLine("TWA Data");
				return new RTLogTWA(tokens);
			}
			else if (tokens[0].CompareTo("RIR") == 0)
			{			
				Debug.WriteLine("RIR Data");
				return new RTLogRIR(tokens);
			}
			else if (tokens[0].CompareTo("ALM") == 0)
			{			
				Debug.WriteLine("ALM Data");
				return new RTLogAlarm(tokens);
			}
			else if (tokens[0].CompareTo("SS") == 0)
			{			
				Debug.WriteLine("SS Data");
				return new RTLogSysStatus(tokens);
			}
			else if ( tokens[0].CompareTo("O2") == 0)
			{
				Debug.WriteLine("O2 Data");
				return new RTLogO2(tokens);
			}
	
			Debug.WriteLine("Unknown Data");
			return new RTLogBase(tokens);
		}


		/// <summary>
		/// RT Log Base Logger
		/// </summary>
		/// <param name="tokens"></param>
		public RTLogBase(string [] tokens) 
		{
			this.dtTimeOccurred = DateTime.Now;
			this.tokens = tokens;
		}
		/// <summary>
		/// tokens for this object
		/// </summary>
		protected string [] tokens;
		/// <summary>
		/// protected date time occurred variable
		/// </summary>
		protected DateTime dtTimeOccurred;
		/// <summary>
		/// Time the Log Occurred
		/// </summary>
		public DateTime TimeOccurred
		{
			get
			{
				return this.dtTimeOccurred;
			}
		}

		/// <summary>
		/// protected version of whether a fault is present with this particular log
		/// </summary>
		protected bool faultPresent = false;
		/// <summary>
		/// Is Fault Present with this particular log
		/// </summary>
		public bool FaultPresent
		{
			get { return faultPresent; }
		}

		/// <summary>
		/// protected variable for port number
		/// </summary>
		protected int portNumber;
		/// <summary>
		/// public variable for port number or 0 for system-wide
		/// </summary>
		public int PortNumber
		{
			get {return portNumber;}
		}

		public override string ToString()
		{
			return base.ToString ();
		}
		/// <summary>
		/// Displayable string for the object
		/// </summary>
		public string DisplayString
		{
			get 
			{
				return String.Format("{0} : {1}", this.TimeOccurred, this.ToString());
			}
		}

		/// <summary>
		/// string used in the export files
		/// </summary>
		virtual public string ExportString
		{
			get 
			{
				return this.ToString();
			}
		}
		#region IComparable Members

		public int CompareTo(object obj)
		{
			RTLogBase otherObj = obj as RTLogBase;
			if ( this.TimeOccurred < otherObj.TimeOccurred )
				return -1;
			if (this.TimeOccurred > otherObj.TimeOccurred )
				return 1;
			return 0;
		}

		#endregion
	}

	/// <summary>
	/// PPM Real Time Log
	/// </summary>
	public class RTLogPPM : RTLogBase
	{
		//  [PPM:<port>:<gas type string>:<PPM>]
		public RTLogPPM(string [] tokens) 
			: base(tokens)
		{
			if (tokens.Length < 4)
				throw new InvalidOperationException("PPM Log requires 4 tokens");

			this.portNumber = Int32.Parse(tokens[1]);
			// parse this to make sure no exception
			int ppm = Int32.Parse(tokens[3]);
		}

		/// <summary>
		/// PPM value for the log
		/// </summary>
		public int PPM 
		{
			get { return Int32.Parse(this.tokens[3]); }
		}

		public string portType
		{
			get { return this.tokens[2]; }
		}

		public override string ToString()
		{
			if (this.portNumber <= 8)
				return String.Format("Onboard Port {0} - {1}     {2} PPM", this.portNumber, this.portType, this.PPM);
			else if (this.portNumber <= 16)
				return String.Format("RIR1 Port {0} - {1}     {2} PPM", this.portNumber-8, this.portType, this.PPM);
			else
				return String.Format("RIR2 Port {0} - {1}     {2} PPM", this.portNumber-16, this.portType, this.PPM);

		}
		public override string ExportString
		{
			get
			{
				if (this.portNumber <= 8)
					return String.Format("Onboard Port {0}, {1}, {2} PPM", this.portNumber, this.portType, this.PPM);
				else if (this.portNumber <= 16)
					return String.Format("RIR1 Port {0}, {1}, {2} PPM", this.portNumber-8, this.portType, this.PPM);
				else
					return String.Format("RIR2 Port {0}, {1}, {2} PPM", this.portNumber-16, this.portType, this.PPM);
			}
		}
	}


	/// <summary>
	/// TWA Real Time Log
	/// </summary>
	public class RTLogTWA : RTLogBase
	{
		//  [TWA:<port>:<gas type string>:<TWA>]
		public RTLogTWA(string [] tokens) 
			: base(tokens)
		{
			if (tokens.Length < 4)
				throw new InvalidOperationException("TWA Log requires 4 tokens");

			this.portNumber = Int32.Parse(tokens[1]);
			// parse this to make sure no exception
			int ppm = Int32.Parse(tokens[3]);
		}

		/// <summary>
		/// TWA value for the log
		/// </summary>
		public int TWA 
		{
			get { return Int32.Parse(this.tokens[3]); }
		}

		public string portType
		{
			get { return this.tokens[2]; }
		}

		public override string ToString()
		{
			if (this.portNumber <= 8)
				return String.Format("Onboard Port {0} - {1}     {2} TWA", this.portNumber, this.portType, this.TWA);
			else if (this.portNumber <= 16)
				return String.Format("RIR1 Port {0} - {1}     {2} TWA", this.portNumber-8, this.portType, this.TWA);
			else 
				return String.Format("RIR2 Port {0} - {1}     {2} TWA", this.portNumber-16, this.portType, this.TWA);
		}

		public override string ExportString
		{
			get
			{
				if (this.portNumber <= 8)
					return String.Format("Onboard Port {0}, {1}, {2} TWA", this.portNumber, this.portType, this.TWA);
				else if (this.portNumber <= 16)
					return String.Format("RIR1 Port {0}, {1}, {2} TWA", this.portNumber-8, this.portType, this.TWA);
				else
					return String.Format("RIR2 Port {0}, {1}, {2} TWA", this.portNumber-16, this.portType, this.TWA);
			}
		}



	}


	/// <summary>
	/// O2 RealTime Log
	/// </summary>
	public class RTLogO2 : RTLogBase
	{
		// [O2:<port>:<O2 value>:<O2 port faults>]
		public RTLogO2(string [] tokens) 
			: base(tokens)
		{
			if (tokens.Length < 4)
				throw new InvalidOperationException("O2 Log requires 4 tokens");

			this.portNumber = Int32.Parse(tokens[1])+24;
			
			// determine the O2Value
			int myo2Val = Int32.Parse(tokens[2]);
			// upper 4 bits is integer part, fractoinal part is lower 4 bits
			float wholepart = (float) (15 + ((myo2Val >> 4) & 0xf));
			float fractionpart = (float) (myo2Val & 0xf) / 10;
			o2Val = wholepart + fractionpart;

			// determine O2 Port Faults
			if ( (Int32.Parse(tokens[3]) & (1 << (this.portNumber-25))) != 0)
			{
				o2Fault = "Discontinuity";
				this.faultPresent = true;
			}
		}

		protected float o2Val;
		protected string o2Fault = "";
		/// <summary>
		/// O2 Percentage value 
		/// </summary>
		public float O2Percentage
		{
			get { return o2Val; }
		}


		public override string ToString()
		{
			if ( !faultPresent )
				return String.Format("Port {0} - O2 - {1}%", this.portNumber, O2Percentage);
			else
				return String.Format("Port {0} - O2 - Discontinuity", this.portNumber);
		}

		public override string ExportString
		{
			get
			{
				if ( !faultPresent )
					return String.Format("Port {0}, O2, {1}%", this.portNumber, O2Percentage);
				else
					return String.Format("Port {0}, O2, Discontinuity", this.portNumber);
			}
		}
	}


	/// <summary>
	/// RIR Real Time Log
	/// </summary>
	public class RTLogRIR : RTLogBase
	{
		// Format: [RIR:<module>:<module status>]
		public RTLogRIR(string [] tokens) 
			: base(tokens)
		{
			if (tokens.Length < 3)
				throw new InvalidOperationException("RIR Log requires 3 tokens");

			// system-wide status (TBD: limit to ports 9-16 or 17-24??)
			this.portNumber = 0;
			
			// determine which faults
			rirFaults = Int32.Parse(tokens[2]);
			if ( (rirFaults & 0x77e) != 0 )
				this.faultPresent = true;
		}

		int rirFaults;

		// list of all faults that may be present
		/// <summary>
		/// Does remote module have a low-alarm fault
		/// </summary>
		public bool LowAlarmFault
		{
			get { return  ((rirFaults & 0x02)) != 0 ? true : false;}
		}
		/// <summary>
		/// Does remote module have a Pressure fault
		/// </summary>		
		public bool PressureFault
		{
			get { return  ((rirFaults & 0x04)) != 0 ? true : false;}
		}
		/// <summary>
		/// Does remote module have a Temperature fault
		/// </summary>
		public bool TemperatureFault
		{
			get { return  ((rirFaults & 0x08)) != 0 ? true : false;}
		}
		/// <summary>
		/// Does remote module have a Lamp fault
		/// </summary>
		public bool LampFault
		{
			get { return  ((rirFaults & 0x10)) != 0 ? true : false;}
		}
		/// <summary>
		/// Does remote module have a chopper fault
		/// </summary>
		public bool ChopperFault
		{
			get { return  ((rirFaults & 0x20)) != 0 ? true : false;}
		}
		/// <summary>
		/// Is Remote Module in setup mode
		/// </summary>
		public bool SetupFault
		{
			get { return  ((rirFaults & 0x40)) != 0 ? true : false;}
		}
		/// <summary>
		/// Is there a discontinuity fault?
		/// </summary>
		public bool DiscontinuityFault
		{
			get { return  ((rirFaults & 0x100)) != 0 ? true : false;}
		}		
		/// <summary>
		/// Is there a Channel Fault?
		/// </summary>
		public bool ChannelFault
		{
			get { return  ((rirFaults & 0x200)) != 0 ? true : false;}
		}
		/// <summary>
		/// Has the Scanner Stopped?
		/// </summary>
		public bool ScannerStoppedFault
		{
			get { return  ((rirFaults & 0x400)) != 0 ? true : false;}
		}

		/// <summary>
		/// Remote IR Module Unit Number for the log
		/// </summary>
		public int UnitNumber
		{
			get { return Int32.Parse(this.tokens[1] +1); }
		}

		public override string ToString()
		{
			string allFaults = "";

			if ( this.faultPresent )
			{
				if (this.ChannelFault)
					allFaults = allFaults.Insert(0, "Channel Error ");
				if (this.ChopperFault)
					allFaults = allFaults.Insert(0, "Chopper ");
				if (this.DiscontinuityFault)
					allFaults = allFaults.Insert(0, "Discontinuity ");
				if (this.LampFault)
					allFaults = allFaults.Insert(0, "Lamp ");			
				if (this.LowAlarmFault)
					allFaults = allFaults.Insert(0, "Low Alarm ");
				if (this.PressureFault)
					allFaults = allFaults.Insert(0, "Pressure ");
				if (this.ScannerStoppedFault)
					allFaults = allFaults.Insert(0, "Scanner Stopped ");
				if (this.SetupFault)
					allFaults = allFaults.Insert(0, "Setup ");
				if ( this.TemperatureFault )
					allFaults = allFaults.Insert(0, "Temperature ");
			}
			else
			{
				allFaults = "None";
			}
			
			return String.Format("RIR{0} Faults: {1}", this.UnitNumber, allFaults);
		}



	}


	/// <summary>
	/// Alarm Real Time Log
	/// </summary>
	public class RTLogAlarm : RTLogBase
	{
		// Format: [ALM:<port>:<port state]
		public RTLogAlarm(string [] tokens) 
			: base(tokens)
		{
			if (tokens.Length < 3)
				throw new InvalidOperationException("Alarm Log requires 3 tokens");

			// port number
			this.portNumber = Int32.Parse(tokens[1]);
			
			// determine which alarms
			alarms = Int32.Parse(tokens[2]);

			if (alarms != 0)
				this.faultPresent = true;
		}

		int alarms;


		// list of all faults that may be present
		/// <summary>
		/// Is Alarm 1 Set?
		/// </summary>
		public bool Alarm1
		{
			get { return  ((alarms & 0x80)) != 0 ? true : false;}
		}
		/// <summary>
		/// Is Alarm 2 Set?
		/// </summary>		
		public bool Alarm2
		{
			get { return  ((alarms & 0x40)) != 0 ? true : false;}
		}
		/// <summary>
		/// Is Alarm 3 Set?
		/// </summary>
		public bool Alarm3
		{
			get { return  ((alarms & 0x20)) != 0 ? true : false;}
		}
		/// <summary>
		/// Is Alarm 4 Set?
		/// </summary>
		public bool Alarm4
		{
			get { return  ((alarms & 0x02)) != 0 ? true : false;}
		}
		/// <summary>
		/// Is Alarm 5 Set?
		/// </summary>
		public bool Alarm5
		{
			get { return  ((alarms & 0x04)) != 0 ? true : false;}
		}
		/// <summary>
		/// Is Alarm 6 Set?
		/// </summary>
		public bool Alarm6
		{
			get { return  ((alarms & 0x08)) != 0 ? true : false;}
		}
		/// <summary>
		/// Is there a Span Fault??
		/// </summary>
		public bool Span
		{
			get { return  ((alarms & 0x01)) != 0 ? true : false;}
		}		
		/// <summary>
		/// Is there a TWA Fault?
		/// </summary>
		public bool TWA
		{
			get { return  ((alarms & 0x10)) != 0 ? true : false;}
		}

		public override string ToString()
		{
			string allFaults = "";

			if ( this.faultPresent )
			{
				if (this.Alarm1)
					allFaults = allFaults.Insert(allFaults.Length, "1 ");
				if (this.Alarm2)
					allFaults = allFaults.Insert(allFaults.Length, "2 ");
				if (this.Alarm3)
					allFaults = allFaults.Insert(allFaults.Length, "3 ");
				if (this.Alarm4)
					allFaults = allFaults.Insert(allFaults.Length, "4 ");			
				if (this.Alarm5)
					allFaults = allFaults.Insert(allFaults.Length, "5 ");
				if (this.Alarm6)
					allFaults = allFaults.Insert(allFaults.Length, "6 ");
				if (this.TWA)
					allFaults = allFaults.Insert(allFaults.Length, "TWA ");
				if (this.Span)
					allFaults = allFaults.Insert(allFaults.Length, "Span ");
			}
			else
			{
				allFaults = "None";
			}
			
			if (this.portNumber <= 8)
				return String.Format("Onboard Port {0} - Alarms: {1}", this.PortNumber, allFaults);
			else if (this.portNumber <= 16)
				return String.Format("RIR1 Port {0} - Alarms: {1}", this.PortNumber-8, allFaults);
			else
				return String.Format("RIR2 Port {0} - Alarms: {1}", this.PortNumber-16, allFaults);

		}

		public override string ExportString
		{
			get
			{
				string allFaults = "";

				if ( this.faultPresent )
				{
					if (this.Alarm1)
						allFaults = allFaults.Insert(allFaults.Length, "1 ");
					if (this.Alarm2)
						allFaults = allFaults.Insert(allFaults.Length, "2 ");
					if (this.Alarm3)
						allFaults = allFaults.Insert(allFaults.Length, "3 ");
					if (this.Alarm4)
						allFaults = allFaults.Insert(allFaults.Length, "4 ");			
					if (this.Alarm5)
						allFaults = allFaults.Insert(allFaults.Length, "5 ");
					if (this.Alarm6)
						allFaults = allFaults.Insert(allFaults.Length, "6 ");
					if (this.TWA)
						allFaults = allFaults.Insert(allFaults.Length, "TWA ");
					if (this.Span)
						allFaults = allFaults.Insert(allFaults.Length, "Span ");
				}
				else
				{
					allFaults = "None";
				}
			
				if (this.portNumber <= 8)
					return String.Format("Onboard Port {0}, Alarms: {1}", this.PortNumber, allFaults);
				else if (this.portNumber <= 16)
					return String.Format("RIR1 Port {0}, Alarms: {1}", this.PortNumber-8, allFaults);
				else
					return String.Format("RIR2 Port {0}, Alarms: {1}", this.PortNumber-16, allFaults);
			}
		}


	}


	/// <summary>
	/// System Status Real Time Log (Onboard 1-8 status)
	/// </summary>
	public class RTLogSysStatus : RTLogBase
	{
		// Format: [SS:<system status>]
		public RTLogSysStatus(string [] tokens) 
			: base(tokens)
		{
			if (tokens.Length < 2)
				throw new InvalidOperationException("SS Log requires 2 tokens");

			// port number
			this.portNumber = 0;
			
			// determine which alarms
			sysStatus = Int32.Parse(tokens[1]);

			if (sysStatus != 0)
				this.faultPresent = true;
		}

		int sysStatus;
		// list of all faults that may be present
		/// <summary>
		/// Is there a lamp fault?
		/// </summary>
		public bool LampFault
		{
			get { return  ((sysStatus & 0x04)) != 0 ? true : false;}
		}
		/// <summary>
		/// Is there a chopper fault?
		/// </summary>		
		public bool ChopperFault
		{
			get { return  ((sysStatus & 0x02)) != 0 ? true : false;}
		}
		/// <summary>
		/// Is there a pump failure?
		/// </summary>
		public bool PumpFault
		{
			get { return  ((sysStatus & 0x08)) != 0 ? true : false;}
		}
		/// <summary>
		/// Is temperature fault?
		/// </summary>
		public bool TempFault
		{
			get { return  ((sysStatus & 0x10)) != 0 ? true : false;}
		}

		public override string ToString()
		{
			string allFaults = "";

			if ( this.faultPresent )
			{
				if (this.LampFault)
					allFaults = allFaults.Insert(0, "Lamp ");
				if (this.ChopperFault)
					allFaults = allFaults.Insert(0, "Chopper ");
				if (this.TempFault)
					allFaults = allFaults.Insert(0, "Temperature ");
				if (this.PumpFault)
					allFaults = allFaults.Insert(0, "Pump ");			
			}
			else
			{
				allFaults = "None";
			}
			
			return String.Format("Onboard - Faults: {0}", allFaults);
		}

	}

	

}
