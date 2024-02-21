using System.Security.Cryptography;
using ConsoleUtility;

namespace BlackjackApp.Card
{
	internal sealed class CardDeck
	{
		private const byte deckSize = 52;
		private const byte cardsPerIndividualCardType = 13;

		private readonly Stack<NumberCard> cardStack;

		/// <summary>
		/// Initializes a card deck of 52 in a random order.
		/// </summary>
		public CardDeck()
		{
			cardStack = [];

			var _values = new NumberCard[52];

			for (byte i = 1; i <= deckSize; i++)
			{
				var _value = (byte)(i % cardsPerIndividualCardType);

				CardType _cardType;
				switch (_value)
				{
					case 11:
						_cardType = CardType.Jack;
						_value = 10;
						break;
					case 12:
						_cardType = CardType.King;
						_value = 10;
						break;
					case 0:
						_cardType = CardType.Queen;
						_value = 10;
						break;
					default:
						_cardType = CardType.Other;
						break;
				}

				var _card = new NumberCard(_value, _cardType);

				_values[i - 1] = _card;
			}
			RandomNumberGenerator.Shuffle<NumberCard>(_values);

			for (int i = 0; i < _values.Length; i++)
			{
				cardStack.Push(_values[i]);
			}
		}

		public bool GetTopCard(out NumberCard _card)
		{
			return cardStack.TryPop(out _card);
		}
	}
}
