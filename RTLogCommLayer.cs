using System;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;

namespace H3RealTimeLogger
{

	/// <summary>
	/// Summary description for RTLogCommLayer.
	/// </summary>
	public class RTLogCommLayer : IDisposable
	{
		/// <summary>
		/// Serial Port object
		/// </summary>
		SerialPort mySerial;
		bool bRunThreads;
		Thread tReadThread;

		public delegate void LogEventHandler( RTLogBase log );
		public event LogEventHandler NewLogEvent;
		/// <summary>
		/// Reply event 
		/// </summary>		
		ManualResetEvent evtReply = new ManualResetEvent(false);	
		byte [] lastReply = null;

		public RTLogCommLayer(ulong ulComPort)
		{
			string strComPort = String.Format("COM{0}", ulComPort);

			// instantiate the serial port.
			//  we are always at 38400, N, 8, 1.
			mySerial = new SerialPort(strComPort, 38400, Parity.None, 8, StopBits.One);

			mySerial.ReadTimeout = 100;
			// open the port
			mySerial.Open();			

			bRunThreads = true;

			// start the read thread up
			tReadThread = new Thread(new ThreadStart(this.ReadThread));
			tReadThread.Start();
		}


		/// <summary>
		/// Send a command to the H3 and wait for a reply
		/// </summary>
		/// <param name="cmd">the command to send to the H3</param>
		/// <returns>the reply that comes back from the H3</returns>
		protected H3PacketRT SendH3CmdWaitReply(H3PacketRT packetCmd)
		{
			// reset the reply event
			this.evtReply.Reset();
			lastReply = null;

			byte [] packetData = packetCmd.ToBytes();
			// send the packet to the device
			mySerial.Write( packetData, 0, packetData.Length );

			// wait for the reply to come back - but if it doesn't return null for no reply
			if (this.evtReply.WaitOne(2500, false) == false)
				throw new InvalidOperationException("The H3 did not respond in a normal period of time");

			// if we didn't get a reply, then stop what we were doing
			if (lastReply == null)
				return null;

			// return the reply
			H3PacketRT replyPacket = new H3PacketRT( lastReply, lastReply.Length );

			this.lastReply = null;
			
			return replyPacket;

		}

		/// <summary>
		/// Return the version of the h3AVR
		/// </summary>
		/// <returns></returns>
		public string GetVersion()
		{
			// generate a packet for the get time command
			H3PacketRT packet = new H3PacketRT(H3PacketRT.eCmdType.RTLOG_CMD_GETVERSION);
			Byte [] packetData = packet.ToBytes();

			// 500 ms timeout for get time
			mySerial.ReadTimeout = 500;

			// send the command, wait for the reply
			H3PacketRT versionResponse = SendH3CmdWaitReply( packet );

			// make sure the response is to this packet
			if ( versionResponse.CmdType != packet.CmdType )
			{
				throw new InvalidOperationException("h3AVR returned illegal response");
			}

			string versionString = System.Text.Encoding.ASCII.GetString(versionResponse.Data);
			return versionString;
		}

		/// <summary>
		/// Read Thread for the Real Time Logger
		/// </summary>
		void ReadThread()
		{
			// byte array of the current log that we will pass on to the next level
			// for further parsing (separated by [ ] )
			byte [] pbyCurrentLogInfo = new byte[100];
			int logPosition = 0;

			// read buffer
			byte [] pbyReadBuff = new byte[256];
			bool bWaitStartDelimiter = true;

			while (bRunThreads)
			{
				// read bytes
				int bytesRead = mySerial.Read( pbyReadBuff, 0, pbyReadBuff.Length );
				if ( bytesRead != 0 )
				{
//					Debug.WriteLine("\r\nRead bytes: " + bytesRead);
					// iterate over the bytes we read						
					for (int i=0; i < bytesRead; i++)
					{ 
						// debug work to display what we see over the serial port on the debug window.  helps in diagnosing comm problems
//						string dbg = System.Text.Encoding.ASCII.GetString(pbyReadBuff, i, 1);
//						if (char.IsLetterOrDigit(dbg, 0) || char.IsSeparator(dbg, 0) || char.IsWhiteSpace(dbg, 0) ||
//							char.IsPunctuation(dbg, 0))
//						{
//							Debug.Write(dbg + " ");
//						}
//						else
//						{
//							Debug.Write(pbyReadBuff[i].ToString("x") + " ");
//						}
						
						// are we looking for the start delimiter?
						if ( bWaitStartDelimiter )
						{
							// did we find it?
							if ( pbyReadBuff[i] == '[')
							{
								// no longer looking for the start delimiter
								bWaitStartDelimiter = false;
								logPosition = 0;								
							}
						}
						else
						{
							// have we found the end delimiter?
							if ( pbyReadBuff[i] != ']')
							{
								// no, copy the data over
								pbyCurrentLogInfo[logPosition++] = pbyReadBuff[i];
								// handle past bounds...
								if ( logPosition >= pbyCurrentLogInfo.Length )
								{
									bWaitStartDelimiter = true;
								}
							}
							else
							{
								// found the end delimter... now we're looking for the next start delimiter
								bWaitStartDelimiter = true;
								// debug what we found
								// Debug.WriteLine(System.Text.Encoding.ASCII.GetString(pbyCurrentLogInfo, 0, logPosition));
								// process it...
								try 
								{
									bool bPCReply = false;
									string pcReplyID = "REPLY:";

									// is the log long enough to be a PC reply
									if ( logPosition > pcReplyID.Length )
									{
										// convert to string
										string pcReplyString = System.Text.Encoding.ASCII.GetString(pbyCurrentLogInfo, 0, pcReplyID.Length);
										// compare it
										if (pcReplyString.CompareTo(pcReplyID) == 0)
										{
											// its a PC reply
											bPCReply = true;
											
											// copy the data to the last data
											this.lastReply = new byte[logPosition - pcReplyID.Length];
											for (int myindex = pcReplyID.Length; myindex < logPosition; myindex++)
											{
												this.lastReply[myindex-pcReplyID.Length] = pbyCurrentLogInfo[myindex];
											}
											

											// trigger the wait
											this.evtReply.Set();
										}
									}

									if (!bPCReply)
									{
										// it's not a reply -- its a realtime log -- simply throw it into the 
										// realtime log class and let it be processed by our system
										RTLogBase rtLog = RTLogBase.CreateRTLog(System.Text.Encoding.ASCII.GetString(pbyCurrentLogInfo, 0, logPosition));

										// notify event handler of new log
										if ( this.NewLogEvent != null )
											this.NewLogEvent(rtLog);
									}
								}
								catch (Exception e)
								{
									Debug.WriteLine("Error Creating Log: " + e.ToString());
								}
							}
						}

					}
				}
			}
		}
		#region IDisposable Members

		public void Dispose()
		{
			// stop the read thread
			this.bRunThreads = false;			
			this.tReadThread.Join();

			try 
			{
				if (this.mySerial != null)
					this.mySerial.Close();
				this.mySerial = null;
			}
			catch 
			{
			}
			//this.mySerial.Dispose();
		}

		#endregion
	}
}
