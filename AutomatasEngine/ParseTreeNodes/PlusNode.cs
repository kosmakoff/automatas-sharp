namespace AutomatasEngine.ParseTreeNodes
{
	public sealed class PlusNode : ParseTreeNode
	{
		private PlusNode()
		{
			Type = NodeType.Plus;
		}

		public PlusNode(ParseTreeNode node)
			: this()
		{
			Node = node;
		}

		public ParseTreeNode Node { get; private set; }
	}
}
