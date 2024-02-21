using BlackjackApp.Game_Logic;
using ConsoleUtility;

namespace BlackjackApp
{
	internal sealed class Program
	{
		private const string helpPrompt =
			"Console commands are: \n" +
			"Clear/clear (to clear terminal), \n" +
			"View/view (to see current deck, will clear terminal) \n";

		static void Main(string[] _)
		{
			ConsoleHelper.SendMessage(helpPrompt);

			new GameManager()
				.StartGame();

			Console.ReadLine();
		}
	}
}
