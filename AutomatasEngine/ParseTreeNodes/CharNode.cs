namespace AutomatasEngine.ParseTreeNodes
{
	public sealed class CharNode : ParseTreeNode
	{
		private CharNode()
		{
			Type = NodeType.Char;
		}

		public CharNode(char value):this()
		{
			Value = value;
		}

		public char Value { get; private set; }

		public override string ToString()
		{
			return string.Format("Char: {0}", Value);
		}
	}
}
