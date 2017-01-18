using NUnit.Framework;

namespace AUTU
{
  class TestCLogger
  {
    [NUnit.Framework.Test]
    public void InitialeWerte()
    {
      ILogger Logger = new CLogger();

      Assert.AreEqual(true, Logger.ConsoleEin);
    }
  }
}
