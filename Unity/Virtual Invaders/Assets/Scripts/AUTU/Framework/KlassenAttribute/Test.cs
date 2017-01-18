using System;

namespace AUTU
{
  /// <summary>
  /// Ein Attribut um eine Methode als AUTU-TestCase zu markieren.
  /// Befindet sich solch eine Methode in einer AUTU.TestGruppe, dann kann die Methode als TestCase ausgeführt werden.
  /// </summary>
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
  public class Test : Attribute
  {
  }
}
