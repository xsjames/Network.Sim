﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network.Sim.Core;

namespace Network.Sim.Network.Ip.Routing {
	public class RoutingTable : List<Route> {

		public RoutingTable()
			: base() {
		}

		public RoutingTable(IEnumerable<Route> collection)
			: base(collection) {
		}

		public void Add(IpAddress destination, IpAddress netmask,
			IpAddress gateway, Interface @interface, int metric) {
			Add(new Route(destination, netmask, gateway, @interface,
				metric));
		}
	}
}