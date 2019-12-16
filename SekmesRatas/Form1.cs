using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using SekmesRatas.Properties;

namespace SekmesRatas
{
    public partial class Form1 : Form
    {

        GameWheel wheel;

        Random rand;
        private SystemLog log;

        private string _result;

        // ReSharper disable once InconsistentNaming
        private List<string> elementList = new List<string>();

        // ReSharper disable once InconsistentNaming
        private readonly string desktopPath =
            Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\Saved.txt";


        public Form1()
        {
            InitializeComponent();

            Shown += Frm_Shown;
            timerRepaint.Tick += TimerRepaint_Tick;

            timerRepaint.Interval = 55;
            
            log = new SystemLog("Form init");

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

            wheel = new GameWheel(panelCenter, radius, 1, 20, 1);
            wheel.SetGraphics(panel.CreateGraphics());
            wheel.Draw();

            log.LogAction("Building wheel");
        }

        private void RefreshListView()
        {
            ClearListView();
            foreach (var elem in elementList) listView1.Items.Add(elem);
            log.LogAction("Refreshing view");
        }

        private void ClearListView()
        {
            listView1.Clear();
            log.LogAction("Clearing view");
        }

        private void Btn_Clear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Resources.str_ar_tikrai, Resources.str_demesio, MessageBoxButtons.YesNo) !=
                DialogResult.Yes) return;
            ClearListView();
            elementList.Clear();
            log.LogAction("Button clear pressed");
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
            log.LogAction("Button delete pressed");
        }

        private void Btn_Save_Click(object sender, EventArgs e)
        {
            using (TextWriter tw = new StreamWriter(desktopPath))
            {
                foreach (var s in elementList)
                    tw.WriteLine(s);
            }
            log.LogAction("Button save pressed");
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
            log.LogAction("Button external pressed");
        }

        private void Btn_Help_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Resources.str_helpas, Resources.str_pagalba, MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            log.LogAction("Button help pressed");
        }

        private void Btn_Spin_Click(object sender, EventArgs e)
        {
            UpdateButtonSpinEnabled(false);
            if (elementList.Count <= 0)
            {
                MessageBox.Show(Resources.str_bent_2_elem, Resources.str_demesio, MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

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
                var thread = new Thread(() => ShowResult(_result)) {IsBackground = true};

                thread.Start();
            }

            

            //MessageBox.Show(_result);

            log.LogAction("Button spin pressed");
        }

        private void Btn_Add_Click(object sender, EventArgs e)
        {
            log.LogAction("Button add pressed");
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

        public void ShowResult(string result)
        {
            Thread.Sleep(17000);
            
            MessageBox.Show(Resources.str_laimejimas + elementList[int.Parse(result)-1], Resources.str_sveikinimas, MessageBoxButtons.OK);
            UpdateButtonSpinEnabled(true);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogAction("Application exit");
        }

        private void Btn_Close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void UpdateButtonSpinEnabled(bool value)
        {
            btn_spin.BeginInvoke(new MethodInvoker(delegate { btn_spin.Enabled = value; }));
        }
    }
}
