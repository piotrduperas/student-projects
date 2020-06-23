using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PWSGWPF
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class LandingPage : Page
    {
        public LandingPage()
        {
            InitializeComponent();
        }

        private void ButtonUnlockClick(object sender, RoutedEventArgs e)
        {
            Directory mainDir = null;
            string pass = passwordBox.Password.Length > 0 ? passwordBox.Password : "\0";
            string hash;
            using (MD5 md5 = MD5.Create())
            {
                hash = Encoding.Default.GetString(md5.ComputeHash(Encoding.UTF8.GetBytes(pass)));
            }

            if (System.IO.File.Exists("Passwords.bin"))
            {
                byte[] bytes = System.IO.File.ReadAllBytes("Passwords.bin");
                byte[] decrypted = WPF_Project.DataEncryption.Decrypt(hash, bytes);
                
                if(decrypted == null)
                {
                    MessageBox.Show("Invalid password!", "Password Manager", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                using (MemoryStream ms = new MemoryStream(decrypted))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    try
                    {
                        mainDir = (Directory)formatter.Deserialize(ms);
                    }
                    catch
                    {
                        MessageBox.Show("Deserialization error!", "Password Manager", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }
            NavigationService.Navigate(new MainPage(hash, mainDir));
        }
    }
}
