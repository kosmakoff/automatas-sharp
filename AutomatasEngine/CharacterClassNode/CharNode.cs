namespace AutomatasEngine.CharacterClassNode
{
	public sealed class CharNode : CharacterClassNode
	{
		public char Value { get; private set; }

		public CharNode(char value)
		{
			Value = value;
		}
	}
}
