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
using System.Configuration;

namespace WPF_control_2
{
    
    class ViewModel:INotifyPropertyChanged
    {

        static Random rnd = new Random();
        private Department selectedDepartment;
        private Emploee selectedEmploees;
        private DataRowView selectedDataRow;
        public DataTable DataTable { get; set; }
        private  static SqlConnection connection;
        private SqlDataAdapter da;
        public ObservableCollection<Department> Departments { get; set; }
        public ObservableCollection<Emploee> Emploees { get; set; }
        public WPF_control_base_3Entities db = new WPF_control_base_3Entities(); //База данных
        public IEnumerable<Emploee> ListBoxSourse { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void LoadData()
        {

            ListBoxSourse = Emploees.ToList().Where(emp => emp.Depart == Selecteddepart?.ID).ToList();
            OnPropertyChanged(nameof(ListBoxSourse));
        }

        public void DataGrid()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"SELECT * FROM [MyBase]";
            cmd.Connection = con;
            da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable = new DataTable("MyBase");
            da.Fill(DataTable);
            cmd = new SqlCommand(@"DELETE FROM [MyBase] WHERE Id=@Id", con);
            da.DeleteCommand = cmd;
            SqlParameter parameter = cmd.Parameters.Add("@Id", SqlDbType.Int, 0, "Id");
            
        }

      

        #region Insert
        public void InsertRow()
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            connection.Open();
            SqlCommand command =
               new SqlCommand("SELECT Id, Name, LastName, Age, Department FROM MyBase",
               connection);
            da.SelectCommand = command;


            command = new SqlCommand(@"INSERT INTO MyBase (Name, LastName, Age, Department) 
                          VALUES (@Name, @LastName, @Age, @Department); SET @ID = @@IDENTITY;",
                          connection);

            command.Parameters.Add("@Name", SqlDbType.NVarChar, -1, "Name");
            command.Parameters.Add("@LastName", SqlDbType.NVarChar, -1, "LastName");
            command.Parameters.Add("@Age", SqlDbType.NVarChar, 58, "Age");
            command.Parameters.Add("@Department", SqlDbType.NVarChar, -1, "Department");

            SqlParameter param = command.Parameters.Add("@Id", SqlDbType.Int, 0, "Id");

            param.Direction = ParameterDirection.Output;

            da.InsertCommand = command;
            da.Update(DataTable);
        }
        #endregion

        #region Update
        public void UpdateRow()
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            connection.Open();

            SqlCommand command = new SqlCommand(@"UPDATE MyBase SET Name = @Name,
            LastName = @LastName, Age = @Age, Department = @Department WHERE Id = @Id", connection);

            command.Parameters.Add("@Name", SqlDbType.NVarChar, -1, "Name");
            command.Parameters.Add("@LastName", SqlDbType.NVarChar, -1, "LastName");
            command.Parameters.Add("@Age", SqlDbType.NVarChar, -1, "Age");
            command.Parameters.Add("@Department", SqlDbType.NVarChar, -1, "Department");
            SqlParameter  param = command.Parameters.Add("@Id", SqlDbType.Int, 0, "Id");

            param.SourceVersion = DataRowVersion.Original;

            da.UpdateCommand = command;
            da.Update(DataTable);
            
        }

        #endregion

        #region Delete
        public void DeleteBase()
        {
            SqlCommand command = new SqlCommand("DELETE FROM MyBase WHERE Id = @Id", connection);
            SqlParameter param = command.Parameters.Add("@Id", SqlDbType.Int, 0, "Id");
            param.SourceVersion = DataRowVersion.Original;
            da.DeleteCommand = command;
            da.Fill(DataTable);

        }

        #endregion

        #region Для базы

        public DataRowView SelectedDataRow
        {
            get => selectedDataRow;
            set
            {
                selectedDataRow = value;
                OnPropertyChanged();
            }

        }


        public RelayCommand addCommandBase;
        public RelayCommand AddCommandBase
        {
            get
            {
                return addCommandBase ??
                    (addCommandBase = new RelayCommand(obj =>
                    {
                        DataRow dataRow = DataTable.NewRow();
                        EditWindow editWindow = new EditWindow(dataRow);
                        editWindow.ShowDialog();
                        if(editWindow.DialogResult.Value)
                        {
                            DataTable.Rows.Add(editWindow.resultRow);
                            InsertRow();

                        }
                        else
                        {
                            dataRow.CancelEdit();
                        }
                    }));
            }
        }


        public RelayCommand updateCommandBase;
        public RelayCommand UpdateCommandBase
        {
            get
            {
                return updateCommandBase ??
                    (updateCommandBase = new RelayCommand(obj =>
                    {
                        DataRowView newRow = SelectedDataRow;
                        newRow.BeginEdit();

                        EditWindow editWindow = new EditWindow(newRow.Row);
                        editWindow.ShowDialog();

                        if (editWindow.DialogResult.HasValue && editWindow.DialogResult.Value)
                        {
                            newRow.EndEdit();
                            UpdateRow();
                            da.Update(DataTable);
                            
                        }
                        else
                        {
                            newRow.CancelEdit();
                        }
                    }));
            }
        }


        public RelayCommand deleteCommandBase;
        public RelayCommand DeleteCommandBase
        {
            get
            {
                return deleteCommandBase ??
                    (deleteCommandBase = new RelayCommand(obj =>
                    {
                        DataRowView newRow = SelectedDataRow;
                        newRow.Row.Delete();
                        da.Update(DataTable);
                    }));
            }
        }


        #endregion
      
        #region Декстопное приложение
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
        /// <param name="Countdb">Количество сотрудников в бд</param>
        public ViewModel(int CountDepartment, int CountEmploee,int Countdb)
        {
            DataGrid();
            Departments = new ObservableCollection<Department>();
            for (int i = 0; i < CountDepartment; i++)
            {
                Departments.Add(new Department { Name = $"Департамент {i + 1 }", ID = i });
            }

            Emploees = new ObservableCollection<Emploee>();
            for (int i = 0; i < CountEmploee; i++)
            {
                Emploees.Add(new Emploee { Name = $"Имя", LastName = "Фамилия", Age = rnd.Next(18, 40), Depart = rnd.Next(Departments.Count) });
            }
            //Добавление в баззу данных

            for (int i = 0; i < Countdb; i++)
            {
                db.MyBase.Add(new MyBase() { Name = "Имя", LastName = "Фамилия", Age = $"{rnd.Next(20, 50)}", Department = $"{i + 1}" });
            }
            db.SaveChanges();
        }
        #endregion
    }
}
