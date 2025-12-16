@echo off
cd /d "%~dp0"
echo Costruzione del Launcher in corso...

:: Cerca il compilatore C# nel percorso standard di .NET Framework (presente in Proton)
set "COMPILER=C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe"

if not exist "%COMPILER%" (
    echo ERRORE: Compilatore non trovato! Assicurati di usare Proton Experimental.
    pause
    exit
)

:: Compila il codice in un vero .EXE senza finestra di console (/target:winexe)
"%COMPILER%" /target:winexe /out:SonicLauncher.exe /r:System.Windows.Forms.dll /r:System.Drawing.dll LauncherCode.cs

if exist "SonicLauncher.exe" (
    echo.
    echo SUCCESSO! SonicLauncher.exe e' stato creato.
    echo Ora puoi impostare SonicLauncher.exe come gioco su Steam.
) else (
    echo ERRORE: La compilazione e' fallita.
)
pause
