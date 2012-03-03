namespace AutomatasEngine.ParseTreeNodes
{
	public sealed class QuestionNode : ParseTreeNode
	{
		private QuestionNode()
		{
			Type = NodeType.Question;
		}

		public QuestionNode(ParseTreeNode node)
			: this()
		{
			Node = node;
		}

		public ParseTreeNode Node { get; private set; }
	}
}
