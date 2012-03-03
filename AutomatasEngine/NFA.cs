using System;
using System.Collections.Generic;

namespace AutomatasEngine
{
	public class NFA<TStateValue, TInputSymbol>
	{
		private HashSet<State<TStateValue>> _states;
		private Func<State<TStateValue>, TInputSymbol, State<TStateValue>> _transitionFunction;
		private State<TStateValue> _initialState;
		private State<TStateValue> _finalState;

		public NFA()
		{
			
		}
	}
}
