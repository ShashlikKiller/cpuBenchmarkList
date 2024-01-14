using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpuListApp.Model.Entities
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Brand()
        {
            CPUs = new List<CPU>();
        }
        public ICollection<CPU> CPUs { get; set; }
    }
}
