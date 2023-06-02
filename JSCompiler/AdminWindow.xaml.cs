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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using JSCompiler.DataSet1TableAdapters;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;

namespace JSCompiler
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public int id;

        public string banId;

        public DataGridView dataGridView = new DataGridView();
        public AdminWindow()
        {
            InitializeComponent();
            EmployeeUpdate();
            ExecutedScriptUpdate();
        }

        private void EmployeeUpdate()
        {
            EmployeeViewTableAdapter adapter = new EmployeeViewTableAdapter();
            DataSet1.EmployeeViewDataTable table = new DataSet1.EmployeeViewDataTable();
            adapter.Fill(table);
            FormsDataGrid.DataSource = table;
        }

        private void ExecutedScriptUpdate()
        {
            ExecutedScriptViewTableAdapter adapter = new ExecutedScriptViewTableAdapter();
            DataSet1.ExecutedScriptViewDataTable table = new DataSet1.ExecutedScriptViewDataTable();
            adapter.Fill(table);
            logsDataGrid.DataSource = table;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            new AuthWindow().Show();
            this.Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddEmployeeWindow window = new AddEmployeeWindow();
            window.ShowDialog();
            if (window.DialogResult == true)
            {
                EmployeeUpdate();
            }
        }

        private void dgvRptTables_CellClick(System.Object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            int index = e.RowIndex;
            FormsDataGrid.Rows[index].Selected = true;
        }

        //private void dgrdResults_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        //{
        //    if (e.Button == System.Windows.Forms.MouseButtons.Right)
        //    {
                
        //    }
        //}

        private void dgrdResults_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            //handle the row selection on right click
            if (e.Button == MouseButtons.Right)
            {
                try
                {
                    FormsDataGrid.CurrentCell = FormsDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    // Can leave these here - doesn't hurt
                    FormsDataGrid.Rows[e.RowIndex].Selected = true;
                    FormsDataGrid.Focus();

                    id = Convert.ToInt32(FormsDataGrid.Rows[e.RowIndex].Cells[0].Value);
                    banId = FormsDataGrid.Rows[e.RowIndex].Cells[12].Value.ToString();

                    ContextMenuStrip contextMenuStrip1 = new ContextMenuStrip();
                    ToolStripMenuItem changeMenuItem = new ToolStripMenuItem("Редактировать");
                    ToolStripMenuItem banMenuItem = new ToolStripMenuItem("Заблокировать");
                    if (banId == "Да")
                    {
                        banMenuItem.Text = "Восстановить";
                    }

                    contextMenuStrip1.Items.AddRange(new[] { changeMenuItem, banMenuItem });
                    FormsDataGrid.ContextMenuStrip = contextMenuStrip1;

                    changeMenuItem.Click += changeMenuItem_Click;
                    banMenuItem.Click += banMenuItem_Click;

                    contextMenuStrip1.Show(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y);
                }
                catch (Exception)
                {

                }
            }
        }

        void changeMenuItem_Click(object sender, EventArgs e)
        {
            EditEmployeeWindow window = new EditEmployeeWindow(id);
            window.ShowDialog();
            if (window.DialogResult == true)
            {
                EmployeeUpdate();
            }
        }
        void banMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (banId == "Да")
                {
                    MessageBoxResult messageBox = System.Windows.MessageBox.Show("Вы уверены?", "Восстановление пользователя", System.Windows.MessageBoxButton.YesNo);
                    if (messageBox == MessageBoxResult.No)
                        return;
                    new EmployeeTableAdapter().BanQuery(1, id);
                    EmployeeUpdate();
                }
                else
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Вы уверены?", "Блокировка пользователя", System.Windows.MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.No)
                        return;
                    new EmployeeTableAdapter().BanQuery(2, id);
                    EmployeeUpdate();
                }
            }
            catch
            {

            }
        }

        private void copyAlltoClipboard()
        {
            dataGridView.SelectAll();
            System.Windows.Forms.DataObject dataObj = dataGridView.GetClipboardContent();
            if (dataObj != null)
                System.Windows.Forms.Clipboard.SetDataObject(dataObj);
        }

        private void AllBorders(Excel.Borders _borders)
        {
            _borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
            _borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
            _borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
            _borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            _borders.Color = Excel.XlRgbColor.rgbBlack;
        }

        private void excelMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (adminTabControl.SelectedIndex == 0)
                {
                    dataGridView = logsDataGrid;
                }
                else
                {
                    dataGridView = FormsDataGrid;
                }

                copyAlltoClipboard();
                Excel.Application xlexcel;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                Excel.Worksheet xlWorkSheet1;
                object misValue = System.Reflection.Missing.Value;
                xlexcel = new Excel.Application();
                xlexcel.Visible = true;
                xlWorkBook = xlexcel.Workbooks.Add(misValue);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlWorkSheet1 = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                Excel.Range CR = (Excel.Range)xlWorkSheet.Cells[2, 1];

                for (int i = 1; i < dataGridView.Columns.Count + 1; i++)
                {
                    xlWorkSheet1.Cells[1, i] = dataGridView.Columns[i - 1].HeaderText;
                    xlWorkSheet.Columns[i].ColumnWidth = 18;
                    for (int j = 1; j < dataGridView.Rows.Count + 1; j++)
                    {
                        AllBorders(xlWorkSheet.Cells[j, i].Borders);
                        xlWorkSheet.Cells[j, i].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    }
                }
                CR.Select();
                xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
            }
            catch
            {

            }
        }

        private void wordMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Filter = "Word Documents (*.docx)|*.docx";

            if (adminTabControl.SelectedIndex == 0)
            {
                dataGridView = logsDataGrid;
                sfd.FileName = "Логи выполенных скриптовt.docx";
                Export_Data_To_Word(dataGridView, sfd.FileName, "Логи выполенных скриптов");
            }
            else
            {
                dataGridView = FormsDataGrid;
                sfd.FileName = "Список сотрудников.docx";
                Export_Data_To_Word(dataGridView, sfd.FileName, "Список сотрудников");
            }
        }
        public void Export_Data_To_Word(DataGridView DGV, string filename, string headerTable)
        {
            try
            {
                if (DGV.Rows.Count != 0)
                {
                    int RowCount = DGV.Rows.Count;
                    int ColumnCount = DGV.Columns.Count;
                    Object[,] DataArray = new object[RowCount + 1, ColumnCount + 1];

                    //add rows
                    int r = 0;
                    for (int c = 0; c <= ColumnCount - 1; c++)
                    {
                        for (r = 0; r <= RowCount - 1; r++)
                        {
                            DataArray[r, c] = DGV.Rows[r].Cells[c].Value;
                        } //end row loop
                    } //end column loop

                    Word.Document oDoc = new Word.Document();
                    oDoc.Application.Visible = true;

                    //page orintation
                    oDoc.PageSetup.Orientation = Word.WdOrientation.wdOrientLandscape;


                    dynamic oRange = oDoc.Content.Application.Selection.Range;
                    string oTemp = "";
                    for (r = 0; r <= RowCount - 1; r++)
                    {
                        for (int c = 0; c <= ColumnCount - 1; c++)
                        {
                            oTemp = oTemp + DataArray[r, c] + "\t";

                        }
                    }

                    //table format
                    oRange.Text = oTemp;

                    object Separator = Word.WdTableFieldSeparator.wdSeparateByTabs;
                    object ApplyBorders = true;
                    object AutoFit = true;
                    object AutoFitBehavior = Word.WdAutoFitBehavior.wdAutoFitContent;

                    oRange.ConvertToTable(ref Separator, ref RowCount, ref ColumnCount,
                                          Type.Missing, Type.Missing, ref ApplyBorders,
                                          Type.Missing, Type.Missing, Type.Missing,
                                          Type.Missing, Type.Missing, Type.Missing,
                                          Type.Missing, ref AutoFit, ref AutoFitBehavior, Type.Missing);

                    oRange.Select();

                    oDoc.Application.Selection.Tables[1].Select();
                    oDoc.Application.Selection.Tables[1].Rows.AllowBreakAcrossPages = 0;
                    oDoc.Application.Selection.Tables[1].Rows.Alignment = 0;
                    oDoc.Application.Selection.Tables[1].Rows[1].Select();
                    oDoc.Application.Selection.InsertRowsAbove(1);
                    oDoc.Application.Selection.Tables[1].Rows[1].Select();

                    //header row style
                    oDoc.Application.Selection.Tables[1].Rows[1].Range.Bold = 1;
                    oDoc.Application.Selection.Tables[1].Rows[1].Range.Font.Name = "Times New Roman";
                    oDoc.Application.Selection.Tables[1].Rows[1].Range.Font.Size = 14;

                    //add header row manually
                    for (int c = 0; c <= ColumnCount - 1; c++)
                    {
                        oDoc.Application.Selection.Tables[1].Cell(1, c + 1).Range.Text = DGV.Columns[c].HeaderText;
                    }

                    //table style 
                    oDoc.Application.Selection.Tables[1].set_Style("Сетка таблицы");
                    oDoc.Application.Selection.Tables[1].Rows[1].Select();
                    oDoc.Application.Selection.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                    //header text
                    foreach (Word.Section section in oDoc.Application.ActiveDocument.Sections)
                    {
                        Word.Range headerRange = section.Headers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                        headerRange.Fields.Add(headerRange, Word.WdFieldType.wdFieldPage);
                        headerRange.Text = headerTable;
                        headerRange.Font.Size = 16;
                        headerRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    }

                    //save the file
                    oDoc.SaveAs2(filename);


                }
            }
            catch { }
            

        }
    }
}
