using System.Collections.Generic;

namespace AutomatasEngine.ParseTreeNodes
{
	public sealed class AlternationNode : ParseTreeNode
	{
		private AlternationNode()
		{
			Type = NodeType.Alteration;
		}

		public AlternationNode(List<ParseTreeNode> nodes)
			: this()
		{
			Nodes = nodes;
		}

		public List<ParseTreeNode> Nodes { get; private set; }
	}
}
