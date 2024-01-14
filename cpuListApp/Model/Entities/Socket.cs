using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpuListApp.Model.Entities
{
    public class Socket
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<CPU> CPUs { get; set; }
        public Socket()
        {
            CPUs = new List<CPU>();
        }
    }
}
