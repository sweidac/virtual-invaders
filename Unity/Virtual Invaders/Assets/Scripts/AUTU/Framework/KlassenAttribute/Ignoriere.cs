using System;

namespace AUTU
{
  /// <summary>
  /// Ein Attribut um ein AUTU-Testcase zu ignorieren.
  /// </summary>
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
  public class Ignoriere : Attribute
  {
  }
}
