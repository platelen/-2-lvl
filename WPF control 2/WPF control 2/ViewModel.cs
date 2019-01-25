using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace WPF_control_2
{
    class ViewModel:INotifyPropertyChanged
    {
        //ApplicationContext db;
        
       
        static Random rnd = new Random();
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        
        private void LoadData()
        {
            //var connectionString = @"Data Source=(localdb)\MSSQLLocalDB;
            //                            Initial Catalog=WPF_control_base 3;
            //                            Integrated Security=True;Pooling=False";
            //SqlConnection connection = new SqlConnection(connectionString);
            //SqlDataAdapter adapter = new SqlDataAdapter();
            //SqlCommand command = new SqlCommand(
            //    "SELECT ID, Name, LastName, Age FROM MyBase",
            //    connection);
            //adapter.SelectCommand = command;
            //DataTable dt = new DataTable();
            //adapter.Fill(dt);
            ListBoxSourse = Emploees.ToList().Where(emp => emp.Depart == Selecteddepart?.ID).ToList();
            OnPropertyChanged(nameof(ListBoxSourse));
        }
        private Department selectedDepartment;
        private Emploee selectedEmploees;
        public ObservableCollection<Department> Departments { get; set; }
        public ObservableCollection<Emploee> Emploees { get; set; }
        public IEnumerable<Emploee> ListBoxSourse { get; set; }

        /// <summary>
        /// Команда на добавление объекта.
        /// </summary>
        public RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                    (addCommand = new RelayCommand(obj =>
                    {
                        Emploee empl = new Emploee();
                        Emploees.Insert(0, empl);
                        SelectedEmploee = empl;
                        LoadData();

                    }));
            }
        }
        public RelayCommand addCommandDep;
        public RelayCommand AddCommandDep
        {
            get
            {
                return addCommandDep ??
                    (addCommandDep = new RelayCommand(obj =>
                    {
                        Department dep = new Department();
                        Departments.Insert(0, dep);
                        Selecteddepart = dep;
                        LoadData();

                    }));
            }
        }

        /// <summary>
        /// Команда удаления объекта.
        /// </summary>

        private RelayCommand removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ??
                  (removeCommand = new RelayCommand(obj =>
                  {
                      Emploee empl = obj as Emploee;
                      if (empl != null)
                      {
                          Emploees.Remove(empl);
                          LoadData();
                      }
                  },
                 (obj) => Emploees.Count > 0));
            }
        }
        /// <summary>
        /// Удаление департамента
        /// </summary>
        private RelayCommand removeCommandDep;
        public RelayCommand RemoveCommandDep
        {
            get
            {
                return removeCommandDep ??
                  (removeCommandDep = new RelayCommand(obj =>
                  {
                      Department dep = obj as Department;
                      if (dep != null)
                      {
                          Departments.Remove(dep);
                      }
                  },
                 (obj) => Departments.Count > 0));
            }
        }


        public Department Selecteddepart
        {
            get => selectedDepartment;
            set
            {
                selectedDepartment = value;
                OnPropertyChanged("Selecteddepart");
                LoadData();
            }

        }

        public Emploee SelectedEmploee
        {
            get => selectedEmploees;
            set
            {
                selectedEmploees = value;
                OnPropertyChanged("SelectedEmploee");
            }
        }

        

        /// <summary>
        /// Задаём начальные данные
        /// </summary>
        /// <param name="CountDepartment">Количество департаментов</param>
        /// <param name="CountEmploee">Количество сотрудников</param>
        public ViewModel(int CountDepartment, int CountEmploee)
        {
            Departments = new ObservableCollection<Department>();
            for (int i = 0; i < CountDepartment; i++)
            {
                Departments.Add(new Department { Name = $"Департамент {i + 1 }", ID = i });
            }

            Emploees = new ObservableCollection<Emploee>();
            for (int i = 0; i < CountEmploee; i++)
            {
                var user = new Emploee
                {
                    Name = $"Имя_{i + 1}",
                    LastName = $"Фамилия_{i + 1}",
                    Age =rnd.Next(20, 50)
                };
                Emploees.Add(user);
                var sql = String.Format("INSERT INTO MyBase(Name, LastName, Age) " +
                                        $"VALUES (N'{0}', N'{1}', N'{2}')",
                                        user.Name,
                                        user.LastName,
                                        user.Age);
                //Emploees.Add(new Emploee { Name = $"Имя", LastName = "Фамилия", Age = rnd.Next(18, 40), Depart = rnd.Next(Departments.Count) });
                //var sql = String.Format("INCERT INTO MyBase (Name,LastName,Age)" +  ($"VALUES (N'{sql.n}', N'{1}', N'{2}')");

            }


        }
    }
}
