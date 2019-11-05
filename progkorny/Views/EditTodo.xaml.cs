using System;
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
        public EditTodo(MainWindow mwindow, Todo todoToEdit)
        {
            InitializeComponent();
            todoController = new TodoController();
            // DatePicker dátum formátum módosítása YYYY.MM.DD -> YYYY-MM-DD
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            Thread.CurrentThread.CurrentCulture = ci;

            // Osztály mezőinek érték beállítása
            parentWindow = mwindow;
            todo_id = todoToEdit.Todo_ID;

            // Az ablakon lévő TextBox-ok értékének beállítása
            txtBox_Title.Text = todoToEdit.Todo_Title;
            txtBox_Body.Text = todoToEdit.Todo_Body;
            txtBox_Author.Text = todoToEdit.Todo_Author;
            dateDeadline.Text = todoToEdit.Todo_Deadline;

            switch (todoToEdit.Todo_Priority)
            {
                case Priority.NORMAL:
                    cmbPriority.SelectedIndex = 0;
                    break;
                case Priority.IMPORTANT:
                    cmbPriority.SelectedIndex = 1;
                    break;
                case Priority.URGENT:
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
            Todo updatedTodo = new Todo();

            updatedTodo.Todo_ID = this.todo_id;
            updatedTodo.Todo_Title = txtBox_Title.Text;
            updatedTodo.Todo_Body = txtBox_Body.Text;
            updatedTodo.Todo_Author = txtBox_Author.Text;
            updatedTodo.Todo_Deadline = dateDeadline.Text;
            updatedTodo.Todo_Priority = (Priority)cmbPriority.SelectedIndex;
            
            // Az Update kísérlete
            int updateResult = todoController.UpdateTodo(updatedTodo);

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
