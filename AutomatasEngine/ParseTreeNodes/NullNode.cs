namespace AutomatasEngine.ParseTreeNodes
{
	public sealed class NullNode : ParseTreeNode
	{
		public NullNode()
		{
			Type = NodeType.Null;
		}
	}
}
