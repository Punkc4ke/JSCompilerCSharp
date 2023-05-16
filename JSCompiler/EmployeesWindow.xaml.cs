using System;
using System.Collections.Generic;
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
using System.ComponentModel;
using System.Data.SqlClient;
using System.Configuration;

namespace JSCompiler
{
    /// <summary>
    /// Логика взаимодействия для EmployeesWindow.xaml
    /// </summary>
    public partial class EmployeesWindow : Window
    {
        public string email;
        User1[] users = ExecuteSql("SELECT * FROM [dbo].[Employee]").ToArray();

        public EmployeesWindow()
        {
            InitializeComponent();
            listview.ItemsSource = users;
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (RoleCB.Text != "Все роли") listview.ItemsSource = users.Where(p => p.Post_ID.Contains(RoleCB.Text)).ToList();
            else listview.ItemsSource = users;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listview.ItemsSource);
            if (SortCB.Text == "Имя")
            {
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            }
            else if (SortCB.Text == "Фамилия")
            {
                view.SortDescriptions.Add(new SortDescription("Surname", ListSortDirection.Ascending));
            }
            else if (SortCB.Text == "Отчество")
            {
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new SortDescription("Patronymic", ListSortDirection.Ascending));
            }
            else if (SortCB.Text == "Должность")
            {
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new SortDescription("Post_ID", ListSortDirection.Ascending));
            }
            else if (SortCB.Text == "-") view.SortDescriptions.Clear();

            if (Poisk.Text.Trim() != "") listview.ItemsSource = users.Where(p => p.Name.Contains(Poisk.Text)).ToList();
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

        static IEnumerable<User1> ExecuteSql(string sql)
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
                        if (read["Post_ID"].ToString() == "1") role = "Директор";
                        else if (read["Post_ID"].ToString() == "2") role = "Программист";
                        User1 user = new User1()
                        {
                            Id = (int)read["ID_Employee"],
                            Name = (string)read["Name"],
                            Surname = (string)read["Surname"],
                            Patronymic = (string)read["Patronymic"],
                            Post_ID = role
                        };

                        yield return user;
                    }
                }
                conn.Close();
            }
        }
    }

    public class User1
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Post_ID { get; set; }
    }
}

