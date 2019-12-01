using SekmesRatas.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SekmesRatas
{
    public partial class Form1 : Form
    {

        GameWheel wheel;

        Random rand;

        private string _result;

        // ReSharper disable once InconsistentNaming
        private List<string> elementList = new List<string>();

        // ReSharper disable once InconsistentNaming
        private readonly string desktopPath =
            Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\Saved.txt";


        public Form1()
        {
            InitializeComponent();

            //SetStyle(ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            Shown += Frm_Shown;
            timerRepaint.Tick += TimerRepaint_Tick;

            timerRepaint.Interval = 55;
            rand = new Random();
        }

        private void TimerRepaint_Tick(object sender, EventArgs e)
        {
            wheel.Refresh(true);
        }

        private void Frm_Shown(object sender, EventArgs e)
        {
            panel.Height = ClientSize.Height;
            panel.Width = panel.Height + 20;
            panel.Top = (ClientSize.Height - panel.Height) / 2;
            var panelCenter = new Point(panel.Width / 2, panel.Height / 2);

            var radius = (ClientSize.Height - 20) / 2;

            wheel = new GameWheel(panelCenter, radius, 0, 20, 1);
            wheel.SetGraphics(panel.CreateGraphics());
            wheel.Draw();
        }
        
        private void RefreshListView()
        {
            ClearListView();
            foreach (var elem in elementList) listView1.Items.Add(elem);
        }

        private void ClearListView()
        {
            listView1.Clear();
        }

        private void Btn_Clear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Resources.str_ar_tikrai, Resources.str_demesio, MessageBoxButtons.YesNo) !=
                DialogResult.Yes) return;
            ClearListView();
            elementList.Clear();
        }

        private void Btn_Delete_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
                MessageBox.Show(Resources.str_nepasirinktas_elem, Resources.str_demesio, MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

            else if (MessageBox.Show(Resources.str_ar_tikrai, Resources.str_demesio, MessageBoxButtons.YesNo) !=
                     DialogResult.Yes) return;

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
                file.Filter = @"txt files (*.txt)|*.txt";
                file.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

                if (file.ShowDialog() == DialogResult.OK) path = file.FileName;
            }

            try
            {
                elementList = File.ReadLines(path ?? throw new InvalidOperationException()).Take(20).ToList();
                RefreshListView();
            }
            catch (Exception)
            {
                MessageBox.Show(Resources.str_try_again);
            }
        }

        private void Btn_Help_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Resources.str_helpas, Resources.str_pagalba, MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void Btn_Spin_Click(object sender, EventArgs e)
        {
            if (elementList.Count <= 0)
            {
                MessageBox.Show(Resources.str_bent_2_elem, Resources.str_demesio, MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            //SetStyle(ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            timerRepaint.Interval = 55;

            rand = new Random();

            panel.Height = ClientSize.Height;
            panel.Width = panel.Height + 20;
            panel.Top = (ClientSize.Height - panel.Height) / 2;
            var panelCenter = new Point(panel.Width / 2, panel.Height / 2);

            var radius = (ClientSize.Height - 20) / 2;

            wheel = new GameWheel(panelCenter, radius, 1, elementList.Count, 1);
            wheel.SetGraphics(panel.CreateGraphics());
            wheel.Draw();

            if (wheel.State == GameWheel.STATE_NOT_STARTED || wheel.State == GameWheel.STATE_HIGHLIGHTING)
            {
                timerRepaint.Start();
                _result = wheel.Spin(ref rand);
            }

            //MessageBox.Show(_result);
        }

        private void Btn_Add_Click(object sender, EventArgs e)
        {
            using (var dialog = new AddDialog())
            {
                var dialogResult = dialog.ShowDialog();

                switch (dialogResult)
                {
                    case DialogResult.OK:
                        if (elementList.Count <= 20 && !string.IsNullOrWhiteSpace(dialog.Textas))
                            elementList.Add(dialog.Textas);
                        else
                            MessageBox.Show(Resources.str_daugiau_negalima, Resources.str_demesio, MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                        RefreshListView();
                        break;
                    case DialogResult.Cancel:
                        break;
                }
            }
        }
    }
}
