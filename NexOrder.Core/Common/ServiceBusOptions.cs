using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NexOrder.Framework.Core.Common
{
    public class ServiceBusOptions
    {
        public string ServiceBusConnectionString { get; set; } = string.Empty;

        public string WebProxyAddress { get; set; } = string.Empty;

        public void CheckConfiguration()
        {
            if (string.IsNullOrEmpty(this.ServiceBusConnectionString))
            {
                throw new Exception("ServiceBusConnectionString is null");
            }
        }
    }
}
