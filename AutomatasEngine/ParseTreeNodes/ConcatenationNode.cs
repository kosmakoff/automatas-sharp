using System.Collections.Generic;

namespace AutomatasEngine.ParseTreeNodes
{
	public sealed class ConcatenationNode : ParseTreeNode
	{
		private ConcatenationNode()
		{
			Type = NodeType.Concat;
		}

		public ConcatenationNode(List<ParseTreeNode> nodes)
			: this()
		{
			Nodes = nodes;
		}

		public List<ParseTreeNode> Nodes { get; private set; }
	}
}
