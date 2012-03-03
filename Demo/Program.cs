using AutomatasEngine;

namespace Demo
{
	class Program
	{
		static void Main(string[] args)
		{
			const string pattern = @"(li|e)*n?[a-z]+[qa-f]*[-a-z]?[^a-f][^\]][^-a-fs]{,9}(i|ea|ololo)+el*e{1,7}";
			//const string pattern = @"gray|grey";
			//var nfa = StateMachine.RenderNFA(pattern);
			var tree = ParseTree.Create(pattern);
		}
	}
}
