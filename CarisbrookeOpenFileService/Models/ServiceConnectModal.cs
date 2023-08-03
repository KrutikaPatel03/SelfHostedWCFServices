using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarisbrookeOpenFileService.Models
{
    public class ServiceConnectModal
    {
        public string HostName { get; set; }
        public string HostIP { get; set; }
        public string ServiceEndPoint { get; set; }
        public bool IsServiceHosted { get; set; }
    }

   
}
