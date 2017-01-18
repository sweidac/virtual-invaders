using NUnit.Framework;

namespace AUTU
{
  class TestCTestOptionen
  {
    [NUnit.Framework.Test]
    public void Intitialwerte()
    {
      ITestOptionen Optionen = new CTestOptionen();

      Assert.AreEqual(true, Optionen.KonsolenLogging);
      Assert.AreEqual(true, Optionen.AbbruchBeiFehler);
    }

    [NUnit.Framework.Test]
    public void KonsolenLogging()
    {
      ITestOptionen Optionen = new CTestOptionen();

      Optionen.setKonsolenLogging(TestOptionen.LoggingStatus.Aus);

      Assert.AreEqual(false, Optionen.KonsolenLogging);

      Optionen.setKonsolenLogging(TestOptionen.LoggingStatus.Vererbt);

      Assert.AreEqual(true, Optionen.KonsolenLogging);

      Optionen.setKonsolenLogging(TestOptionen.LoggingStatus.Ein);

      Assert.AreEqual(true, Optionen.KonsolenLogging);
    }

    [NUnit.Framework.Test]
    public void KonsolenLoggingMerge()
    {
      ITestOptionen HauptOptionen = new CTestOptionen();
      ITestOptionen UnterOptionen = new CTestOptionen();

      HauptOptionen.setKonsolenLogging(TestOptionen.LoggingStatus.Aus);
      UnterOptionen.setKonsolenLogging(TestOptionen.LoggingStatus.Vererbt);

      Assert.AreEqual(false, UnterOptionen.merge(HauptOptionen).KonsolenLogging);

      HauptOptionen.setKonsolenLogging(TestOptionen.LoggingStatus.Ein);
      UnterOptionen.setKonsolenLogging(TestOptionen.LoggingStatus.Vererbt);

      Assert.AreEqual(true, UnterOptionen.merge(HauptOptionen).KonsolenLogging);

      HauptOptionen.setKonsolenLogging(TestOptionen.LoggingStatus.Ein);
      UnterOptionen.setKonsolenLogging(TestOptionen.LoggingStatus.Aus);

      Assert.AreEqual(false, UnterOptionen.merge(HauptOptionen).KonsolenLogging);

      HauptOptionen.setKonsolenLogging(TestOptionen.LoggingStatus.Aus);
      UnterOptionen.setKonsolenLogging(TestOptionen.LoggingStatus.Ein);

      Assert.AreEqual(true, UnterOptionen.merge(HauptOptionen).KonsolenLogging);
    }

    [NUnit.Framework.Test]
    public void AbbruchBeiFehlerMerge()
    {
      ITestOptionen HauptOptionen = new CTestOptionen();
      ITestOptionen UnterOptionen = new CTestOptionen();

      HauptOptionen.AbbruchBeiFehler = true;
      UnterOptionen.AbbruchBeiFehler = false;

      Assert.AreEqual(true, UnterOptionen.merge(HauptOptionen).AbbruchBeiFehler);

      HauptOptionen.AbbruchBeiFehler = true;
      UnterOptionen.AbbruchBeiFehler = true;

      Assert.AreEqual(true, UnterOptionen.merge(HauptOptionen).AbbruchBeiFehler);

      HauptOptionen.AbbruchBeiFehler = false;
      UnterOptionen.AbbruchBeiFehler = true;

      Assert.AreEqual(false, UnterOptionen.merge(HauptOptionen).AbbruchBeiFehler);

      HauptOptionen.AbbruchBeiFehler = false;
      UnterOptionen.AbbruchBeiFehler = false;

      Assert.AreEqual(false, UnterOptionen.merge(HauptOptionen).AbbruchBeiFehler);
    }

  }
}
