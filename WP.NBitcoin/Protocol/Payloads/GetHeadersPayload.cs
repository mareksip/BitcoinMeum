﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBitcoin.Protocol
{
	[Payload("getheaders")]
	public class GetHeadersPayload : Payload
	{
		uint version = (uint)ProtocolVersion.PROTOCOL_VERSION;
		public ProtocolVersion Version
		{
			get
			{
				return (ProtocolVersion)version;
			}
			set
			{
				version = (uint)value;
			}
		}

		BlockLocator blockLocators;

		public BlockLocator BlockLocators
		{
			get
			{
				return blockLocators;
			}
			set
			{
				blockLocators = value;
			}
		}

		uint256 hashStop = new uint256(0);
		public uint256 HashStop
		{
			get
			{
				return hashStop;
			}
			set
			{
				hashStop = value;
			}
		}

		public override void ReadWriteCore(BitcoinStream stream)
		{
			stream.ReadWrite(ref version);
			stream.ReadWrite(ref blockLocators);
			stream.ReadWrite(ref hashStop);
		}
	}
}
