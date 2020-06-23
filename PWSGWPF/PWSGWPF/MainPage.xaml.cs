using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage(string password, Directory mainDir)
        {
            Data = new DataModel(mainDir);
            Data.SaveCommand = new SavePasswords(Data.MainDir, password);
            //MockTree();
            InitializeComponent();
            DataContext = Data;
        }

        public DataModel Data { get; set; }

        private void MockTree()
        {
            Directory dirr = new Directory();
            Data.MainDir.Add(new Directory());
            dirr.Add(new Image());
            Passwords p = new Passwords();
            p.Accounts.Add(new Account() { Name = "John Mock" });
            p.Accounts.Add(new Account() { Name = "Paweł Aszklar", Password = "WPF to chuj" });
            Data.MainDir.Add(dirr);
            Data.MainDir.Add(p);
        }

        private void MenuItemLogoutClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LandingPage());
        }

        private void StackPanel_LostFocus(object sender, RoutedEventArgs e)
        {
            StackPanel s = (StackPanel)sender;
            s.Tag = "Collapsed";
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                Keyboard.ClearFocus();
                StackPanel_LostFocus(sender, e);
            }
        }
    }

    public class FileCollection : ObservableCollection<File> { }

    public class DataModel
    {
        public DataModel(Directory dir = null)
        {
            MainDir = dir ?? new Directory();
        }
        public Directory MainDir { get; }
        public ICommand SaveCommand { get; set; }
    }

    [Serializable]
    abstract public class File
    {
        public Directory Parent { get; set; }
        public string Name { get; set; }
    }

    [Serializable]
    public class Directory : File
    {
        public Directory()
        {
            Name = "New Directory";
            Files = new ObservableCollection<File>();
        }
        [field: NonSerialized]
        public ObservableCollection<File> Files { get; set; }
        private File[] SerializationFiles;
        public void Add(File f)
        {
            f.Parent = this;
            Files.Add(f);
        }

        [OnSerializing]
        public void OnSerializingMethod(StreamingContext context)
        {
            SerializationFiles = Files.ToArray();
        }

        [OnDeserialized]
        public void OnDeserializeMethod(StreamingContext context)
        {
            Files = new ObservableCollection<File>(SerializationFiles);
        }
    }

    [Serializable]
    public class Image : File
    {
        public Image()
        {
            Name = "New Image";
        }
        [field: NonSerialized]
        public BitmapImage Bitmap { get; set; }
        private byte[] SerializationBitmap;

        [OnSerializing]
        public void OnSerializingMethod(StreamingContext context)
        {
            SerializationBitmap = BitmapHelper.BitmapToBytes(Bitmap);
        }

        [OnDeserialized]
        public void OnDeserializeMethod(StreamingContext context)
        {
            Bitmap = BitmapHelper.BytesToBitmap(SerializationBitmap);
        }
    }

    [Serializable]
    public class Passwords : File
    {
        [field: NonSerialized]
        public ObservableCollection<Account> Accounts { get; set; } = new ObservableCollection<Account>();
        private Account[] SerializationAccounts;
        public Passwords()
        {
            Name = "New Passwords";
        }
        public File[] SerializationFiles;

        [OnSerializing]
        public void OnSerializingMethod(StreamingContext context)
        {
            SerializationAccounts = Accounts.ToArray();
        }

        [OnDeserialized]
        public void OnDeserializeMethod(StreamingContext context)
        {
            Accounts = new ObservableCollection<Account>(SerializationAccounts);
        }
    }

    [Serializable]
    public class Account
    {
        [field: NonSerialized]
        public BitmapImage Icon { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Website { get; set; }
        public string Notes { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastEditTime { get; set; }

        public Account()
        {
            CreationTime = LastEditTime = DateTime.Now;
        }

        private byte[] SerializationIcon;

        [OnSerializing]
        public void OnSerializingMethod(StreamingContext context)
        {
            SerializationIcon = BitmapHelper.BitmapToBytes(Icon);
        }

        [OnDeserialized]
        public void OnDeserializeMethod(StreamingContext context)
        {
            Icon = BitmapHelper.BytesToBitmap(SerializationIcon);
        }
    }

    public static class BitmapHelper
    {
        public static byte[] BitmapToBytes(BitmapImage Bitmap)
        {
            if (Bitmap == null) return null;
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(Bitmap));

            using (var fileStream = new MemoryStream())
            {
                encoder.Save(fileStream);
                return fileStream.ToArray();
            }
        }

        public static BitmapImage BytesToBitmap(byte[] bytes)
        {
            if (bytes == null) return null;
            using (var ms = new MemoryStream(bytes))
            {
                var Bitmap = new BitmapImage();
                Bitmap.BeginInit();
                Bitmap.CacheOption = BitmapCacheOption.OnLoad;
                Bitmap.StreamSource = ms;
                Bitmap.EndInit();
                return Bitmap;
            }
        }
    }

    public abstract class AbstractCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;
        public abstract void Execute(object parameter);
    }

    public class AddSomething<T> : AbstractCommand
         where T : File, new()
    {
        public override void Execute(object parameter)
        {
            var dir = (Directory)parameter;
            dir.Add(new T());
        }
    }

    public class AddDir : AddSomething<Directory> { }
    public class AddImage : AbstractCommand
    {
        public override void Execute(object parameter)
        {
            var dir = (Directory)parameter;
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG;*TIF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIF|All files (*.*)|*.*";
            
            if (dialog.ShowDialog().Value)
            {
                Image img = new Image();
                img.Bitmap = new BitmapImage(new Uri(dialog.FileName));
                dir.Add(img);
            }
        }
    }
    public class AddPasswords : AddSomething<Passwords> { }

    public class SavePasswords : AbstractCommand
    {
        private Directory data;
        private string password;
        public SavePasswords(Directory data, string password)
        {
            this.data = data;
            this.password = password;
        }
        public override void Execute(object parameter)
        {
            using (var fileStream = new FileStream("Passwords.bin", FileMode.OpenOrCreate))
            using (var memoryStream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, data);

                memoryStream.Seek(0, SeekOrigin.Begin);
                var bytes = new byte[memoryStream.Length];
                memoryStream.Read(bytes, 0, (int)memoryStream.Length);

                // Encrypt your bytes with your chosen encryption method, and write the result instead of the source bytes
                var encryptedBytes = WPF_Project.DataEncryption.Encrypt(password, bytes);
                fileStream.Write(encryptedBytes, 0, encryptedBytes.Length);
            }
            
        }
    }

    public class RenameFile : AbstractCommand
    {
        
        public override void Execute(object parameter)
        {
            DependencyObject ob = (DependencyObject)parameter;
            do
            {
                ob = VisualTreeHelper.GetParent(ob);
            } while (!(ob is ContextMenu));

            ob = (ob as ContextMenu).PlacementTarget;

            do
            {
                ob = VisualTreeHelper.GetParent(ob);
            } while (!(ob is StackPanel));

            StackPanel sp = ob as StackPanel;
            sp.Tag = "Visible";
            ((TextBox)sp.Children[1]).Focus();
        }
    }

    public class RemoveFile : AbstractCommand
    {
        public override void Execute(object parameter)
        {
            File f = (File)parameter;
            f.Parent.Files.Remove(f);
        }
    }

    public class SaveImage : AbstractCommand
    {
        public override void Execute(object parameter)
        {
            BitmapImage b = (BitmapImage)parameter;
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "PNG File(*.PNG)|*.PNG";
            if (dialog.ShowDialog().Value)
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(b));

                using (var fileStream = new FileStream(dialog.FileName, FileMode.Create))
                {
                    encoder.Save(fileStream);
                }
            }
        }
    }
}
