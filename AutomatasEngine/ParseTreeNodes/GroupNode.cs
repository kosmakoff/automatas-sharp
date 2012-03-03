namespace AutomatasEngine.ParseTreeNodes
{
	public sealed class GroupNode : ParseTreeNode
	{
		private GroupNode()
		{
			Type = NodeType.Group;
		}

		public GroupNode(ParseTreeNode[] nodes)
			: this()
		{
			Nodes = nodes;
		}

		public ParseTreeNode[] Nodes { get; private set; }
	}
}
