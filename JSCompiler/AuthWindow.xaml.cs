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
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Threading;

namespace JSCompiler
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
        }

        class ConnectString
        {
            public string connectionString = ConfigurationManager.ConnectionStrings["JSCompiler.Properties.Settings.CompilerDBConnectionString"].ConnectionString;
        }

        ConnectString cs = new ConnectString();

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (Login.Text.Trim() != "" && Password.Password.Trim() != "")
            {
                using (SqlConnection connection = new SqlConnection(cs.connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand($"SELECT * FROM [dbo].[Employee] WHERE Login = '{Login.Text}' AND Password = '{Password.Password}'", connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if(reader["Active_ID"].ToString() == "1")
                        {
                            if (reader["Post_ID"].ToString() == "1")
                            {
                                AdminWindow window = new AdminWindow();
                                window.Show();
                                this.Hide();
                            }
                            if (reader["Post_ID"].ToString() == "2")
                            {
                                MainWindow window = new MainWindow(reader["ID_Employee"].ToString());
                                window.Show();
                                this.Hide();
                            }
                        }
                        else MessageBox.Show("Данный пользователь заблокирован!");
                    }
                }
            }
            else MessageBox.Show("Ошибка. Пустые поля!");
        }
    }
}
