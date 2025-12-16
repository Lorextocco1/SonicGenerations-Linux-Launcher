# Sonic Generations - ROG Ally & Linux Custom Launcher 🦔

Un Launcher nativo in C# creato per risolvere i problemi di schermo nero o crash degli script su Linux (Steam Deck, ROG Ally, BazziteOS) quando si modda **Sonic Generations**.

## 🛑 Il Problema
Su Linux/Proton, i vecchi metodi (`.bat`, `.hta` o file batch semplici) spesso crashano o non riescono ad aprire il **Configuration Tool** perché Wine/Proton gestisce male il mix tra console e finestre grafiche.

## ✅ La Soluzione (Custom .EXE)
Questo progetto usa un trucco di "auto-compilazione". Utilizza il compilatore C# (`csc.exe`) già presente dentro Proton per trasformare questo codice in una vera Applicazione Windows Nativa (`.exe`) direttamente sul tuo dispositivo.
Risultato: **Un launcher grafico, stabile, con supporto mouse/touch e zero crash.**

## 🚀 Istruzioni di Installazione

1. Scarica i 3 file di questo progetto (`LauncherCode.cs`, `CostruisciApp.bat`, `logo.png`).
2. Copiali nella cartella principale di Sonic Generations (dove si trova `SonicGenerations.exe`).
3. **Su Steam:**
   - Aggiungi `CostruisciApp.bat` come "Gioco non di Steam" (o impostalo momentaneamente come destinazione del gioco).
   - Forza la compatibilità: **Proton Experimental** (o Proton 8.0).
   - Avvialo **UNA VOLTA**.
4. Se tutto va bene, apparirà un nuovo file **`SonicLauncher.exe`** nella tua cartella.
5. **Passo Finale:**
   - Imposta il collegamento di Steam per puntare a `SonicLauncher.exe`.
   - Pulisci le opzioni di avvio (lasciale vuote).
   - Gioca!

## 🛠️ Contenuto
- `LauncherCode.cs`: Il codice sorgente del Menu.
- `CostruisciApp.bat`: Lo script che compila l'app usando il framework interno di Proton.
- `logo.png`: L'asset grafico.

Buon divertimento alla massima velocità! 🔵💨
