using cpuListApp.Model.Backend;
using cpuListApp.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpuListApp.ViewModel
{
    public class CpuInfoVM : basicVM
    {
        private CPU currentCPU;

        private int brandImageId;
        public int BrandImageId
        {
            get
            {
                return brandImageId;
            }
            set
            {
                brandImageId = value;
                OnPropertyChanged();
            }
        }

        public CPU CurrentCPU
        {
            get
            {
                return currentCPU;
            }
            set
            {
                currentCPU = value;
                OnPropertyChanged();
            }
        }

        private bool multiplier;
        public int Multiplier
        {
            get
            {
                return Convert.ToInt32(multiplier);
            }
            set
            {
                multiplier = Convert.ToBoolean(value);
                OnPropertyChanged();
            }
        }

        private Socket selectedSocket;

        public Socket SelectedSocket
        {
            get
            {
                return selectedSocket;
            }
            set 
            {
                selectedSocket = value;
                OnPropertyChanged(); 
            }
        }

        private Brand selectedBrand;

        public Brand SelectedBrand
        {
            get
            {
                return selectedBrand;
            }
            set { selectedBrand = value; OnPropertyChanged(); }
        }

        private Command saveChanges;
        public Command SaveChanges
        {
            get
            {
                return saveChanges = new Command(async obj =>
                {
                    SaveChangesInDB();
                });
            }
        }

        private async void SaveChangesInDB()
        {
            using (var db = new cpuListContext())
            {
                CurrentCPU.Multiplier = multiplier;
                CurrentCPU.SocketId = SelectedSocket.Id;
                if (db.CPUs.Any(x=>x.Id == CurrentCPU.Id))
                {
                    db.Entry(CurrentCPU).State = System.Data.Entity.EntityState.Modified;
                    await db.SaveChangesAsync();
                }
            }
        }

        public CpuInfoVM(CPU selectedCPU)
        {
            CurrentCPU = selectedCPU;
            Multiplier = Convert.ToInt32(CurrentCPU.Multiplier);
            BrandImageId = CurrentCPU.BrandId;
            GetSockets();
            GetBrands();
            SelectedSocket = Sockets.Where(c => c.Id == CurrentCPU.SocketId).FirstOrDefault();
            SelectedBrand = Brands.Where(c => c.Id == CurrentCPU.BrandId).FirstOrDefault();
        }

        public List<Socket> Sockets { get; set; }
        private void GetSockets()
        {
            using (var db = new cpuListContext())
            {
                Sockets = db.Sockets.ToList();
            }
        }
        public List<Brand> Brands;
        private void GetBrands()
        {
            using (var db = new cpuListContext())
            {
                Brands = db.Brands.ToList();
            }
        }
    }
}
