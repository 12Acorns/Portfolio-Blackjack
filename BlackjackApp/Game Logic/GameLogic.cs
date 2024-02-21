using ConsoleUtility;

namespace BlackjackApp.Game_Logic
{
	internal static class GameLogic
	{
		private const string newCardPrompt =
			"Do you want to hit? \n" +
			"[Hit/Pass, default Hit]";

		public static bool AskForUserToGetNewCard()
		{
			while (true)
			{
				var _response = ConsoleHelper
					.SendQuestionAndReceiveResponse(newCardPrompt);

				switch(_response.ToLowerInvariant())
				{
					case "hit":
						return true;
					case "pass":
						return false;
					case "" or " ":
						return true;
					case "clear":
						Console.Clear();
						continue;
					case "view":
						Console.Clear();
						PlayerLogic.Instance.PromptCurrentCards();
						continue;
					default:
						continue;
				}
			}
		}
	}
}
