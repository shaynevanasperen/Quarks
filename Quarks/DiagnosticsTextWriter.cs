using System.Diagnostics;
using System.IO;
using System.Text;

namespace Quarks
{
	[DebuggerNonUserCode]
	class DiagnosticsTextWriter : TextWriter
	{
		public override void Write(char[] buffer, int index, int count)
		{
			Debug.Write(new string(buffer, index, count));
		}

		public override void Write(string value)
		{
			Debug.Write(value);
		}

		public override Encoding Encoding
		{
			get { return Encoding.Default; }
		}
	}
}
