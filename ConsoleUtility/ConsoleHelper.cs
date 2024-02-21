namespace ConsoleUtility
{
	public static class ConsoleHelper
	{
		public static void SendMessage(string _message)
		{
			Console.WriteLine(_message);
		}
		public static string SendQuestionAndReceiveResponse(string _prompt)
		{
			SendMessage(_prompt);
			var _output = Console.ReadLine();

			if (string.IsNullOrEmpty(_output) || string.IsNullOrWhiteSpace(_output))
			{
				return string.Empty;
			}
			return _output;
		}
	}
}
