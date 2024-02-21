using BlackjackApp.Card;
using ConsoleUtility;
using System.Text;

namespace BlackjackApp.Game_Logic
{
	internal sealed class DealerLogic
	{
		public const int minCardsToStopPickup = 17;

		private static readonly object lockObject = new();

		private static DealerLogic? instance = null;

		public static DealerLogic Instance
		{
			get
			{
				lock(lockObject)
				{
					instance ??= new DealerLogic();
					return instance;
				}
			}
		}

		private const string displayMessage =
			"Dealers cards are:";

		private StringBuilder displayText = new(displayMessage);

		private List<(NumberCard _card, bool _currentlyAppendedToDisplay)>
			cards = new(8);

		private bool firstAppendedToDisplay = true;

		private byte totalCardValues;


		public byte GetValue() => totalCardValues;

		public void GiveCard(NumberCard _card)
		{
			var _value = _card.GetValue();

			if(_value != 1)
			{
				totalCardValues += _value;
				cards.Add((_card, true));
				return;
			}

			var _valueIfCanBeAce = (byte)(11 + totalCardValues);
			if(_valueIfCanBeAce < 22)
			{
				totalCardValues = _valueIfCanBeAce;
				var _cardToAdd = new NumberCard(11, CardType.Ace);
				cards.Add((_cardToAdd, true));
				return;
			}
			totalCardValues++;
			cards.Add((_card, true));
		}

		public void PromptCurrentCards()
		{
			cards = cards
				.Where(x =>
					x._currentlyAppendedToDisplay)
				.Select(x =>
				{
					x._currentlyAppendedToDisplay = false;
					return x;
				})
				.ToList();

			cards.ForEach(x =>
			{
				string _displayCardType =
					x._card.GetCardType() != CardType.Other
						? x._card.GetCardType().ToString()
						: x._card.GetValue().ToString();

				string _appendText =
					firstAppendedToDisplay
						? $" {_displayCardType}"
						: $", {_displayCardType}";

				displayText = displayText.Append(_appendText);

				firstAppendedToDisplay = false;
			});

			ConsoleHelper.SendMessage(displayText.ToString());
		}

		public void Reset()
		{
			cards =
				new List<(NumberCard _card, bool _currentlyAppendedToDisplay)>
					(8);
			firstAppendedToDisplay = true;
			totalCardValues = 0;
			displayText = new StringBuilder(displayMessage);
		}
	}
}
