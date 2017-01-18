using System;

namespace Assets.Scripts
{
	public class MockKopfhoererDetektor : IKopfhoererDetektor
	{
		private static string _name = "Mock Impl.";

		public MockKopfhoererDetektor ()
		{
		}

		#region IKopfhoererDetektor implementation

		public bool istVerbunden ()
		{
			return true;
		}

		public string name () {
			return _name;
		}
		#endregion
	}
}

