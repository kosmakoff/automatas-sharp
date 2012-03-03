namespace AutomatasEngine.ParseTreeNodes
{
	public class QuantityNode : ParseTreeNode
	{
		private QuantityNode()
		{
			Type = NodeType.Quantity;
		}

		public QuantityNode(int? min, int? max, ParseTreeNode node)
			: this()
		{
			Min = min;
			Max = max;
			Node = node;
		}

		public int? Min { get; private set; }
		public int? Max { get; private set; }

		public ParseTreeNode Node { get; private set; }
	}
}
