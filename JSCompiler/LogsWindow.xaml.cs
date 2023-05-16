using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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

namespace JSCompiler
{
    /// <summary>
    /// Логика взаимодействия для LogsWindow.xaml
    /// </summary>
    public partial class LogsWindow : Window
    {
        public string email;
        Log[] logs = ExecuteSql("SELECT * FROM [dbo].[ExecutedScript]").ToArray();

        public LogsWindow()
        {
            InitializeComponent();
            listview.ItemsSource = logs;
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listview.ItemsSource);
            if (SortCB.Text == "Путь")
            {
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new SortDescription("Path", ListSortDirection.Ascending));
            }
            else if (SortCB.Text == "Код")
            {
                view.SortDescriptions.Add(new SortDescription("Code", ListSortDirection.Ascending));
            }
            else if (SortCB.Text == "Результат")
            {
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new SortDescription("Result", ListSortDirection.Ascending));
            }
            else if (SortCB.Text == "Время выполнения")
            {
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new SortDescription("Execution_Time", ListSortDirection.Ascending));
            }
            else if (SortCB.Text == "-") view.SortDescriptions.Clear();

            if (Poisk.Text.Trim() != "") listview.ItemsSource = logs.Where(p => p.Path.Contains(Poisk.Text)).ToList();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow window = new AdminWindow();
            window.Show();
            this.Close();
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            new AuthWindow().Show();
            this.Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddEmployeeWindow window = new AddEmployeeWindow();
            window.Show();
            this.Close();
        }

        private void SelectCurrentItem(object sender, KeyboardFocusChangedEventArgs e)
        {
            ListViewItem item = (ListViewItem)sender;
            item.IsSelected = true;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            User1 user = listview.Items.GetItemAt(listview.SelectedIndex) as User1;
            EditEmployeeWindow window = new EditEmployeeWindow(user.Id);
            window.Show();
            this.Close();
        }

        class ConnectString
        {
            public string connectionString = ConfigurationManager.ConnectionStrings["JSCompiler.Properties.Settings.CompilerDBConnectionString"].ConnectionString;
        }

        static IEnumerable<Log> ExecuteSql(string sql)
        {

            ConnectString cs = new ConnectString();

            SqlConnection conn = new SqlConnection(cs.connectionString);

            using (conn)
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader read = cmd.ExecuteReader();

                string role = null;
                using (read)
                {
                    while (true)
                    {
                        if (read.Read() == false) break;
                        //if (read["Post_ID"].ToString() == "1") role = "Директор";
                        //else if (read["Post_ID"].ToString() == "2") role = "Программист";
                        Log log = new Log()
                        {
                            Path = (string)read["Path"],
                            Code = (string)read["Code"],
                            Result = (string)read["Result"],
                            Execution_Time = (DateTime)read["Execution_Time"],
                            //Post_ID = role;
                        };

                        yield return log;
                    }
                }
                conn.Close();
            }
        }
    }

    public class Log
    {
        public string Path { get; set; }
        public string Code { get; set; }
        public string Result { get; set; }
        public DateTime Execution_Time { get; set; }

        public string Employee_ID { get; set; }
    }
}
