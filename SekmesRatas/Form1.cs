using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SekmesRatas
{
    public partial class Form1 : Form
    {

        List<string> elementList = new List<string>();
        //ObservableCollection<string> observable = new ObservableCollection<string>();

        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\Saved.txt";
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            using (var dialog = new AddDialog())
            {
                DialogResult result = dialog.ShowDialog();

                switch (result)
                {

                    case DialogResult.OK:
                        if (elementList.Count < 20)
                        {
                            elementList.Add(dialog.Textas); 
                        }
                        else
                        {
                            MessageBox.Show("Daugiau elementu negalima prideti");
                        }
                        RefreshListView();
                        break;
                    case DialogResult.Cancel:
                        break;
                }

            }
        }

        private void RefreshListView()
        {
            ClearListView();
            foreach (var elem in elementList)
            {
                listView1.Items.Add(elem);

            }
        }

        private void ClearListView()
        {
            listView1.Clear();
        }

        private void Btn_Clear_Click(object sender, EventArgs e)
        {
            ClearListView();
            elementList.Clear();
        }

        private void Btn_Delete_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem eachItem in listView1.SelectedItems)
            {
                listView1.Items.Remove(eachItem);
                elementList.Remove(eachItem.Text);
            }
            RefreshListView();
        }

        private void Btn_Save_Click(object sender, EventArgs e)
        {

            using (TextWriter tw = new StreamWriter(desktopPath))
            {
                foreach (var s in elementList)
                    tw.WriteLine(s);
            }
        }

        private void Btn_External_Click(object sender, EventArgs e)
        {
            string path = null;

            using (var file = new OpenFileDialog())
            {
                file.Filter = "txt files (*.txt)|*.txt";
                file.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

                if (file.ShowDialog() == DialogResult.OK)
                {
                    path = file.FileName;
                }
            }

            try
            {
                elementList = File.ReadLines(path).Take(20).ToList();
                RefreshListView();
            }
            catch (Exception)
            {
                MessageBox.Show("Bandykite dar karta");
            }

        }

        private void Btn_Help_Click(object sender, EventArgs e)
        {
            MessageBox.Show("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa\naaaaaaaaaaaaaaaaaaaaa","Pagalba",MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
