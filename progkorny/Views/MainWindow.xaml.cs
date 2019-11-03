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
        private List<string> dates = new List<string>();
        private ThemeController tc = new ThemeController();

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Kiüríti a DataGrid-et, lekéri az adatbázisból a Todo-kat, majd újra feltölti a DataGridet a kapott eredménnyel
        /// </summary>
        public void RefreshData()
        {
            dt.Clear();
            TodoController.LoadTodos(dt);
            dataGrid.DataContext = dt;

            LoadDates();
        }

        /// <summary>
        /// Feltölti a színválasztó ComboBox-ot a Nézet menüben, és kiválasztottra állítja a legelsőt
        /// </summary>
        //private void LoadColors()
        //{
        //    colorsComboBox.ItemsSource = colors;
        //    colorsComboBox.SelectedIndex = 0;
        //}

        /// <summary>
        /// MainWindow Loaded metódusa, itt történik a Todo-k betöltése, DataGrid feltöltése, Színek és dátumok betöltése
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        /// <summary>
        /// Kilépés gombra lefutó metódus, amely MessageBox-ban megerősítteti a kilépési szándékot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Biztosan kilépsz?", "Kilépés megerősítése", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// Új Todo létrehozása, InsertTodo ablakot nyit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateTodoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            InsertTodo insertTodoWindow = new InsertTodo(this);
            insertTodoWindow.Show();
        }

        /// <summary>
        /// Sötét téma checked metódusa, ha lefut, a téma átvált BaseDark-ra
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void darkThemeToggle_Checked(object sender, RoutedEventArgs e)
        {
            tc.ChangeTheme(Themes.DARK);
        }

        /// <summary>
        /// Sötét téma unchecked metódusa, ha lefut, a téma átvált BaseLight-ra
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DarkThemeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            tc.ChangeTheme(Themes.LIGHT);
        }

        /// <summary>
        /// A Calendarhoz készült metódus, amely csak a dátumokat tölti be, hogy azok megjelenítődhessenek a naptáron
        /// </summary>
        private void LoadDates()
        {
            dates = TodoController.LoadDates();
        }

        /// <summary>
        /// A színeket beállító ComboBox SelectionChanged metódusa, amely ha lefut, akkor a kiválasztott színűre vált az ablak témája
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColorsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (colorsComboBox.SelectedValue.ToString())
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
            }
        }

        /// <summary>
        /// DataGridRow dupla klikk metódusa, mely kiszedi az adott sorból az értékeket és továbbadja azt az EditTodo ablaknak szerkesztésre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditTodo_Handler(object sender, MouseButtonEventArgs e)
        {
            if(dataGrid.SelectedItem != null)
            {
                DataRowView drv = (DataRowView)dataGrid.SelectedItem;

                string id = drv["todo_id"].ToString();
                string title = drv["todo_title"].ToString();
                string body = drv["todo_body"].ToString();
                string author = drv["todo_author"].ToString();
                string created_at = drv["todo_created_at"].ToString();
                string priority = drv["todo_priority"].ToString();

                EditTodo editTodo = new EditTodo(this,id,title,body,author,created_at,priority);
                editTodo.Show();
            }
        }

        /// <summary>
        /// Naptár nézet kapcsolója, ha lefut, akkor a DataGrid eltűnik, helyette megjelenik a naptár
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewModeCalendar_Checked(object sender, RoutedEventArgs e)
        {
            dataGrid.Visibility = Visibility.Hidden;
            calendar.Visibility = Visibility.Visible;
            LoadDates();
            foreach (string date in dates)
            {
                calendar.SelectedDates.Add(DateTime.Parse(date));
            }
        }

        /// <summary>
        /// Naptár nézet kapcsolója, ha lefut, akkor a naptár eltűnik és megjelenítődik a DataGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewModeCalendar_Unchecked(object sender, RoutedEventArgs e)
        {
            dataGrid.Visibility = Visibility.Visible;
            calendar.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Naptár dátumválasztó metódusa, minden dátumválasztásnál újra be kell tölteni a dátumokat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Calendar senderCalendar = (Calendar)sender;
            DateTime selectedDate = (DateTime)senderCalendar.SelectedDate;
            calendar.SelectedDates.Add(selectedDate);

            foreach (string date in dates)
            {
                calendar.SelectedDates.Add(DateTime.Parse(date));
            }
        }

        /// <summary>
        /// Adatok frissítése, újra betölti a dátumokat és hozzáadja azokat a naptárhoz, illetve a DataGrid-et is frissíti
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshTodoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            RefreshData();
            foreach (string date in dates)
            {
                calendar.SelectedDates.Add(DateTime.Parse(date));
            }
        }
    }
}
