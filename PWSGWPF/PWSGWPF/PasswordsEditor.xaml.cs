using Microsoft.Win32;
using System;
using System.Windows;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Brushes = System.Windows.Media.Brushes;
using Brush = System.Windows.Media.Brush;

namespace PWSGWPF
{
    /// <summary>
    /// Interaction logic for PasswordsEditor.xaml
    /// </summary>
    public partial class PasswordsEditor : UserControl
    {
        public PasswordsEditor()
        {
            InitializeComponent();
        }

        private void ChangeIcon(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            OpenFileDialog dialog = new OpenFileDialog();
            if(dialog.ShowDialog().Value)
            {
                b.Tag = new BitmapImage(new Uri(dialog.FileName));
            }
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox t = sender as TextBox;
            var c = (t.Tag as CollectionViewSource);
            c.View.Filter = (object o) => ((Account)o).Name?.ToLower().Contains(t.Text.ToLower()) ?? false;
            c.View.Refresh();
        }

        private void accountListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (sender as ListView).Tag = "Preview";
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }

    public class AddNewAccount : AbstractCommand
    {
        public override void Execute(object parameter)
        {
            ListView lv = parameter as ListView;
            Account n = new Account();
            ((lv.ItemsSource as ListCollectionView).SourceCollection as ObservableCollection<Account>).Add(n);

            lv.SelectedItem = n;
            lv.Tag = "Edit";
        }
    }

    public class CancelEdition : AbstractCommand
    {
        public override void Execute(object parameter)
        {
            ListView lv = parameter as ListView;

            lv.Tag = "Preview";

            if ((lv.SelectedItem as Account).Name == null)
            {
                ((lv.ItemsSource as ListCollectionView).SourceCollection as ObservableCollection<Account>).Remove(lv.SelectedItem as Account);
            }
        }
    }

    public class ApplyEdition : AbstractCommand
    {
        public override void Execute(object parameter)
        {
            var panel = parameter as FrameworkElement;

            bool updated = false;

            // Setting default Account Name
            var name = (panel.FindName("nameTB") as TextBox)
                .GetBindingExpression(TextBox.TextProperty);

            Account a = name.ResolvedSource as Account;
            string newName = (name.Target as TextBox).Text;

            if(a.Name != newName)  //Name jest osobno, gdyż miałem problem z bindingiem domyślnej wartości
            {
                a.Name = newName;
                updated = true;
            }

            string[] controls = new string[] {"passwordTB", "loginTB", "websiteTB", "emailTB", "notesTB", "iconControl" };

            foreach(string c in controls)
            {
                FrameworkElement f = panel.FindName(c) as FrameworkElement;
                DependencyProperty p = f is TextBox ? TextBox.TextProperty : FrameworkElement.TagProperty;

                BindingExpression be = f.GetBindingExpression(p);
                updated |= be.IsDirty;
                be.UpdateSource();
            }
            
            ListView lv = panel.Tag as ListView;
            if (updated)
            {
                (lv.SelectedItem as Account).LastEditTime = DateTime.Now;
            }
            lv.Tag = "Preview";
            ICollectionView view = CollectionViewSource.GetDefaultView(lv.ItemsSource);
            view.Refresh();
        }
    }

    public class TurnOnEdition : AbstractCommand
    {
        public override void Execute(object parameter)
        {
            var f = parameter as FrameworkElement;
            f.Tag = "Edit";
        }
    }

    public class DeleteAccount : AbstractCommand
    {
        public override void Execute(object parameter)
        {
            ListView lv = parameter as ListView;
            ((lv.ItemsSource as ListCollectionView).SourceCollection as ObservableCollection<Account>).Remove(lv.SelectedItem as Account);
        }
    }

    public class CopyCommand : AbstractCommand
    {
        public override void Execute(object parameter)
        {
            Clipboard.SetText(parameter as string);
        }
    }

    public class PasswordConverter : IValueConverter
    {
        private static Dictionary<PasswordStrength, (Brush brush, string desc, int perc)> strengths;

        static PasswordConverter()
        {
            strengths = new Dictionary<PasswordStrength, (Brush brush, string desc, int perc)>();
            strengths.Add(PasswordStrength.Invalid, (Brushes.Black, "Invalid", 0));
            strengths.Add(PasswordStrength.VeryWeak, (Brushes.Red, "Very weak", 20));
            strengths.Add(PasswordStrength.Weak, (Brushes.Red, "Weak", 40));
            strengths.Add(PasswordStrength.Average, (Brushes.Orange, "Average", 60));
            strengths.Add(PasswordStrength.Strong, (Brushes.GreenYellow, "Strong", 80));
            strengths.Add(PasswordStrength.VeryStrong, (Brushes.Green, "Very strong", 100));
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var s = strengths[PasswordStrengthUtils.CalculatePasswordStrength(value.ToString())];
            if (targetType == typeof(Brush))
            {
                return s.brush;
            }
            else if (targetType == typeof(string))
            {
                if(parameter as string == "dots")
                {
                    return new string('●', (value as string).Length);
                }
                return s.desc;
            }
            else return s.perc;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class FirstLetterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = value as string;
            if (s != null && s.Length > 0)
                return s.Substring(0, 1).ToUpper();
            return " ";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = value as string;
            return (s == null || s == "") ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ListViewEnabled : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return !(values[0] != null && values[1] as string == "Edit");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MailConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = value as string;
            return "mailto:" + s;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var im = value as BitmapImage;
            //Math.Round, gdyż zauważyłem, że po deserializacji z jakiegoś powodu liczby te dostają jakichś ułamków
            return $"Resolution: {Math.Round(im.Width)}x{Math.Round(im.Height)}\nDPI: {Math.Round(im.DpiX)}x{Math.Round(im.DpiY)}\nFormat: {im.Format}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
