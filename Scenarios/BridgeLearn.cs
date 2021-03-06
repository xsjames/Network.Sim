﻿using System;
using Network.Sim.Core;
using Network.Sim.Lan.Ethernet;
using Network.Sim.Link;
using Network.Sim.Network.Ip;

namespace Network.Sim.Scenarios {
	/// <summary>
	/// Demonstrates the learning algorithm of an Ethernet bridge.
	/// </summary>
	public class BridgeLearn : Scenario {
        /// <summary>
        /// The friendly name of the scenario.
        /// </summary>
	    public override string Name {
            get {
                return "Learning Bridge Algorithm";
            }
        }

        /// <summary>
        /// A description of the scenario.
        /// </summary>
	    public override string Description {
            get {
                return string.Join(Environment.NewLine,
                    "Demonstrates the learning algorithm of an Ethernet bridge.",
                    "",
                    "3 stations are attached to an Ethernet bridge with station A on",
                    "port 0, station B on port 1 and station C on port 2. Station A",
                    "transmits a packet destined for station C."
                );
            }
        }

        /// <summary>
        /// Runs the BridgeLearn scenario.
        /// </summary>
		public override void Run() {
			var bridge = new Bridge(numPorts: 4, delay: 200);
			var H1 = new Host("A");
			H1.RegisterInterface(new Interface(new Nic(
				new MacAddress("AA:AA:AA:AA:AA:AA")), "eth0", "192.168.1.2/24",
				"192.168.1.1"));
			var H2 = new Host("B");
			H2.RegisterInterface(new Interface(new Nic(
				new MacAddress("BB:BB:BB:BB:BB:BB")), "eth0", "192.168.1.3/24",
				"192.168.1.1"));
			var H3 = new Host("C");
			H3.RegisterInterface(new Interface(new Nic(
				new MacAddress("CC:CC:CC:CC:CC:CC")), "eth0", "192.168.1.4/24",
				"192.168.1.1"));
			// Attach each station to the bridge using a 10BASE5 cable.
			new C10Base5(250)
				.Pierce(0, H1.Interfaces["eth0"].Nic.Connector)
				.Pierce(250, bridge.Ports[0]);
			new C10Base5(250)
				.Pierce(0, H2.Interfaces["eth0"].Nic.Connector)
				.Pierce(250, bridge.Ports[1]);
			new C10Base5(250)
				.Pierce(0, H3.Interfaces["eth0"].Nic.Connector)
				.Pierce(250, bridge.Ports[2]);

			// Station H1 triggers a transmission at time t = 0ns.
			Simulation.Callback(0, () => {
				H1.Output("eth0", new IpAddress("192.168.1.3"),
					new byte[] { 1, 2, 3, 4 });
			});

			Simulation.AddObject("Bridge", bridge);
			Simulation.AddObject(H1.Hostname, H1);
			Simulation.AddObject(H2.Hostname, H2);
			Simulation.AddObject(H3.Hostname, H3);
			Simulation.Start();
		}
	}
}
