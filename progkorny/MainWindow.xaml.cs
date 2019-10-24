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
using MahApps.Metro.Controls;
using System.Data.OleDb;
using System.Data;
using System.Threading;

namespace progkorny
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private DataTable dt = new DataTable();
        private List<string> colors = new List<string>() { "kék", "narancssárga", "lila", "barna" };

        public MainWindow()
        {
            InitializeComponent();

            colorsComboBox.ItemsSource = colors;
            colorsComboBox.SelectedIndex = 0;
        }

        public void RefreshData()
        {
            dt.Clear();
            TodoController.LoadTodos(dt);
            dataGrid.DataContext = dt;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            dt.Clear();
            TodoController.LoadTodos(dt);
            dataGrid.DataContext = dt;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Biztosan kilépsz?", "Kilépés megerősítése", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void CreateTodoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            InsertTodo insertTodoWindow = new InsertTodo(this);
            insertTodoWindow.Show();
        }

        private void MetroWindow_GotFocus(object sender, RoutedEventArgs e)
        {
            dt.Clear();
            TodoController.LoadTodos(dt);
            dataGrid.DataContext = dt;
        }

        private void darkThemeToggle_Checked(object sender, RoutedEventArgs e)
        {
            ThemeController tc = new ThemeController();
            tc.ChangeTheme(Themes.DARK);
        }

        private void DarkThemeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            ThemeController tc = new ThemeController();
            tc.ChangeTheme(Themes.LIGHT);
        }

        private void ColorsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ThemeController tc = new ThemeController();
            switch (colorsComboBox.SelectedValue)
            {
                case "kék":
                    tc.ChangeColor(Colors.KEK);
                    break;
                case "lila":
                    tc.ChangeColor(Colors.LILA);
                    break;
                case "narancssárga":
                    tc.ChangeColor(Colors.NARANCSSARGA);
                    break;
                case "barna":
                    tc.ChangeColor(Colors.BARNA);
                    break;
                default:
                    tc.ChangeColor(Colors.KEK);
                    break;
            }
        }

        private void EditTodo_Handler(object sender, MouseButtonEventArgs e)
        {
            if(dataGrid.SelectedItem != null)
            {
                DataRowView drv = dataGrid.SelectedItem as DataRowView;
                string id = drv["todo_id"].ToString();
                string title = drv["todo_title"].ToString();
                string body = drv["todo_body"].ToString();
                string author = drv["todo_author"].ToString();
                string created_at = drv["todo_created_at"].ToString();
                EditTodo editTodo = new EditTodo(this, id,title,body,author,created_at);
                editTodo.Show();
            }
        }

    }
}
