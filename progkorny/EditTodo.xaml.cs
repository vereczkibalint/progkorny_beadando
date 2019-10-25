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
    public partial class EditTodo : Window
    {
        private MainWindow parentWindow;

        private string todo_id;
        private string todo_title;
        private string todo_body;
        private string todo_author;
        private string todo_created_at;
        
        /// <summary>
        /// EditTodo window példányosítása
        /// </summary>
        /// <param name="mwindow">MainWindow objektum</param>
        /// <param name="id">Todo ID</param>
        /// <param name="title">Todo Title</param>
        /// <param name="body">Todo Body</param>
        /// <param name="author">Todo Author</param>
        /// <param name="created_at">Todo Created at</param>
        public EditTodo(MainWindow mwindow, string id, string title, string body, string author, string created_at)
        {
            InitializeComponent();

            // DatePicker dátum formátum módosítása YYYY.MM.DD -> YYYY - MM - DD
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            Thread.CurrentThread.CurrentCulture = ci;

            // Osztály mezőinek érték beállítása
            parentWindow = mwindow;
            this.todo_id = id;
            this.todo_title = title;
            this.todo_body = body;
            this.todo_author = author;
            this.todo_created_at = created_at;

            // Az ablakon lévő TextBox-ok értékének beállítása
            txtBox_Title.Text = this.todo_title;
            txtBox_Body.Text = this.todo_body;
            txtBox_Author.Text = this.todo_author;
            dateCreatedAt.Text = this.todo_created_at;
        }

        /// <summary>
        /// Az Update gombra lefutó metódus, kísérletet tesz a Todo frissítésére az adatbázisban
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            // A Todo adatai
            string id = this.todo_id;
            string title = txtBox_Title.Text;
            string body = txtBox_Body.Text;
            string author = txtBox_Author.Text;
            string created_at = dateCreatedAt.Text;
            
            // Az Update kísérlete
            int updateResult = TodoController.UpdateTodo(id, title,body,author,created_at);

            // 0 - Sikertelen frissítés
            // 1 - Sikeres frissítés
            if(updateResult == 1)
            {
                // frissítjük a főablakon a DataGrid-et, hogy egyből lássuk a frissített Todo-t
                parentWindow.RefreshData(); 
                this.Close();
            }
            else
            {
                MessageBox.Show("Hiba történt az adatfrissítés közben!");
            }
        }

        /// <summary>
        /// A Delete gombra lefutó metódus, amely kísérletet tesz a Todo törlésére az adatbázisból
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            // Ha a MessageBox-ra Igen-re nyomnak akkor próbéljuk kitörölni a Todo-t
            if(MessageBox.Show("Biztosan ki szeretnéd törölni ezt a bejegyzést?", "Törlés megerősítése", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                // A törlés kísérlete
                int deleteResult = TodoController.DeleteTodo(this.todo_id);

                // 0 - Sikertelen törlés
                // 1 - Sikeres törlés
                if (deleteResult == 1)
                {
                    //  frissítjük a főablakon a DataGrid-et, hogy egyből lássuk a frissített Todo-t
                    parentWindow.RefreshData();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Hiba történt a törlés közben!");
                }
            }
        }
    }
}
