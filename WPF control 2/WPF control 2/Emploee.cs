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
   public class Emploee : INotifyPropertyChanged
    {
        string name;
        string lastName;
        int depart; //Добавлено.
        int age;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnpropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        public string Name { get => name; set { name = value; OnpropertyChanged("Name"); } }
        public int Depart{ get => depart; set { depart = value; OnpropertyChanged("Depart"); } }
        public string LastName { get => lastName; set { lastName = value; OnpropertyChanged("LastName"); } }
        public int Age { get => age; set { age = value; OnpropertyChanged("Age"); } }
        public override string ToString() => Name;

    }
}
