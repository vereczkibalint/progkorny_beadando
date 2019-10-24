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
        public InsertTodo(MainWindow mwindow)
        {
            InitializeComponent();
            parentWindow = mwindow;

            // DatePicker custom formátum
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            Thread.CurrentThread.CurrentCulture = ci;
        }

        private void InsertBtn_Click(object sender, RoutedEventArgs e)
        {
            string title = txtBox_Title.Text;
            string body = txtBox_Body.Text;
            string author = txtBox_Author.Text;
            string created_at = dateCreatedAt.Text;
            
            int insertResult = TodoController.InsertTodo(title,body,author,created_at);

            if(insertResult == 1)
            {
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
