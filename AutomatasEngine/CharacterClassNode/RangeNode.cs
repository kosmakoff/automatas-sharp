using System;
using System.Collections;
using System.Collections.Generic;

namespace AutomatasEngine.CharacterClassNode
{
	public sealed class RangeNode : CharacterClassNode, IEnumerable<char>
	{
		public char From { get; private set; }
		public char To { get; private set; }

		public RangeNode(char from ,char to)
		{
			if (from.CompareTo(to) > 0)
				throw new Exception("Invalid character range specification");

			From = from;
			To = to;
		}
		
		public IEnumerator<char> GetEnumerator()
		{
			for (var i = (int)From; i <= (int)To; i++)
				yield return (char)i;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
