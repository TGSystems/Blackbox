using System;

namespace H3RealTimeLogger
{
	/// <summary>
	/// packet class representing packet that will be sent across the wire to the H3
	/// </summary>
	public class H3PacketRT
	{
		/// <summary>
		/// Constructor given a command
		/// </summary>
		/// <param name="cmd"></param>
		public H3PacketRT(eCmdType cmd)
		{
			this.CmdType = cmd;
			this.Data = null;
		}

		/// <summary>
		/// Constructor given a byte stream
		/// </summary>
		/// <param name="byteStream">byte stream coming in from over the datalink</param>
		public H3PacketRT(Byte [] byteStream, int length)
		{
			if ( byteStream.Length < 4 || byteStream.Length > MaxDataLength + 4 )
				throw new InvalidOperationException("Invalid byte Stream length");

			if ( byteStream[0] != PacketStartByte )
				throw new InvalidOperationException("Start Byte invalid");

			// copy the cmd type
			cmdType = (eCmdType) byteStream[1];
			if ( cmdType >= eCmdType.RTLOG_CMD_MAX)
				throw new InvalidOperationException("Invalid command");

			// test the data length
			if (byteStream[2] > MaxDataLength)
				throw new InvalidOperationException("Invalid data length");

			// make sure total bytes given to constructor matches the payload information
			if (length != 4 + byteStream[2])
				throw new InvalidOperationException("Length Mismatch");

			// do the data copy
			if ( byteStream[2] != 0 )
			{
				this.data = new Byte[byteStream[2]];
				// copy the data
				int i;
				for (i=0; i < data.Length; i++)
				{
					this.data[i] = byteStream[3+i];					
				}
			}

			// validate the checksum
			if ( this.GenerateChecksum() != byteStream[length-1] )
			{
				throw new InvalidOperationException("Invalid Checksum");
			}

		}

		/// <summary>
		/// Convert this object to a bytestream to be sent over the data link
		/// </summary>
		/// <returns></returns>
		public Byte[] ToBytes()
		{
			Byte [] bytestream;

			if ( data != null )
				bytestream = new byte[4 + data.Length];
			else
				bytestream = new byte[4];

			bytestream[0] = PacketStartByte;
			bytestream[1] = (Byte) this.cmdType;
				
			// do data area
			if ( data == null)
				bytestream[2] = 0;
			else
			{
				bytestream[2] = (byte) data.Length;
				for (int i=0; i< data.Length; i++)
				{
					bytestream[3+i] = data[i];
				}
			}

			// do checksum
			bytestream[bytestream.Length-1] = GenerateChecksum();

			return bytestream;
		}

		/// <summary>
		///  all packets must start with the start byte
		/// </summary>
		const Byte PacketStartByte = 0xe7;
		/// <summary>
		/// max data length for a given packet
		/// </summary>
		const int MaxDataLength = 32;

		/// <summary>
		/// Command Types
		/// </summary>
		// commands for PC->H3 link
		public enum eCmdType
		{
			// get version of software
			RTLOG_CMD_GETVERSION=0,
		
			// do not put any commands past RTLOG_MAX
			RTLOG_CMD_MAX
		};
		/// <summary>
		/// command type member
		/// </summary>
		protected eCmdType cmdType;
		/// <summary>
		/// data array member
		/// </summary>
		protected Byte [] data;

		/// <summary>
		/// Command Type to send or was received from the datalink
		/// </summary>
		public eCmdType CmdType
		{
			set
			{
				if ( value >= eCmdType.RTLOG_CMD_MAX )
					throw new InvalidOperationException("Illegal Command");
				this.cmdType = value;
			}
			get
			{
				return cmdType;
			}
		}
		/// <summary>
		/// Data Associated with a Data link packet, if any
		/// </summary>
		public Byte [] Data
		{
			set 
			{
				if ( value != null && value.Length > MaxDataLength )
					throw new InvalidOperationException("Too much data");
					
				data = value;
			}
			get 
			{
				// return the data
				return data;
			}
		}

		/// <summary>
		/// generate a checksum describing the data here
		/// </summary>
		/// <returns></returns>
		protected Byte GenerateChecksum()
		{
			Byte tmp;

			tmp = (Byte) (PacketStartByte + (Byte) this.cmdType);
			if ( data != null)
			{
				tmp += (Byte) data.Length;

				foreach(Byte b in data)
					tmp += b;
			}


			return (byte) (0-tmp);
		}	
	}
}
