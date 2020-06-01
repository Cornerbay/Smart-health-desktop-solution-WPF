using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Smart_health_desktop_solution_WPF
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private static readonly string connectionString = "Server=tcp:healthcare-app2000.database.windows.net,1433;Initial Catalog=healthcare-app;Persist Security Info=False;User ID=designerkaktus;Password=HestErBest!!!1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private SqlConnection con = null;
        private Persistence persistence = new Persistence();

        public Login()
        {
            setConnection();
            InitializeComponent();
            
        }

        private void setConnection()
        {
            con = new SqlConnection(connectionString);
            try
            {
                con.Open();
            }
            catch (Exception exp)
            {
                MessageBox.Show("The connection seems to have failed due to:\n" + exp.ToString());
            }

        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            string password = passwordBox.Password;
            string username = usernameTxtBox.Text;
            String[] user = new String[4];
            string table = null;

            switch (roleComboBox.SelectedIndex)
            {
                case 0:
                    user = persistence.GetUser(username, "Doctor");
                    table = "Doctor";
                    break;
                case 1:
                    user = persistence.GetUser(username, "Admin");
                    table = "Admin";
                    break;
            }

            if (user[0] != null && user[0].Equals("1"))
            {
                if (CheckPassword(password, user[1]))
                {
                    if (table.Equals("Doctor"))
                    {
                        this.Close();
                        new MainWindow(user[2] + " " + user[3],username,table).ShowDialog();
                    }
                    else
                    {
                        this.Close();
                        new MainWindow(user[2]+ " " + user[3],username,table).ShowDialog();
                    }
                }
                else
                {
                    errorMsgTxtBlock.Visibility = Visibility.Visible;
                    ButtonShakeAnimation();
                }
            }
            else
            {
                errorMsgTxtBlock.Visibility = Visibility.Visible;
                ButtonShakeAnimation();
            }
        }

        private bool CheckPassword(string password, string hash)
        {
            bool isCorrect = false;
            try
            {
                isCorrect = (SecurePasswordHasher.Verify(password, hash));
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("The password hash in database is wrong");
            }
            return isCorrect;
        }


        private void ButtonShakeAnimation()
        {
            var doubleAnimation = new DoubleAnimation(-3, 3, TimeSpan.FromSeconds(0.05));
            doubleAnimation.AutoReverse = true;
            doubleAnimation.RepeatBehavior = new RepeatBehavior(3);
            doubleAnimation.FillBehavior = (FillBehavior)1;
            buttonRotation.BeginAnimation(RotateTransform.AngleProperty, doubleAnimation);
        }

        private void roleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            usernameTxtBlock.Visibility = Visibility.Visible;
            usernameTxtBox.Visibility = Visibility.Visible;
            passwordTxtBlock.Visibility = Visibility.Visible;
            passwordBox.Visibility = Visibility.Visible;
            loginDockPanel.Visibility = Visibility.Visible;
        }
    }
}
