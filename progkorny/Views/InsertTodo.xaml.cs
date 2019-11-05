using System;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace progkorny
{
    /// <summary>
    /// Interaction logic for InsertTodo.xaml
    /// </summary>
    public partial class InsertTodo : Window
    {
        private MainWindow parentWindow;
        private TodoController todoController;

        /// <summary>
        /// InsertTodo window példányosítása
        /// </summary>
        /// <param name="mwindow">MainWindow objektum</param>
        public InsertTodo(MainWindow mwindow)
        {
            InitializeComponent();

            // Osztály mezőjének értékbeállítása
            todoController = new TodoController();
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
            Todo insertTodo = new Todo();
            insertTodo.Todo_Title = txtBox_Title.Text;
            insertTodo.Todo_Body = txtBox_Body.Text;
            insertTodo.Todo_Author = txtBox_Author.Text;
            insertTodo.Todo_Deadline = dateDeadline.Text;
            insertTodo.Todo_Priority = (Priority)Enum.Parse(typeof(Priority), cmbPriority.SelectedValue.ToString().ToUpper());

            // Az insert kísérlete
            int insertResult = todoController.InsertTodo(insertTodo);
            // -1 - Sikertelen insert
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
