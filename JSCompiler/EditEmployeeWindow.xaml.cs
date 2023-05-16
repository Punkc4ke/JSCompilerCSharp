using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для EditEmployeeWindow.xaml
    /// </summary>
    public partial class EditEmployeeWindow : Window
    {
        class ConnectString
        {
            public string connectionString = ConfigurationManager.ConnectionStrings["JSCompiler.Properties.Settings.CompilerDBConnectionString"].ConnectionString;
        }

        ConnectString cs = new ConnectString();

        public EditEmployeeWindow(int User_Id)
        {
            InitializeComponent();

            idTextBlock.Text = User_Id.ToString();

            SqlConnection conn = new SqlConnection(cs.connectionString);
            using (conn)
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM [dbo].[Employee]", conn);
                SqlDataReader read = cmd.ExecuteReader();
                using (read)
                {
                    while (true)
                    {
                        if (read.Read() == false) break;
                        if (read["ID_Employee"].ToString() == User_Id.ToString())
                        {
                            Surname.Text = read["Surname"].ToString();
                            Name.Text = read["Name"].ToString();
                            Patronymic.Text = read["Patronymic"].ToString();
                            PasportSeries.Text = read["Pasport_Series"].ToString();
                            PassportNumber.Text = read["Passport_Number"].ToString();
                            Phone.Text = read["Telephone"].ToString();
                            DateOfBirth.Text = read["Date_Of_Birth"].ToString();
                            Adres.Text = read["Adress"].ToString();
                            Login.Text = read["Login"].ToString();
                            passwd.Text = read["Password"].ToString();

                            break;
                        }
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EmployeesWindow window = new EmployeesWindow();
            window.Show();
            this.Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            new AuthWindow().Show();
            this.Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (Name.Text.Trim() != "" && Surname.Text.Trim() != "" && PasportSeries.Text.Trim() != "" && PassportNumber.Text.Trim() != "" && Phone.Text.Trim() != "" && DateOfBirth.Text.Trim() != "" && Adres.Text.Trim() != "" && Login.Text.Trim() != "" && passwd.Text.Trim() != "")
            {
                if (Regex.IsMatch(PasportSeries.Text, @"[0-9]{4}$"))
                {
                    if (Regex.IsMatch(PassportNumber.Text, @"[0-9]{6}$"))
                    {
                        if (Regex.IsMatch(passwd.Text, @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$"))
                        {
                            new EmployeeTableAdapter().UpdateQuery(Name.Text, Surname.Text, Patronymic.Text, PasportSeries.Text, PassportNumber.Text, Phone.Text, DateOfBirth.Text, Adres.Text, Login.Text, passwd.Text, 2, 1, Convert.ToInt32(idTextBlock.Text));
                            EmployeesWindow window = new EmployeesWindow();
                            window.Show();
                            this.Close();
                        }
                        else MessageBox.Show("Пароль должен соответствовать следующим требованиям: минимум 6 символов, 1 прописная буква, минимум 1 цифра, по крайней мере один спец.символ!");
                    }
                    else MessageBox.Show("Введите корректный номер паспорта!");
                }
                else MessageBox.Show("Введите корректную серию паспорта!");
            }
            else MessageBox.Show("Заполните пожалуйста все поля!");
        }
    }
}
