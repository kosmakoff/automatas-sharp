using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomatasEngine.CharacterClassNode;
using CCN = AutomatasEngine.CharacterClassNode;
using PTN = AutomatasEngine.ParseTreeNodes;

namespace AutomatasEngine
{
	public class ParseTree
	{
		public PTN.ParseTreeNode Root { get; private set; }

		public static ParseTree Create(string pattern)
		{
			var enumerator = pattern.GetEnumerator();

			var groupCount = 0;
			var rootNode = MergeNodes(GetParseTreeNodes(enumerator, ref groupCount));

			if (groupCount != 0)
				throw new Exception("Not enough closing parentheses");

			return new ParseTree {Root = rootNode};
		}

		private static PTN.ParseTreeNode[] GetParseTreeNodes(CharEnumerator enumerator, ref int groupCount)
		{
			var nodesStack = new Stack<PTN.ParseTreeNode>();

			bool forceStop = false;

			while (!forceStop && enumerator.MoveNext())
			{
				switch (enumerator.Current)
				{
					case '(':
						groupCount++;
						nodesStack.Push(new PTN.GroupNode(GetParseTreeNodes(enumerator, ref groupCount)));
						break;

					case ')':
						forceStop = true;
						groupCount--;
						if (groupCount < 0)
							throw new Exception("Too much closing parentheses");
						break;

					case '[':
						var charClassNodes = GetCharClassNodes(enumerator);
						nodesStack.Push(new PTN.CharacterClassNode(charClassNodes));

						break;

					case '{':
						int? min, max;

						GetQuantityParams(enumerator, out min, out max);
						nodesStack.Push(new PTN.QuantityNode(min, max, nodesStack.Pop()));

						break;

					case '*':
						// star
						nodesStack.Push(new PTN.StarNode(nodesStack.Pop()));
						break;

					case '?':
						// questionmark
						nodesStack.Push(new PTN.QuestionNode(nodesStack.Pop()));
						break;

					case '+':
						// plus
						nodesStack.Push(new PTN.PlusNode(nodesStack.Pop()));
						break;

					case '|':
						// alteration
						// get all previous items to a list
						var nodesList = nodesStack.Reverse().ToArray();

						if (nodesList[0] is PTN.AlternationNode)
						{
							var altNode = (PTN.AlternationNode)nodesList[0];
							var restNodes = nodesList.Skip(1).ToArray();
							var newNode = MergeNodes(restNodes);
							var altList = altNode.Nodes;
							altList.Add(newNode);
							nodesStack.Clear();
							nodesStack.Push(altNode);
						}
						else
						{
							nodesStack.Clear();
							nodesStack.Push(new PTN.AlternationNode(new List<PTN.ParseTreeNode> {MergeNodes(nodesList)}));
						}

						break;

					case '\\':
						// special symbol!
						throw new NotImplementedException();

					default:
						// all other characters
						nodesStack.Push(new PTN.CharNode(enumerator.Current));
						break;
				}
			}

			FinalizeGroup(nodesStack);
			return nodesStack.Reverse().ToArray();
		}

		private static void GetQuantityParams(CharEnumerator enumerator, out int? min, out int? max)
		{
			var sb = new StringBuilder(8);
			int comasCount = 0;
			bool wasClosed = false;

			while (enumerator.MoveNext())
			{
				var c = enumerator.Current;

				if (char.IsDigit(c) || c == ',')
				{
					if (c == ',')
						comasCount++;

					if (comasCount > 1)
						throw new Exception("Too many comas in quantity modifier definition");

					sb.Append(c);
					continue;
				}

				if (c == '}')
				{
					wasClosed = true;
					break;
				}

				throw new Exception("Invalid character in quantity modifier");
			}

			if (!wasClosed)
				throw new Exception("Unexpected end of pattern in quantity modifier");

			var s = sb.ToString();
			if (s.Length == 0)
				throw new Exception("Empty quantity modifier");

			if (s == ",")
				throw new Exception("Quantity modifier contains no range limits");

			var comaPos = s.IndexOf(',');
			if (comaPos < 0)
			{
				min = max = int.Parse(s);
			}
			else if (comaPos == 0)
			{
				min = null;
				max = int.Parse(s.Substring(1));
			}
			else if (comaPos == s.Length - 1)
			{
				max = null;
				min = int.Parse(s.Substring(0, s.Length - 1));
			}
			else
			{
				min = int.Parse(s.Substring(0, comaPos));
				max = int.Parse(s.Substring(comaPos + 1));
			}
		}

		private static CCN.CharacterClassNode[] GetCharClassNodes(CharEnumerator enumerator)
		{
			var nodesStack = new Stack<CCN.CharacterClassNode>();
			bool forceStop = false;

			while (!forceStop && enumerator.MoveNext())
			{
				switch (enumerator.Current)
				{
					case '^':
						if (!nodesStack.Any())
							nodesStack.Push(new CCN.NegationNode());
						else
							goto default;

						break;

					case '-':
						// it's just a dash that occurrs either at the very beginning or after "^"
						if (nodesStack.Any() && (nodesStack.Count() != 1 || !(nodesStack.Peek() is CCN.NegationNode)))
						{
							// it's range dash

							if (nodesStack.Peek() is CCN.RangeNode)
								throw new Exception("Unexpected dash after character range");

							var prevSymbol = ((CharNode)nodesStack.Pop()).Value;
							if (!enumerator.MoveNext())
								throw new Exception("Unexpected end of range is character class");

							var endSymbol = enumerator.Current;

							nodesStack.Push(new CCN.RangeNode(prevSymbol, endSymbol));
						}
						else
							goto default;

						break;

					case '\\':
						// special symbols include backslash, opening and closing square bracket
						if (!enumerator.MoveNext())
							throw new Exception("Unexpected end of pattern after backslash");

						if (!(new[] {'\\', '[', ']'}.Contains(enumerator.Current)))
							throw new Exception("Invalide escape sequence");

						goto default;

					case ']':
						forceStop = true;
						break;

					default:
						nodesStack.Push(new CharNode(enumerator.Current));
						break;
				}
			}

			return nodesStack.Reverse().ToArray();
		}

		private static PTN.ParseTreeNode MergeNodes(IList<PTN.ParseTreeNode> restNodes)
		{
			if (restNodes.Count == 0)
				return new PTN.NullNode();

			if (restNodes.Count == 1)
				return restNodes[0];

			return new PTN.ConcatenationNode(restNodes.ToList());
		}

		private static void FinalizeGroup(Stack<PTN.ParseTreeNode> nodesStack)
		{
			if (!nodesStack.Any())
			{
				nodesStack.Clear();
				nodesStack.Push(new PTN.NullNode());

				return;
			}

			if (nodesStack.Count == 1)
				return;

			var nodesList = nodesStack.Reverse().ToArray();
			nodesStack.Clear();

			if (nodesList[0] is PTN.AlternationNode)
			{
				var altNode = (PTN.AlternationNode)nodesList[0];
				var restNodes = nodesList.Skip(1).ToArray();
				var newNode = MergeNodes(restNodes);
				var altList = altNode.Nodes;
				altList.Add(newNode);
				nodesStack.Push(altNode);
			}
			else
			{
				nodesStack.Push(MergeNodes(nodesList));
			}
		}
	}
}
