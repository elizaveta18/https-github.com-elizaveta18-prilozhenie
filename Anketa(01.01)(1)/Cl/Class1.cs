using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anketa_01._01__1_.Cl
{
    internal class Class1 : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public double maxX { get; set; } = 0;

        public double maxY { get; set; } = 0;

        public void Raise()
        {
            // PropertyChanged(this, new PropertyChangedEventArgs("maxX"));
            // PropertyChanged(this, new PropertyChangedEventArgs("maxY"));
        }
    }
}
