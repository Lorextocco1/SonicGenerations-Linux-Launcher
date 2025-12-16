using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.IO;

namespace SonicLauncher
{
    public class MainForm : Form
    {
        private PictureBox logoBox;
        private Button btnPlay;
        private Button btnConfig;
        private Button btnExit;

        public MainForm()
        {
            // 1. IMPOSTAZIONI FINESTRA
            this.Text = "Sonic Generations - ROG Ally Launcher";
            this.Size = new Size(900, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ColorTranslator.FromHtml("#001242"); // Blu SEGA
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Icon = Icon.ExtractAssociatedIcon("SonicGenerations.exe");

            // 2. LOGO (Carica logo.png se esiste)
            logoBox = new PictureBox();
            logoBox.Size = new Size(600, 250);
            logoBox.Location = new Point((900 - 600) / 2 - 10, 30); // Centrato
            logoBox.SizeMode = PictureBoxSizeMode.Zoom;
            logoBox.BackColor = Color.Transparent;

            if (File.Exists("logo.png"))
            {
                try { logoBox.Image = Image.FromFile("logo.png"); }
                catch { }
            }
            this.Controls.Add(logoBox);

            // 3. BOTTONI
            btnPlay = CreateButton("[ 1 ]  GIOCA", 340);
            btnPlay.Click += (sender, e) => { LanciaApp("SonicGenerations.exe"); this.Close(); };

            btnConfig = CreateButton("[ 2 ]  CONFIGURA", 410);
            btnConfig.Click += (sender, e) => { LanciaApp("ConfigurationTool.exe"); };

            btnExit = CreateButton("[ 3 ]  ESCI", 480);
            btnExit.Click += (sender, e) => { this.Close(); };

            // Footer
            Label footer = new Label();
            footer.Text = "ROG ALLY CUSTOM LAUNCHER .EXE";
            footer.ForeColor = Color.Gray;
            footer.AutoSize = true;
            footer.Location = new Point(360, 580);
            this.Controls.Add(footer);
        }

        private Button CreateButton(string text, int top)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Location = new Point((900 - 300) / 2 - 10, top);
            btn.Size = new Size(300, 50);
            btn.FlatStyle = FlatStyle.Flat;
            btn.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            btn.ForeColor = ColorTranslator.FromHtml("#FFD700"); // Oro
            btn.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#FFD700");
            btn.FlatAppearance.BorderSize = 2;
            btn.Cursor = Cursors.Hand;
            
            // Effetto Hover Semplice
            btn.MouseEnter += (s, e) => { btn.BackColor = ColorTranslator.FromHtml("#FFD700"); btn.ForeColor = ColorTranslator.FromHtml("#001242"); };
            btn.MouseLeave += (s, e) => { btn.BackColor = Color.Transparent; btn.ForeColor = ColorTranslator.FromHtml("#FFD700"); };
            
            this.Controls.Add(btn);
            return btn;
        }

        private void LanciaApp(string exeName)
        {
            try
            {
                if (File.Exists(exeName))
                {
                    Process.Start(exeName);
                }
                else
                {
                    MessageBox.Show("File non trovato: " + exeName, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore avvio: " + ex.Message);
            }
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new MainForm());
        }
    }
}
