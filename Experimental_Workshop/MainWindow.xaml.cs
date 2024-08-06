using Experimental_Workshop.MVVM.Model;
using Experimental_Workshop.ApplicationResourses;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.DirectoryServices.ActiveDirectory;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security;
using System.Runtime.InteropServices;
using Experimental_Workshop.MVVM.ViewModel;
using Experimental_Workshop.MVVM.View;

namespace Experimental_Workshop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainVM mainVM;
        public MainWindow()
        {
            InitializeComponent();
            mainVM = new MainVM();
            this.DataContext = mainVM;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        
        

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// МЕРЗОСТЬ УЖОС ГАДОСТЬ ТВАРЬ Я ЧТО ТАК ПИШУ КОД ВЫРВИТЕ МНЕ ГЛАЗА НО Я УСТАЛ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckPassword_Click(object sender, RoutedEventArgs e)
        {
            if(Marshal.PtrToStringUni(Marshal.SecureStringToGlobalAllocUnicode(PasswordBox.SecurePassword)) == "123456")
            {
                PasswordBox.Clear();
                PasswordText.Visibility = Visibility.Collapsed;
                PasswordBox.Visibility = Visibility.Collapsed;
                CheckPassword.Visibility = Visibility.Collapsed;
                var window = new AdminWindow();
                window.Show();
                window.DatabaseUpdated += mainVM.OnContextUpdate;
            }
            else
            {
                PasswordBox.Clear();
                PasswordText.Visibility = Visibility.Collapsed;
                PasswordBox.Visibility = Visibility.Collapsed;
                CheckPassword.Visibility = Visibility.Collapsed;
            }
        }
        /// <summary>
        /// УЖАС УЖАС УЖАС Я ПРОСТО ЗАЕБАЛСЯ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PasswordText.Visibility = Visibility.Visible;
            PasswordBox.Visibility = Visibility.Visible;
            CheckPassword.Visibility = Visibility.Visible;
        }
    }
}