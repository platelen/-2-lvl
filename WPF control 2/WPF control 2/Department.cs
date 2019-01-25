using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WPF_control_2
{
   public class Department : INotifyPropertyChanged
    {
        string name;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnpropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public int ID { get; set; }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnpropertyChanged("Name");
            }
        }
        public override string ToString() => Name;
      
    }
}
