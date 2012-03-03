namespace AutomatasEngine.ParseTreeNodes
{
	public sealed class StarNode : ParseTreeNode
	{
		private StarNode()
		{
			Type = NodeType.Star;
		}

		public StarNode(ParseTreeNode node)
			: this()
		{
			Node = node;
		}

		public ParseTreeNode Node { get; private set; }
	}
}
