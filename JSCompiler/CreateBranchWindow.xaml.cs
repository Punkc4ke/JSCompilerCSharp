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

namespace JSCompiler
{
    /// <summary>
    /// Логика взаимодействия для CreateBranchWindow.xaml
    /// </summary>
    public partial class CreateBranchWindow : Window
    {
        public CreateBranchWindow()
        {
            InitializeComponent();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (branchNameBox.Text != null && branchNameBox.Text.Trim() != "")
            {
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Введите название для новой ветки");
            }  
        }

        public string BranchName
        {
            get { return branchNameBox.Text; }
        }
    }
}
