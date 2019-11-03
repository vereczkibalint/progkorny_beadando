using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace progkorny
{
    /// <summary>
    /// Interaction logic for InsertTodo.xaml
    /// </summary>
    public partial class InsertTodo : Window
    {
        private MainWindow parentWindow;

        /// <summary>
        /// InsertTodo window példányosítása
        /// </summary>
        /// <param name="mwindow">MainWindow objektum</param>
        public InsertTodo(MainWindow mwindow)
        {
            InitializeComponent();

            // Osztály mezőjének értékbeállítása
            parentWindow = mwindow;

            // DatePicker custom formátum
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            Thread.CurrentThread.CurrentCulture = ci;
        }

        /// <summary>
        /// Insert gombra lefutó metódus, amely kísérletet tesz a Todo beillesztésére az adatbázisba
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InsertBtn_Click(object sender, RoutedEventArgs e)
        {
            // Todo adatai
            string title = txtBox_Title.Text;
            string body = txtBox_Body.Text;
            string author = txtBox_Author.Text;
            string created_at = dateCreatedAt.Text;
            string priority = cmbPriority.SelectedValue.ToString().ToLower();

            // Az insert kísérlete
            int insertResult = TodoController.InsertTodo(title,body,author,created_at,priority);

            // 0 - Sikertelen insert
            // 1 - Sikeres insert

            if(insertResult == 1)
            {
                // frissítjük a főablakon a DataGrid-et, hogy egyből lássuk az új Todo-t 
                parentWindow.RefreshData();
                this.Close();
            }
            else
            {
                MessageBox.Show("Hiba történt az adatfelvitel közben!");
            }
        }
    }
}
