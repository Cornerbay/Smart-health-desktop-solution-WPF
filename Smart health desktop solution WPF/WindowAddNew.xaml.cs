using System;
using System.Collections;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            Label tableNameLabel = new Label();
            tableNameLabel.Content = tableName;
            tableNameLabel.FontSize = 20;
            textboxStackpanel.Children.Add(tableNameLabel);

            foreach (DataRow row in tableColumns.Rows)
            {
                StackPanel stackpanel = new StackPanel();
                stackpanel.Name = row[0].ToString();
                stackpanel.Orientation = Orientation.Vertical;
                Label label = new Label();
                label.Content = row[0].ToString() + ":";
                stackpanel.Children.Add(label);
                
                if (row[1].ToString() == "date")
                {
                    DatePicker datePicker = new DatePicker();
                    datePicker.Name = row[0].ToString() + "Date";
                    stackpanel.Children.Add(datePicker);
                }else if (row[1].ToString() == "int")
                {
                    TextBox textbox = new TextBox();
                    textbox.PreviewTextInput += intRegexTextbox;
                    textbox.Name = row[0].ToString() + "Textbox";
                    if(!DBNull.Value.Equals(row[2]))
                    {
                        textbox.MaxLength = Convert.ToInt32(row[2]);
                    }
                    else { textbox.MaxLength = 9; }
                    stackpanel.Children.Add(textbox);
                }
                else
                {
                    TextBox textbox = new TextBox();
                    textbox.Name = row[0].ToString() + "Textbox";
                    if (!DBNull.Value.Equals(row[2]))
                    {
                        textbox.MaxLength = Convert.ToInt32(row[2]);
                    }
                    stackpanel.Children.Add(textbox);
                }
                textboxStackpanel.Children.Add(stackpanel);
            }
        }


        private void intRegexTextbox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            if (regex.IsMatch(e.Text))
            {
                MessageBox.Show("Invalid Input! \n This field can only contain numbers.");
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
            ArrayList addToTableList = new ArrayList();
            string tableName = null;
            Persistence persistence = new Persistence();

            foreach (object child in textboxStackpanel.Children)
            {
                TextBox textbox = null;
                StackPanel stackPanel = null;
                Label tableLabel = null;
                if (child is FrameworkElement)
                {
                    if(child is Label)
                    {
                        tableLabel = (Label)child;
                        tableName = (String)tableLabel.Content;
                    }

                    if(child is StackPanel)
                    {
                        stackPanel = (StackPanel)child;
                        foreach (object child2 in stackPanel.Children)
                        {
                            if (child2 is TextBox)
                            {
                                textbox = (TextBox)child2;
                                addToTableList.Add(textbox.Text);
                            }
                        }
                    }
                    /*childname.Name = (child as FrameworkElement).Name + "Textbox";*/
                   
                }


            }
            foreach (Object obj in addToTableList)
            {
                Console.Write("{0}", obj);
            }
            
            persistence.AddQuerySentence(addToTableList, tableName);
        }
    }
}
