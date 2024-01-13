using cpuListApp.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpuListApp.ViewModel
{
    public class appWindowVM : basicVM
    {
        private List<CPU> GetCPUsFromDB()
        {
            List<CPU> cpus = new List<CPU>();
            using (var db = new cpuListContext())
            {
                cpus = db.CPUs.OrderBy(x => x.Rank).ToList();
            }
            return cpus;
        }
    }
}
