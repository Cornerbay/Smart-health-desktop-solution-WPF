using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

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
            DataTable databaseTables = persistence.GetSchema();

            foreach (DataRow row in databaseTables.Rows)
            {
                comboBoxGetTable.Items.Add(row[2]);
            }
        }

        private void comboBoxGetTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string tableName = (sender as ComboBox).SelectedItem as string;
            Persistence persistence = new Persistence();
            DataTable tableColumns = persistence.GetColumnNames(tableName);

            //removes all children already existing in stackpanel
            if (textboxStackpanel.Children.Count > 0)
            {
                textboxStackpanel.Children.Clear();
            }
            foreach (DataRow row in tableColumns.Rows)
            {
                StackPanel stackpanel = new StackPanel();
                stackpanel.Name = row[0].ToString();
                stackpanel.Orientation = Orientation.Vertical;
                Label label = new Label();
                label.Content = row[0].ToString();
                TextBox textbox = new TextBox();
                textbox.Name = row[0].ToString()+"Textbox";
                Label infoLabel = new Label();
                infoLabel.Name = row[0].ToString() + "Info";
                infoLabel.Visibility = Visibility.Hidden;
                infoLabel.Content = row[1];
                stackpanel.Children.Add(label);
                stackpanel.Children.Add(textbox);
                stackpanel.Children.Add(infoLabel);
                textboxStackpanel.Children.Add(stackpanel);
            }
        }

        private void Add_Row_Button_Click(object sender, RoutedEventArgs e)
        {
 /*           Hashtable htAddRow = new Hashtable()
            {
                {"Table", comboBoxGetTable.ToString()},
                {"BirthNumber", Int64.Parse(Birthdate.Text)},
                {"FirstName", "fornavn"},
                {"LastName", "etternavn"},
                {"Adress", "addresse"},
                {"Birthdate","2001-01-01"},
                {"AuthorizationLevel", 1}
            };*/

            foreach (object child in textboxStackpanel.Children)
            {
                string childname = null;
                if (child is FrameworkElement)
                {
                    childname = (child as FrameworkElement).Name;
                }

                string textboxName = childname + "Textbox";
                string hiddenLabelName = childname + "Info";
                /*
                                htAddRow.Add(childname, textboxName.Text);

                                Console.WriteLine(childname);
                */
                TextBox textbox;
                textbox = (TextBox)FindName(textboxName);
                Console.WriteLine(textbox.Text);

            }
        }
    }
}
