using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для AddEmployeeWindow.xaml
    /// </summary>
    public partial class AddEmployeeWindow : Window
    {
        public AddEmployeeWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow window = new AdminWindow();
            window.Show();
            this.Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Name.Text.Trim() != "" && Surname.Text.Trim() != "" && PasportSeries.Text.Trim() != "" && PassportNumber.Text.Trim() != "" && Phone.Text.Trim() != "" && DateOfBirth.Text.Trim() != "" && Adres.Text.Trim() != "" && Login.Text.Trim() != "" && passwd.Text.Trim() != "" )
                {
                    if (Regex.IsMatch(PasportSeries.Text, @"[0-9]{4}$"))
                    {
                        if (Regex.IsMatch(PassportNumber.Text, @"[0-9]{6}$"))
                        {
                            if (Regex.IsMatch(passwd.Text, @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$"))
                            {
                                new EmployeeTableAdapter().InsertQuery(Name.Text, Surname.Text, Patronymic.Text, PasportSeries.Text, PassportNumber.Text, Phone.Text, DateOfBirth.Text, Adres.Text, Login.Text, passwd.Text, 2, 1);
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
            catch
            {
                
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            new AuthWindow().Show();
            this.Close();
        }
    }
}
