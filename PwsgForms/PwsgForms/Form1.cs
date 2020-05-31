using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;

namespace PwsgForms
{
    public partial class RoomPlaner : Form
    {
        private Bitmap bm;
        private Button selectedButton;
        private Furniture CreatedNow;
        private Furniture selectedFurniture;
        private Point mousePrevPosition;
        private Size maxBitmapSize = new Size(0, 0);
        private BindingList<Furniture> furniture;
        public RoomPlaner()
        {
            Init("en");
        }

        private void Init(string language)
        {
            Controls.Clear();
            CultureInfo culture = CultureInfo.GetCultureInfo(language);
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            InitializeComponent();
            if (bm == null) ResetBitmap();
            else pictureBox.Image = bm;
            BindFurnitureToListBox(furniture ?? new BindingList<Furniture>());
            listBox.SelectedIndexChanged += onListBoxIndexChanged;
            pictureBox.MouseWheel += onMouseWheel;
        }

        void BindFurnitureToListBox(BindingList<Furniture> f)
        {
            if (f != null)
                furniture = f;
            listBox.DataSource = furniture;
            SelectFurniture(null);
        }

        private void CreateNewBitmap(Size s)
        {
            if (bm != null)
            {
                bm.Dispose();
                bm = null;
            }
            bm = new Bitmap(s.Width, s.Height);
            pictureBox.Image = bm;
            pictureBox.Invalidate();
        }

        private void ResetBitmap()
        {
            int w = flowLayoutPanel2.Width - 1;
            int h = flowLayoutPanel2.Height - 1;
            if (w > maxBitmapSize.Width || h > maxBitmapSize.Height)
            {
                maxBitmapSize = new Size(Math.Max(w, maxBitmapSize.Width), Math.Max(h, maxBitmapSize.Height));
                CreateNewBitmap(maxBitmapSize);
            }
        }

        private void UnselectButton()
        {
            if(selectedButton != null) 
                selectedButton.BackColor = Color.White;
            selectedButton = null;
        }

        private void checkIfFurnitureReady()
        {
            if (CreatedNow == null) return;
            if(!CreatedNow.IsBeingCreated)
            {
                UnselectButton();
                CreatedNow = null;
            }
        }

        private void AddFurniture(Point loc)
        {
            if (selectedButton == null) return;

            string type = selectedButton.Tag.ToString();
            CreatedNow = Furniture.Make(loc, type);
            furniture.Add(CreatedNow);
            SelectFurniture(null);
        }

        private void DrawFurniture(Graphics gr)
        {
            if (bm == null) return;
            gr.FillRectangle(Brushes.White, 0, 0, bm.Width, bm.Height);

            foreach(Furniture f in furniture)
            {
                var saveState = gr.BeginContainer();
                gr.TranslateTransform(f.loc.X, f.loc.Y);
                gr.RotateTransform(f.angle);
                gr.ScaleTransform(f.scale, f.scale);
                f.Draw(gr);
                gr.EndContainer(saveState);
            }
        }

        private void SelectFurniture(Furniture f)
        {
            if (selectedFurniture == f) { }
            else if (selectedFurniture != null)
            {
                selectedFurniture.Selected = false;
            }
            selectedFurniture = f;
            if (selectedFurniture != null)
            {
                selectedFurniture.Selected = true;
                int index = furniture.IndexOf(f);
                if (listBox.SelectedIndex != index) listBox.SelectedIndex = index;
            }
            else
            {
                listBox.SelectedIndex = -1;
            }
            onFurnitureChanged();
        }

        private Furniture FindFurnitureAt(Point location)
        {
            Furniture found = null;
            int bestDist = int.MaxValue;
            foreach(Furniture f in furniture.Reverse<Furniture>())
            {
                int? d = f.DistanceFrom(f.WorldToObject(location));
                if(d.HasValue && d.Value < bestDist)
                {
                    bestDist = d.Value;
                    found = f;
                }
            }
            return found;
        }

        private void onPictureBoxPaint(object sender, PaintEventArgs e)
        {
            DrawFurniture(e.Graphics);
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if (selectedFurniture == null) return;
            if (e.KeyCode == Keys.Delete)
            {
                furniture.Remove(selectedFurniture);
                SelectFurniture(null);
            }
            else if (e.KeyCode == Keys.Z)
            {
                selectedFurniture.scale += 0.05f;
            }
            else if (e.KeyCode == Keys.X)
            {
                selectedFurniture.scale -= 0.05f;
            }
            onFurnitureChanged();
        }

        private void onListBoxIndexChanged(object sender, EventArgs e)
        {
            if (listBox.SelectedIndex == -1) return;
            SelectFurniture(furniture[listBox.SelectedIndex]);
        }

        private void onMouseWheel(object sender, MouseEventArgs e)
        {
            if(selectedFurniture != null)
            {
                int d = e.Delta / Math.Abs(e.Delta);
                selectedFurniture.angle += d * 10;
                onFurnitureChanged();
            }
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void onMouseMove(object sender, MouseEventArgs e)
        {
            if (selectedFurniture != null && e.Button == MouseButtons.Left)
            {
                selectedFurniture.loc.X += e.Location.X - mousePrevPosition.X;
                selectedFurniture.loc.Y += e.Location.Y - mousePrevPosition.Y;
                furniture.ResetItem(listBox.SelectedIndex);
                onFurnitureChanged();
            }
            else if(CreatedNow != null)
            {
                CreatedNow.onMoveWhenCreating(e);
                onFurnitureChanged();
            }
            mousePrevPosition = e.Location;
        }

        private void onFurnitureChanged(object sender = null, EventArgs e = null)
        {
            pictureBox.Invalidate();
        }

        private void onResize(object sender, EventArgs e)
        {
            ResetBitmap();
        }

        private void onNewBlueprint(object sender, EventArgs e)
        {
            furniture.Clear();
            ResetBitmap();
            onFurnitureChanged();
        }

        private void onButtonClick(object sender, EventArgs e)
        {
            if (CreatedNow != null)
            {
                CreatedNow.IsBeingCreated = false;
                CreatedNow = null;
            }
            if (selectedButton == sender)
            {
                UnselectButton();
            }
            else
            {
                UnselectButton();
                selectedButton = (Button)sender;
                selectedButton.BackColor = Color.AntiqueWhite;
            }
            
            SelectFurniture(null);
        }

        private void onChangeLanguage(object sender, EventArgs e)
        {
            Init((string) ((ToolStripItem)sender).Tag);
        }

        private void onOpenBlueprint(object sender, EventArgs e)
        {
            Stream file;
            OpenFileDialog sfd = new OpenFileDialog();

            sfd.Filter = Language.Get("FileFilter");
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if ((file = sfd.OpenFile()) != null)
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    try
                    {
                        var newF = (BindingList<Furniture>) formatter.Deserialize(file);
                        Size s = (Size) formatter.Deserialize(file);
                        CreateNewBitmap(s);
                        BindFurnitureToListBox(newF);
                        MessageBox.Show(Language.Get("OpenFileSuccess"));
                    }
                    catch(Exception ee)
                    {
                        MessageBox.Show(Language.Get("CannotOpenFile"));
                    }
                    file.Close();
                }
            }
            sfd.Dispose();
        }

        private void onSaveBlueprint(object sender, EventArgs e)
        {
            Stream file;
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Filter = Language.Get("FileFilter");
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if ((file = sfd.OpenFile()) != null)
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    try
                    {
                        Size s = new Size(bm.Width, bm.Height);
                        formatter.Serialize(file, furniture);
                        formatter.Serialize(file, s);
                        MessageBox.Show(Language.Get("SaveFileSuccess"));
                    }
                    catch
                    {
                        MessageBox.Show(Language.Get("CannotSaveFile"));
                    }
                    file.Close();
                }
            }
            sfd.Dispose();
        }

        private void onMouseDownPicture(object sender, MouseEventArgs e)
        {
            if (CreatedNow == null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (selectedButton == null)
                    {
                        SelectFurniture(FindFurnitureAt(e.Location));
                    }
                    else AddFurniture(e.Location);
                }
            }
            else
            {
                CreatedNow.onClickWhenCreating(e);
                onFurnitureChanged();
            }
            mousePrevPosition = e.Location;
            checkIfFurnitureReady();
        }
    }

    public static class Language
    {
        private static ResourceManager manager = new ResourceManager(typeof(RoomPlaner));

        public static string Get(string s)
        {
            return manager.GetString(s, Thread.CurrentThread.CurrentUICulture) ?? s;
        }
    }
}
