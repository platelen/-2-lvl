using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_control_2
{
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public DataRow resultRow { get; set; }
        public EditWindow(DataRow dataRow)
        {
            InitializeComponent();
            resultRow = dataRow;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            nameTextBox.Text = resultRow["Name"].ToString();
            lastnameTextBox.Text = resultRow["LastName"].ToString();
            ageTextBox.Text = resultRow["Age"].ToString();
            departTextBox.Text = resultRow["Department"].ToString();
        }
        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            resultRow["Name"] = nameTextBox.Text;
            resultRow["LastName"] = lastnameTextBox.Text;
            resultRow["Age"] = ageTextBox.Text;
            resultRow["Department"] = departTextBox.Text;
            this.DialogResult = true;
        }
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
