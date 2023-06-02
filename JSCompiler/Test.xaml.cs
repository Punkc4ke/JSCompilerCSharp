using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Shapes;
using JSCompiler.DataSet1TableAdapters;

namespace JSCompiler
{
    /// <summary>
    /// Логика взаимодействия для Test.xaml
    /// </summary>
    public partial class Test : Window
    {
        public Test()
        {
            InitializeComponent();
            EmployeeUpdate();
        }

        private void EmployeeUpdate()
        {
            EmployeeViewTableAdapter adapter = new EmployeeViewTableAdapter();
            DataSet1.EmployeeViewDataTable table = new DataSet1.EmployeeViewDataTable();
            adapter.Fill(table);
            FormsDataGrid.DataSource = table;
        }
    }
}
