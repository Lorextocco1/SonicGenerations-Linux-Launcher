using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.IO;

namespace SonicLauncher
{
    // Classe per i bottoni trasparenti
    public class TransparentButton : Button
    {
        public TransparentButton()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
        }
    }

    public class MainForm : Form
    {
        private Button btnPlay;
        private Button btnConfig;
        private Button btnMods;
        private Button btnExit;

        // Colori e stile (Blu Sonic)
        Color sonicBlue = Color.FromArgb(0, 100, 255);
        Color sonicDark = Color.FromArgb(0, 0, 50);
        Color glassEffect = Color.FromArgb(180, 0, 20, 60);

        int leftMargin = 50; 

        public MainForm()
        {
            // --- 0. FIX CRITICO: INIEZIONE REGISTRO (STEAMFAKE) ---
            // Questo viene eseguito PRIMA di mostrare la finestra
            try {
                string regFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "steamfake.reg");
                if (File.Exists(regFile)) {
                    // Esegue regedit in modalità silenziosa (/s)
                    Process.Start("regedit", $"/s \"{regFile}\"");
                }
            } catch {
                // Se fallisce, continuiamo lo stesso per non bloccare il launcher
            }

            // --- 1. SETUP FINESTRA ---
            this.Text = "Sonic Generations - Linux Launcher";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = sonicDark;

            // Icona del gioco
            try {
                string gameExe = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SonicGenerations.exe");
                if (File.Exists(gameExe)) this.Icon = Icon.ExtractAssociatedIcon(gameExe);
            } catch {}

            // Sfondo
            string wallPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wallpaper.jpg");
            if (File.Exists(wallPath)) {
                this.BackgroundImage = Image.FromFile(wallPath);
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }

            // --- 2. BOTTONI ---
            int startY = 250;
            int spacing = 70;

            // Tasto 1: GIOCA
            btnPlay = CreateSonicButton("GIOCA", startY);
            btnPlay.Click += (sender, e) => { 
                LanciaApp("SonicGenerations.exe");
                this.Close();
            };

            // Tasto 2: CONFIGURAZIONE (Importante per Sonic)
            btnConfig = CreateSonicButton("CONFIGURAZIONE GRAFICA", startY + spacing);
            btnConfig.Click += (sender, e) => { 
                LanciaApp("ConfigurationTool.exe");
            };

            // Tasto 3: MOD MANAGER (Se presente)
            btnMods = CreateSonicButton("HEDGE MOD MANAGER", startY + spacing * 2);
            btnMods.Click += (sender, e) => { 
                // Cerca l'eseguibile classico del mod manager
                if (File.Exists("HedgeModManager.exe")) LanciaApp("HedgeModManager.exe");
                else MessageBox.Show("HedgeModManager.exe non trovato nella cartella.");
            };

            // Tasto 4: ESCI
            btnExit = CreateSonicButton("ESCI", startY + spacing * 3);
            btnExit.Click += (sender, e) => { this.Close(); };

            // Firma
            Label footer = new Label();
            footer.Text = "SONIC LINUX HUB | LOREXTHEGAMER";
            footer.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            footer.ForeColor = Color.White;
            footer.AutoSize = true;
            footer.BackColor = Color.Transparent;
            footer.Location = new Point(10, this.ClientSize.Height - 25);
            this.Controls.Add(footer);
        }

        // Funzione per creare i bottoni stile Sonic
        private Button CreateSonicButton(string text, int top)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Size = new Size(300, 60);
            btn.Location = new Point(leftMargin, top);
            btn.FlatStyle = FlatStyle.Flat;
            btn.Font = new Font("Segoe UI Black", 14, FontStyle.Bold);
            btn.ForeColor = Color.White;
            btn.BackColor = glassEffect;
            btn.FlatAppearance.BorderColor = Color.White;
            btn.FlatAppearance.BorderSize = 2;
            btn.Cursor = Cursors.Hand;

            btn.MouseEnter += (s, e) => { 
                btn.BackColor = sonicBlue; // Diventa blu Sonic al passaggio
                btn.ForeColor = Color.Yellow; // Scritta gialla stile anelli
            };
            btn.MouseLeave += (s, e) => { 
                btn.BackColor = glassEffect;
                btn.ForeColor = Color.White;
            };

            this.Controls.Add(btn);
            return btn;
        }

        // Funzione di avvio sicuro
        private void LanciaApp(string exeName)
        {
            try {
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, exeName);
                if (File.Exists(fullPath)) {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = fullPath;
                    startInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    Process.Start(startInfo);
                } else {
                    MessageBox.Show("File mancante: " + exeName, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
