using System;
using System.Collections.Generic;
using System.Data;
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

namespace Smart_health_desktop_solution_WPF
{
    /// <summary>
    /// Interaction logic for WindowAddNew.xaml
    /// </summary>
    public partial class WindowAddNew : Window
    {
        public WindowAddNew()
        {
            InitializeComponent();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Persistence persistence = new Persistence();
            DataTable databaseTables = persistence.getSchema();

            foreach (DataRow row in databaseTables.Rows)
            {
                comboBoxGetTable.Items.Add(row[2]);
            }
        }

        private void comboBoxGetTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string tableName = (sender as ComboBox).SelectedItem as string;
            Persistence persistence = new Persistence();
            DataTable tableColumns = persistence.getColumnNames(tableName);

            //removes all children already existing in stackpanel
            if (textboxStackpanel.Children.Count > 0)
            {
                textboxStackpanel.Children.Clear();
            }
            foreach (DataRow row in tableColumns.Rows)
            {
                StackPanel stackpanel = new StackPanel();
                stackpanel.Orientation = Orientation.Vertical;
                Label label = new Label();
                label.Content = row[0].ToString()+"Label";
                TextBox textbox = new TextBox();
                textbox.Name = row[0].ToString()+"Textbox";
                stackpanel.Children.Add(label);
                stackpanel.Children.Add(textbox);
                textboxStackpanel.Children.Add(stackpanel);
            }
        }
    }
}
