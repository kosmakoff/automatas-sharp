using System;
using System.Collections.Generic;
using System.Linq;
using CCN = AutomatasEngine.CharacterClassNode;

namespace AutomatasEngine.ParseTreeNodes
{
	public class CharacterClassNode : ParseTreeNode
	{
		private CharacterClassNode()
		{
			Type = NodeType.CharacterClass;
		}

		public CharacterClassNode(IList<CCN.CharacterClassNode> charClassNodes)
			: this()
		{
			if (!charClassNodes.Any())
				throw new Exception("Empty character class");

			bool isNegationMode = charClassNodes[0] is CCN.NegationNode;

			var remainingNodes = charClassNodes.Skip(isNegationMode ? 1 : 0).ToArray();

			if (!remainingNodes.Any())
				throw new Exception("Empty character class with only negation present");

			var charNodesList = new List<char>(32);

			foreach (var characterClassNode in remainingNodes)
			{
				if (characterClassNode is CCN.CharNode)
					charNodesList.Add(((CCN.CharNode)characterClassNode).Value);
				else if (characterClassNode is CCN.RangeNode)
					charNodesList.AddRange((CCN.RangeNode)characterClassNode);
			}

			IsNegationMode = isNegationMode;
			Characters = charNodesList.ToArray();
		}

		public bool IsNegationMode { get; private set; }
		public char[] Characters { get; private set; }
	}
}
