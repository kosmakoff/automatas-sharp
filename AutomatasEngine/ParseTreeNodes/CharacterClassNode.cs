using System;
using System.Collections.Generic;
using System.Linq;
using AutomatasEngine.CharacterClassNodes;
using CCN = AutomatasEngine.CharacterClassNode;

namespace AutomatasEngine.ParseTreeNodes
{
	public class CharacterClassNode : ParseTreeNode
	{
		private CharacterClassNode()
		{
			Type = NodeType.CharacterClass;
		}

		public CharacterClassNode(IList<CharacterClassNodes.CharacterClassNode> charClassNodes)
			: this()
		{
			if (!charClassNodes.Any())
				throw new Exception("Empty character class");

			bool isNegationMode = charClassNodes[0] is NegationNode;

			var remainingNodes = charClassNodes.Skip(isNegationMode ? 1 : 0).ToArray();

			if (!remainingNodes.Any())
				throw new Exception("Empty character class with only negation present");

			var charNodesList = new List<char>(32);

			foreach (var characterClassNode in remainingNodes)
			{
				if (characterClassNode is CharacterClassNodes.CharNode)
					charNodesList.Add(((CharacterClassNodes.CharNode)characterClassNode).Value);
				else if (characterClassNode is RangeNode)
					charNodesList.AddRange((RangeNode)characterClassNode);
			}

			IsNegationMode = isNegationMode;
			Characters = charNodesList.ToArray();
		}

		public bool IsNegationMode { get; private set; }
		public char[] Characters { get; private set; }
	}
}
