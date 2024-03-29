﻿using BlackjackApp.Card;
using ConsoleUtility;

namespace BlackjackApp.Game_Logic
{
	internal sealed class GameManager
	{
		private const int startingCards = 2;
		private const string welcomePrompt =
			"Welcome to blackjack. Do you want to play? \n" +
			"[Y/N, default Y]";

		private const string newGamePrompt =
			"You lost! \n" +
			"Do you want to go again? \n" +
			"[Y/N, default Y]";

		private const string waitForGameToBeginPrompt =
			"Game has not started yet.";

		private const string wonGamePrompt =
			"You Won! \n" +
			"Do you want to go again? \n" +
			"[Y/N, default Y]";

		private CardDeck deck = new();

		private bool gameInited;

		public GameManager()
		{
			InitValues();
			gameInited = true;
		}
		public GameManager StartGame()
		{
			if (!gameInited)
			{
				NewGame();
				return this;
			}
			InitGame(GameState.FirstGame);
			return this;
		}
		public GameManager NewGame()
		{
			InitValues();
			InitGame(GameState.NewGame);
			return this;
		}
		private void InitValues()
		{
			deck = new CardDeck();
			PlayerLogic.Instance.Reset();
			DealerLogic.Instance.Reset();
			gameInited = false;
		}
		private void InitGame(GameState _state)
		{
			bool _respondedCorrectly = false;
			var _greetMessage =
				_state switch
				{
					GameState.WonGame => wonGamePrompt,
					GameState.NewGame => newGamePrompt,
					GameState.FirstGame => welcomePrompt,
					_ => welcomePrompt
				};

			do
			{
				var _response =
					ConsoleHelper
						.SendQuestionAndReceiveResponse(_greetMessage);
				
				switch(_response.ToLowerInvariant())
				{
					case "y":
						_respondedCorrectly = true;
						gameInited = true;
						Console.Clear();
						PlayerGameLoop();
						break;
					case "n":
						_respondedCorrectly = true;
						Environment.Exit(0);
						break;
					case "" or " ":
						goto case "y";
					case "clear":
						Console.Clear();
						gameInited = false;
						continue;
					case "view":
						ConsoleHelper.SendMessage(waitForGameToBeginPrompt);
						continue;
					default:
						gameInited = false;
						Console.Clear();
						continue;
				}
			} while (!_respondedCorrectly);
		}

		private void WonGame()
		{
			gameInited = false;

			InitValues();
			InitGame(GameState.WonGame);
		}
		private void EndGame()
		{
			gameInited = false;
			StartGame();
		}

		private void PlayerGameLoop()
		{
			if (!GiveFirstTwoCards())
			{
				return;
			}

			while(true)
			{
				if (!GameLogic.AskForUserToGetNewCard())
				{
					break;
				}

				if (!deck.GetTopCard(out var _card))
				{
					EndGame();
					return;
				}

				PlayerLogic.Instance.GiveCard(_card);

				PlayerLogic.Instance.PromptCurrentCards();

				byte _currentCardValue = PlayerLogic.Instance.GetValue();

				ConsoleHelper.SendMessage($"Sum of all cards is: {_currentCardValue}");

				if (!CheckForBust(_currentCardValue))
				{
					if(CheckForTwentyOne(_currentCardValue))
					{
						WonGame();
						return;
					}
					continue;
				}
				EndGame();
				return;
			}
			DealerGameLoop();
		}
		private void DealerGameLoop()
		{
			while (DealerLogic.Instance.GetValue() < DealerLogic.minCardsToStopPickup)
			{
				if(!deck.GetTopCard(out var _card))
				{
					ShowDealerAndPlayerCards();
					WonGame(); // Can't be bothered to add new end state rn, so player wins by default
					return;
				}

				DealerLogic.Instance.GiveCard(_card);

				var _currentDealerCardValueSubLoop = DealerLogic.Instance.GetValue();

				if(!CheckForBust(_currentDealerCardValueSubLoop))
				{
					if(CheckForTwentyOne(_currentDealerCardValueSubLoop))
					{
						ShowDealerAndPlayerCards();
						EndGame();
						return;
					}
					continue;
				}
				ShowDealerAndPlayerCards();
				WonGame();
				return;
			}
			Console.Clear();

			do
			{
				if (!deck.GetTopCard(out var _card))
				{
					WonGame(); // Can't be bothered to add new end state rn, so player wins by default
					return;
				}

				DealerLogic.Instance.GiveCard(_card);

				byte _currentDealerCardValue2 = DealerLogic.Instance.GetValue();

				if (!CheckForBust(_currentDealerCardValue2))
				{
					if (CheckForTwentyOne(_currentDealerCardValue2)
					    || _currentDealerCardValue2 > PlayerLogic.Instance.GetValue())
					{
						ShowDealerAndPlayerCards();
						EndGame();
						return;
					}
					continue;
				}
				ShowDealerAndPlayerCards();
				WonGame(); // Player Won
				return;
			} while (DealerLogic.Instance.GetValue() <= PlayerLogic.Instance.GetValue());

			Console.Clear();

			ShowDealerAndPlayerCards();

			byte _currentDealerCardValue3 = DealerLogic.Instance.GetValue();
			if(!CheckForBust(_currentDealerCardValue3))
			{
				if(CheckForTwentyOne(_currentDealerCardValue3)
				   || _currentDealerCardValue3 > PlayerLogic.Instance.GetValue())
				{
					EndGame();
					return;
				}
				WonGame(); // Player Won
				return; 
			}
			WonGame();
		}

		private static void ShowDealerAndPlayerCards()
		{
			PlayerLogic.Instance.PromptCurrentCards();

			byte _currentPlayerCardValue = PlayerLogic.Instance.GetValue();

			ConsoleHelper.SendMessage($"Sum of all your cards is: {_currentPlayerCardValue}");

			DealerLogic.Instance.PromptCurrentCards();

			byte _currentDealerCardValue = DealerLogic.Instance.GetValue();

			ConsoleHelper.SendMessage($"Sum of all dealers cards is: {_currentDealerCardValue}");
		}

		private bool GiveFirstTwoCards()
		{
			for (int i = 0; i < startingCards; i++)
			{
				if(!deck.GetTopCard(out var _card))
				{
					EndGame();
					return false;
				}
				PlayerLogic.Instance.GiveCard(_card);
			}
			PlayerLogic.Instance.PromptCurrentCards();

			var _currentCardValue = PlayerLogic.Instance.GetValue();

			ConsoleHelper.SendMessage($"Sum of all cards is: {_currentCardValue}");

			if(!CheckForBust(_currentCardValue))
			{
				if (!CheckForTwentyOne(_currentCardValue))
				{
					return true;
				}
				WonGame();
				return false;
			}
			EndGame();
			return false;
		}

		private static bool CheckForTwentyOne(byte _value)
		{
			return _value == 21;
		}
		private static bool CheckForBust(byte _value)
		{
			return _value > 21;
		}
	}
}
