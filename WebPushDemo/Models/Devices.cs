using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebPushDemo.Models
{
    public class Devices
    {
        public int Id { get; set; }
        
        [MaxLength(100)]
        public string Name { get; set; } = default!;
        [MaxLength(512)]
        public string PushEndpoint { get; set; } = default!;
        [MaxLength(512)]
        public string PushP256Dh { get; set; } = default!;
        [MaxLength(512)]
        public string PushAuth { get; set; } = default!;
    }
}
