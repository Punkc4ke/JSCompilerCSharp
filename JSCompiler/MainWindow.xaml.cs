using System;
using System.Windows;
using System.Windows.Controls;
using Jint;
using System.IO;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Windows.Input;
using System.Linq;
using LibGit2Sharp;
using LibGit2Sharp.Core;
using LibGit2Sharp.Core.Handles;
using System.Data;
using JSCompiler.DataSet1TableAdapters;
using Microsoft.Win32;
using WinForms = System.Windows.Forms;
using ICSharpCode.AvalonEdit;
using System.Collections.ObjectModel;
using Jint.Parser;
using Jint.Runtime;

namespace JSCompiler
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string id;
        public ObservableCollection<MyBranch> MyBranchesList { get; set; }
        public ObservableCollection<MyCommit> MyCommitsList { get; set; }

        public string folderPath = @"C:\Users\Алексей\Desktop\Скрипты";

        public MainWindow(string id)
        {
            this.id = id;
            InitializeComponent();

            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
            if (directoryInfo.Exists)
            {
                BuildTree(directoryInfo, treeView.Nodes);
            }

            MyBranchesList = new ObservableCollection<MyBranch> { };

            MyCommitsList = new ObservableCollection<MyCommit> { };

            branchListBox.ItemsSource = MyBranchesList;

            commitListBox.ItemsSource = MyCommitsList;

            using (var repo = new Repository(folderPath))
            {
                foreach (Branch branch in repo.Branches)
                {
                    MyBranchesList.Add(new MyBranch { BranchName = branch.FriendlyName });
                }
            }
        }

        private void UpdateExecutedScript()
        {
            ExecutedScriptTableAdapter adapter = new ExecutedScriptTableAdapter();
        }

        private void Execute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DockPanel dock = MyTabControl.SelectedContent as DockPanel;

                TextEditor editor = dock.Children[1] as TextEditor;
                String code = editor.Text;
                String path = null;
                String result;
                Engine jsEngine = new Engine(c => c.DebugMode());

                jsEngine.Execute(code);

                listBox.Items.Add(jsEngine.GetCompletionValue().ToString());
                result = jsEngine.GetCompletionValue().ToString();

                TextBlock textBlock = dock.Children[0] as TextBlock;

                code = code.Replace(Environment.NewLine, " ");

                if (textBlock.Text == null && textBlock.Text.Trim() == "")
                {
                    path = "null";
                }
                else
                {
                    path = textBlock.Text;
                }
                new ExecutedScriptTableAdapter().InsertQuery(path, code, result, DateTime.Now, Convert.ToInt32(this.id));
                UpdateExecutedScript();
            }
            catch (ParserException pEx)
            {
                listBox.Items.Add("Parser Exception: " + pEx.Message);
            }
            catch (JavaScriptException rEx)
            {
                listBox.Items.Add("Runtime Exception: " + rEx.Message);
            }
        }

        private void openFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JavaScirpt files (*.js)|*.js|All files (*.*)|*.*";

            TabItem item = null;
            DockPanel dock = null;
            TextEditor editor = null;
            TextBlock textblock = null;

            try
            {
                textblock = new TextBlock();
                DockPanel.SetDock(textblock, Dock.Top);

                // Creating the TextBox
                editor = new TextEditor();
                editor.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                editor.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                editor.ShowLineNumbers = true;
                editor.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                editor.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinition("JavaScript");
                editor.FontSize = 16;

                if (openFileDialog.ShowDialog() == true)
                {
                    editor.Text = File.ReadAllText(openFileDialog.FileName);
                    textblock.Text = Path.GetFullPath(openFileDialog.FileName);

                    for (int i = 0; i < MyTabControl.Items.Count; i++)
                    {
                        TabItem tabItem = MyTabControl.Items[i] as TabItem;
                        DockPanel dockPanel = tabItem.Content as DockPanel;
                        TextBlock textBlock = dockPanel.Children[0] as TextBlock;

                        if (openFileDialog.FileName == textBlock.Text)
                        {
                            MyTabControl.SelectedItem = tabItem;
                            return;
                        }
                    }
                }

                dock = new DockPanel();
                dock.Children.Add(textblock);
                dock.Children.Add(editor);

                item = new TabItem();
                item.Header = openFileDialog.SafeFileName;
                item.Content = dock;
                MyTabControl.Items.Add(item);
                MyTabControl.SelectedItem = item;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating the TabItem content! " + ex.Message);
            }
            finally
            {
                editor = null;
                item = null;
            }
        }

        private void CloseEditor_Click(object sender, RoutedEventArgs e)
        {
            MyTabControl.Items.RemoveAt(MyTabControl.SelectedIndex);
        }

        private void openFolder_Click(object sender, RoutedEventArgs e)
        {
            WinForms.FolderBrowserDialog openFolderDialog = new WinForms.FolderBrowserDialog();
            if (openFolderDialog.ShowDialog() == WinForms.DialogResult.OK)
            {
                folderPath = openFolderDialog.SelectedPath;
            }
            else
            {
                return;
            }
            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
            if (directoryInfo.Exists)
            {
                treeView.Nodes.Clear();
                BuildTree(directoryInfo, treeView.Nodes);
            }
        }

        private void CloseFolder_Click(object sender, RoutedEventArgs e)
        {
            treeView.Nodes.Clear();
            MyTabControl.Items.Clear();
            listBox.Items.Clear();
            folderPath = null;
        }

        private void newFile_Click(object sender, RoutedEventArgs e)
        {
            TabItem item = null;
            DockPanel dock = null;
            TextEditor editor = null;
            TextBlock textblock = null;

            try
            {
                textblock = new TextBlock();
                DockPanel.SetDock(textblock, Dock.Top);

                // Creating the TextBox
                editor = new TextEditor();
                editor.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                editor.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                editor.ShowLineNumbers = true;
                editor.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                editor.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinition("JavaScript");
                editor.FontSize = 16;

                dock = new DockPanel();
                dock.Children.Add(textblock);
                dock.Children.Add(editor);

                item = new TabItem();
                item.Header = "untitled";
                item.Content = dock;
                MyTabControl.Items.Add(item);
                MyTabControl.SelectedItem = item;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating the TabItem content! " + ex.Message);
            }
            finally
            {
                editor = null;
                item = null;
            }
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DockPanel dock = MyTabControl.SelectedContent as DockPanel;
                TabItem tabItem = MyTabControl.SelectedItem as TabItem;
                TextBlock textBlock = dock.Children[0] as TextBlock;
                TextEditor editor = dock.Children[1] as TextEditor;
                String fileText = editor.Text;

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.DefaultExt = ".js";
                saveFileDialog.Filter = "JavaScirpt files (*.js)|*.js|All files (*.*)|*.*";

                Nullable<bool> result = saveFileDialog.ShowDialog();

                if (result == true)
                {
                    // Save document
                    File.WriteAllText(saveFileDialog.FileName, fileText);
                    tabItem.Header = saveFileDialog.SafeFileName;
                    textBlock.Text = saveFileDialog.FileName;

                    DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
                    if (directoryInfo.Exists)
                    {
                        treeView.Nodes.Clear();
                        BuildTree(directoryInfo, treeView.Nodes);
                    }

                }
            }
            catch { }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DockPanel dock = MyTabControl.SelectedContent as DockPanel;
                TabItem tabItem = MyTabControl.SelectedItem as TabItem;
                TextBlock textBlock = dock.Children[0] as TextBlock;
                TextEditor editor = dock.Children[1] as TextEditor;
                String fileText = editor.Text;
                File.WriteAllText(textBlock.Text, fileText);

                DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
                if (directoryInfo.Exists)
                {
                    treeView.Nodes.Clear();
                    BuildTree(directoryInfo, treeView.Nodes);
                }
            }
            catch { }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            new AuthWindow().Show();
            this.Close();
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BuildTree(DirectoryInfo directoryInfo, WinForms.TreeNodeCollection addInMe)
        {
            WinForms.TreeNode curNode = addInMe.Add(directoryInfo.Name);
            
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                curNode.Nodes.Add(file.FullName, file.Name);
            }

            foreach (DirectoryInfo subdir in directoryInfo.GetDirectories())
            {
                if (subdir.Name == ".git")
                {
                    
                }
                else
                {
                    BuildTree(subdir, curNode.Nodes);
                }  
            }
        }

        private void treeView_AfterSelect(object sender, WinForms.TreeViewEventArgs e)
        {
            TabItem item = null;
            DockPanel dock = null;
            TextEditor editor = null;
            TextBlock textblock = null;

            try
            {
                textblock = new TextBlock();
                textblock.Text = Path.GetFullPath(e.Node.Name);
                DockPanel.SetDock(textblock, Dock.Top);

                // Creating the TextBox
                editor = new TextEditor();
                editor.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                editor.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                editor.ShowLineNumbers = true;
                editor.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                editor.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinition("JavaScript");
                editor.FontSize = 16;
  
                if (e.Node.Name.EndsWith("js"))
                {
                    for (int i = 0; i < MyTabControl.Items.Count; i++)
                    {
                        TabItem tabItem = MyTabControl.Items[i] as TabItem;
                        DockPanel dockPanel = tabItem.Content as DockPanel;
                        TextBlock textBlock = dockPanel.Children[0] as TextBlock;

                        if (Path.GetFullPath(e.Node.Name) == textBlock.Text)
                        {
                            MyTabControl.SelectedItem = tabItem;
                            return;
                        }
                    }
                    StreamReader reader = new StreamReader(e.Node.Name);
                    editor.Text = reader.ReadToEnd();
                    reader.Close();

                    dock = new DockPanel();
                    dock.Children.Add(textblock);
                    dock.Children.Add(editor);

                    item = new TabItem();
                    item.Header = Path.GetFileName(e.Node.Name);
                    item.Content = dock;
                    MyTabControl.Items.Add(item);
                    MyTabControl.SelectedItem = item;
                }
            }
            catch { }
            finally
            {
                editor = null;
                item = null;
                textblock = null;    
            }
        }

        private void branchListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MyBranch myBranch = (MyBranch)branchListBox.SelectedItem;

            MyCommitsList.Clear();

            createBranch.Header = "Create branch from " + myBranch.BranchName;

            using (var repo = new Repository(folderPath))
            {
                foreach (Commit commit in repo.Commits)
                {
                    MyCommitsList.Add(new MyCommit { Author = commit.Author.ToString(), Message = commit.MessageShort, Sha = commit.Sha, When = commit.Committer.When.ToString(), Committer = commit.Committer.ToString(), Email = commit.Committer.Email });
                    commitListBox.Items.Refresh();
                }
            }
        }


        private void commitListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MyCommit myCommit = (MyCommit)commitListBox.SelectedItem;

            using (var repo = new Repository(folderPath))
            {
                if (myCommit != null)
                {
                    messageTextBlock.Text = myCommit.Message;
                    lolTextBlock.Text = myCommit.Committer.ToString() + " on " + myCommit.When.ToString();
                }
                else
                {
                    return;
                }
            }
        }

        public class MyBranch
        {
            public string BranchName { get; set; }
        }

        public class MyCommit
        {
            public string Author { get; set; }

            public string Message { get; set; }

            public string Sha { get; set; }

            public string When { get; set; }

            public string Committer { get; set; }

            public string Email { get; set; }
        }

        private void checkoutBranch_Click(object sender, RoutedEventArgs e)
        {
            MyBranch myBranch = (MyBranch)branchListBox.SelectedItem;

            MyCommitsList.Clear();

            if (myBranch != null)
            {
                if (MessageBox.Show("Произвести checkout? Все несохранённые изменения будут утерены.", "JSCompiler", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    return;
                }
                else
                {
                    using (var repo = new Repository(folderPath))
                    {
                        CheckoutOptions opt = new CheckoutOptions() { CheckoutModifiers = CheckoutModifiers.None };

                        LibGit2Sharp.Commands.Checkout(repo, myBranch.BranchName, opt);

                        foreach (Commit commit in repo.Commits)
                        {
                            MyCommitsList.Add(new MyCommit { Author = commit.Author.ToString(), Message = commit.MessageShort, Sha = commit.Sha, When = commit.Committer.When.ToString(), Committer = commit.Committer.ToString(), Email = commit.Committer.Email });
                            commitListBox.Items.Refresh();
                        }

                        DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
                        if (directoryInfo.Exists)
                        {
                            treeView.Nodes.Clear();
                            MyTabControl.Items.Clear();
                            BuildTree(directoryInfo, treeView.Nodes);
                        }
                    }
                }
            }
            else return;
        }

        private void createBranch_Click(object sender, RoutedEventArgs e)
        {
            using (var repo = new Repository(folderPath))
            {
                CreateBranchWindow createBranchWindow = new CreateBranchWindow();
                string newBranchName;

                if (createBranchWindow.ShowDialog() == true)
                {
                    newBranchName = createBranchWindow.BranchName;
                    repo.CreateBranch(newBranchName);
                }
            }
        }

        private void checkoutCommit_Click(object sender, RoutedEventArgs e)
        {
            MyCommit myCommit = (MyCommit)commitListBox.SelectedItem;

            if (myCommit != null)
            {
                if (MessageBox.Show("Произвести checkout? Все несохранённые изменения будут утерены.", "JSCompiler", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    return;
                }
                else
                {
                    using (var repo = new Repository(folderPath))
                    {
                        CheckoutOptions opt = new CheckoutOptions() { CheckoutModifiers = CheckoutModifiers.None };

                        LibGit2Sharp.Commands.Checkout(repo, myCommit.Sha, opt);
                    }

                    DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
                    if (directoryInfo.Exists)
                    {
                        treeView.Nodes.Clear();
                        MyTabControl.Items.Clear();
                        BuildTree(directoryInfo, treeView.Nodes);
                    }
                }
            }
            else return;
        }
    }
}