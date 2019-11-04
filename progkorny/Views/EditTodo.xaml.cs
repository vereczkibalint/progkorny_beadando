using System.Globalization;
using System.Threading;
using System.Windows;

namespace progkorny
{
    /// <summary>
    /// Interaction logic for InsertTodo.xaml
    /// </summary>
    public partial class EditTodo : Window
    {
        private MainWindow parentWindow;
        private TodoController todoController;

        private string todo_id;

        /// <summary>
        /// EditTodo window példányosítása
        /// </summary>
        /// <param name="mwindow">MainWindow objektum</param>
        /// <param name="id">Todo ID</param>
        /// <param name="title">Todo Title</param>
        /// <param name="body">Todo Body</param>
        /// <param name="author">Todo Author</param>
        /// <param name="deadline">Todo Deadline</param>
        /// <param name="priority">Todo Priority</param>
        public EditTodo(MainWindow mwindow, string id, string title, string body, string author, string deadline, string priority)
        {
            InitializeComponent();
            todoController = new TodoController();
            // DatePicker dátum formátum módosítása YYYY.MM.DD -> YYYY-MM-DD
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            Thread.CurrentThread.CurrentCulture = ci;

            // Osztály mezőinek érték beállítása
            parentWindow = mwindow;
            todo_id = id;

            // Az ablakon lévő TextBox-ok értékének beállítása
            txtBox_Title.Text = title;
            txtBox_Body.Text = body;
            txtBox_Author.Text = author;
            dateDeadline.Text = deadline;

            switch (priority)
            {
                case "normal":
                    cmbPriority.SelectedIndex = 0;
                    break;
                case "important":
                    cmbPriority.SelectedIndex = 1;
                    break;
                case "urgent":
                    cmbPriority.SelectedIndex = 2;
                    break;
                default:
                    cmbPriority.SelectedIndex = 0;
                    break;
            }
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
            string deadline = dateDeadline.Text;
            string priority = cmbPriority.Text.ToLower();
            
            // Az Update kísérlete
            int updateResult = todoController.UpdateTodo(id,title,body,author,deadline,priority);

            // -1 - Sikertelen frissítés
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
                int deleteResult = todoController.DeleteTodo(this.todo_id);

                // -1 - Sikertelen törlés
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
