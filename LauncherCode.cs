using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Diagnostics;
using System.IO;

namespace SonicLauncher
{
    // --- CLASSE BOTTONE "SONIC STYLE" (Trasparente e Giallo) ---
    public class SonicButton : Label
    {
        private bool isHovered = false;

        public SonicButton()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
            this.DoubleBuffered = true; 
            this.Cursor = Cursors.Hand;
            this.Font = new Font("Arial Black", 24, FontStyle.Italic);
            this.AutoSize = false;
            this.TextAlign = ContentAlignment.MiddleCenter;
        }

        protected override void OnMouseEnter(EventArgs e) { isHovered = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { isHovered = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.TextRenderingHint = TextRenderingHint.AntiAlias; 
            g.SmoothingMode = SmoothingMode.HighQuality;

            string text = this.Text;
            Font font = this.Font;

            SizeF textSize = g.MeasureString(text, font);
            float x = (this.Width - textSize.Width) / 2;
            float y = (this.Height - textSize.Height) / 2;

            // COLORI UFFICIALI SONIC
            Color fillColor = ColorTranslator.FromHtml("#FFD700"); // Giallo Oro
            Color outlineColor = isHovered ? ColorTranslator.FromHtml("#00BFFF") : ColorTranslator.FromHtml("#003399"); // Blu Elettrico
            int offset = isHovered ? 3 : 2; 

            // 1. DISEGNA IL BORDO BLU
            using (Brush outlineBrush = new SolidBrush(outlineColor))
            {
                g.DrawString(text, font, outlineBrush, x - offset, y);
                g.DrawString(text, font, outlineBrush, x + offset, y);
                g.DrawString(text, font, outlineBrush, x, y - offset);
                g.DrawString(text, font, outlineBrush, x, y + offset);
                g.DrawString(text, font, outlineBrush, x - offset, y - offset);
                g.DrawString(text, font, outlineBrush, x + offset, y + offset);
                g.DrawString(text, font, outlineBrush, x - offset, y + offset);
                g.DrawString(text, font, outlineBrush, x + offset, y - offset);
            }

            // 2. DISEGNA IL TESTO GIALLO
            using (Brush fillBrush = new SolidBrush(fillColor))
            {
                g.DrawString(text, font, fillBrush, x, y);
            }
        }
    }
    // --------------------------------------------------

    public class MainForm : Form
    {
        private PictureBox logoBox;
        private SonicButton btnPlay;
        private SonicButton btnConfig;
        private SonicButton btnExit;

        public MainForm()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            // 1. SETUP FINESTRA
            this.Text = "Sonic Generations - Speed Launcher";
            this.Size = new Size(960, 540);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(20, 40, 80); 

            try {
                string gameExe = Path.Combine(baseDir, "SonicGenerations.exe");
                if (File.Exists(gameExe)) this.Icon = Icon.ExtractAssociatedIcon(gameExe);
            } catch {}

            // 2. SFONDO WALLPAPER
            string wallPath = "";
            if (File.Exists(Path.Combine(baseDir, "wallpaper.jpg"))) 
                wallPath = Path.Combine(baseDir, "wallpaper.jpg");
            else if (File.Exists(Path.Combine(baseDir, "wallpaper.png")))
                wallPath = Path.Combine(baseDir, "wallpaper.png");

            if (!string.IsNullOrEmpty(wallPath)) {
                this.BackgroundImage = Image.FromFile(wallPath);
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }

            // 3. LOGO
            logoBox = new PictureBox();
            logoBox.Size = new Size(500, 180);
            logoBox.Location = new Point((this.ClientSize.Width - 500) / 2, 20);
            logoBox.SizeMode = PictureBoxSizeMode.Zoom;
            logoBox.BackColor = Color.Transparent; 

            string logoPath = Path.Combine(baseDir, "logo.png");
            if (File.Exists(logoPath)) {
                try { logoBox.Image = Image.FromFile(logoPath); } catch { }
            }
            this.Controls.Add(logoBox);

            // 4. BOTTONI SONIC
            int startY = 230;
            int spacing = 80;

            btnPlay = CreateSonicButton("GIOCA", startY);
            btnPlay.Click += (sender, e) => { 
                LanciaApp("SonicGenerations.exe"); 
                this.Close(); 
            };

            btnConfig = CreateSonicButton("CONFIGURA", startY + spacing);
            btnConfig.Click += (sender, e) => { 
                LanciaApp("ConfigurationTool.exe"); 
            };

            btnExit = CreateSonicButton("ESCI", startY + spacing * 2);
            btnExit.Click += (sender, e) => { this.Close(); };

            // --- 5. FIRMA (MARCHIO DI FABBRICA) ---
            Label footer = new Label();
            footer.Text = "SONIC CUSTOM LAUNCHER | LOREXTHEGAMER";
            footer.Font = new Font("Arial", 8, FontStyle.Bold);
            // *** MODIFICA QUI: Colore Nero Solido per contrasto ***
            footer.ForeColor = Color.Black; 
            footer.AutoSize = true;
            footer.BackColor = Color.Transparent;
            
            // Posizionamento Bottom-Right preciso
            footer.Location = new Point(this.ClientSize.Width - footer.PreferredWidth - 20, this.ClientSize.Height - 25);
            
            this.Controls.Add(footer);
        }

        private SonicButton CreateSonicButton(string text, int top)
        {
            SonicButton btn = new SonicButton();
            btn.Text = text;
            btn.Size = new Size(350, 70); 
            btn.Location = new Point((this.ClientSize.Width - 350) / 2, top);
            this.Controls.Add(btn); 
            return btn;
        }

        private void LanciaApp(string exeName)
        {
            try {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string fullPath = Path.Combine(baseDir, exeName);

                if (File.Exists(fullPath)) {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = fullPath;
                    startInfo.WorkingDirectory = baseDir;
                    Process.Start(startInfo);
                } else {
                    MessageBox.Show("File non trovato: " + exeName, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } catch (Exception ex) {
                MessageBox.Show("Errore di avvio: " + ex.Message);
            }
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
