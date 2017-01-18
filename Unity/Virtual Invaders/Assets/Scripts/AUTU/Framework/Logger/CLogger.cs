using System;
using System.Diagnostics;
using UnityEngine;

namespace AUTU
{
  public class CLogger : ILogger
  {
    public bool ConsoleEin { get; set; }
    public bool FehlerConsoleEin { get; set; }

    public void Log(string Text, StackTrace StackDiagnose = null)
    {
      if (StackDiagnose == null)
      {
        StackDiagnose = new StackTrace(1, true);
      }
      StackFrame Frame = StackDiagnose.GetFrame(0);

      if (ConsoleEin)
      {
        Text += "\n";
        Text += "-> Datei: " + Frame.GetFileName() + " (" + Frame.GetFileLineNumber() + ")";
        UnityEngine.Debug.Log(Text);
      }
    }

    public void LogWarning(string Text, StackTrace StackDiagnose = null)
    {
      if (StackDiagnose == null)
      {
        StackDiagnose = new StackTrace(1, true);
      }
      StackFrame Frame = StackDiagnose.GetFrame(0);

      if (FehlerConsoleEin)
      {
        Text += "\n";
        Text += "-> Datei: " + Frame.GetFileName() + " (" + Frame.GetFileLineNumber() + ")";
        UnityEngine.Debug.LogWarning(Text);
      }
    }

    public void LogError(string Text, StackTrace StackDiagnose = null)
    {
      if (StackDiagnose == null)
      {
        StackDiagnose = new StackTrace(1, true);
      }
      StackFrame Frame = StackDiagnose.GetFrame(0);

      if (FehlerConsoleEin)
      {
        Text += "\n";
        Text += "-> Datei: " + Frame.GetFileName() + " (" + Frame.GetFileLineNumber() + ")";
        UnityEngine.Debug.LogError(Text);
      }
    }

    public void FalscherWert(string Text, object ErwarteterWert, object TatseachlicherWert, StackTrace StackDiagnose = null)
    {
      if (StackDiagnose == null)
      {
        StackDiagnose = new StackTrace(1, true);
      }
      StackFrame Frame = StackDiagnose.GetFrame(0);

      if (FehlerConsoleEin)
      {
        Text += "\n";
        Text += "-> Erwarteter Wert: " + ErwarteterWert + "; ";
        Text += "Tatsächlicher Wert: " + TatseachlicherWert + "\n";
        Text += "-> Datei: " + Frame.GetFileName() + " (" + Frame.GetFileLineNumber() + ")";
        UnityEngine.Debug.LogError(Text);
      }
    }

    public void ExceptionIstAufgetreten(string Text, Exception DieseExeption, StackTrace StackDiagnose = null)
    {
      if (StackDiagnose == null)
      {
        StackDiagnose = new StackTrace(1, true);
      }
      StackFrame Frame = StackDiagnose.GetFrame(0);

      if (FehlerConsoleEin)
      {
        Text += "\n";
        Text += "-> Exception: " + DieseExeption + "\n";
        Text += "-> Datei: " + Frame.GetFileName() + " (" + Frame.GetFileLineNumber() + ")";
        UnityEngine.Debug.LogError(Text);
      }
    }

    public void FalscherVergleich(string Text, object Vergleichswert, object TatseachlicherWert, StackTrace StackDiagnose = null)
    {
      if (StackDiagnose == null)
      {
        StackDiagnose = new StackTrace(1, true);
      }
      StackFrame Frame = StackDiagnose.GetFrame(0);

      if (FehlerConsoleEin)
      {
        Text += "\n";
        Text += "-> Vergleichswert: " + Vergleichswert + "; ";
        Text += "Tatsächlicher Wert: " + TatseachlicherWert + "\n";
        Text += "-> Datei: " + Frame.GetFileName() + " (" + Frame.GetFileLineNumber() + ")";
        UnityEngine.Debug.LogError(Text);
      }
    }

    public void WertAusserhalbToleranz(string Text, object UntereGrenze, object ObereGrenze, object TatseachlicherWert, StackTrace StackDiagnose = null)
    {
      if (StackDiagnose == null)
      {
        StackDiagnose = new StackTrace(1, true);
      }
      StackFrame Frame = StackDiagnose.GetFrame(0);

      if (FehlerConsoleEin)
      {
        Text += "\n";
        Text += "-> Intervall: " + UntereGrenze + " - " + ObereGrenze+ "; ";
        Text += "Tatsächlicher Wert: " + TatseachlicherWert + "\n";
        Text += "-> Datei: " + Frame.GetFileName() + " (" + Frame.GetFileLineNumber() + ")";
        UnityEngine.Debug.LogError(Text);
      }
    }

    public CLogger()
    {
      ConsoleEin = true;
      FehlerConsoleEin = true;
    }
  }
}
