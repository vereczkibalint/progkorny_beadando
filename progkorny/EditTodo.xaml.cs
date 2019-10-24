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
        
        public EditTodo(MainWindow mwindow, string id, string title, string body, string author, string created_at)
        {
            InitializeComponent();

            // DatePicker custom formátum
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            Thread.CurrentThread.CurrentCulture = ci;

            parentWindow = mwindow;
            this.todo_id = id;
            this.todo_title = title;
            this.todo_body = body;
            this.todo_author = author;
            this.todo_created_at = created_at;

            txtBox_Title.Text = this.todo_title;
            txtBox_Body.Text = this.todo_body;
            txtBox_Author.Text = this.todo_author;
            dateCreatedAt.Text = this.todo_created_at;
        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            string id = this.todo_id;
            string title = txtBox_Title.Text;
            string body = txtBox_Body.Text;
            string author = txtBox_Author.Text;
            string created_at = dateCreatedAt.Text;
            
            int updateResult = TodoController.UpdateTodo(id, title,body,author,created_at);

            if(updateResult == 1)
            {
                parentWindow.RefreshData();
                this.Close();
            }
            else
            {
                MessageBox.Show("Hiba történt az adatfrissítés közben!");
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Biztosan ki szeretnéd törölni ezt a bejegyzést?", "Törlés megerősítése", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                int deleteResult = TodoController.DeleteTodo(this.todo_id);

                if(deleteResult == 1)
                {
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
