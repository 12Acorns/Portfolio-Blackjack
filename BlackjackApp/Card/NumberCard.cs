using System.Runtime.InteropServices;

namespace BlackjackApp.Card
{
	[StructLayout(LayoutKind.Auto, Size = sizeof(byte) + sizeof(CardType))]
	internal readonly struct NumberCard(byte _value, CardType _type = CardType.Other)
	{
		public const byte MaxValue = 10; // Don't need to account for size

		private static byte SetValue(byte _value)
		{
			var _setValue = _value > MaxValue
				? MaxValue
				: _value;
			return _setValue;
		}
		public byte GetValue() => value;
		public CardType GetCardType() => _type;

		private readonly byte value = SetValue(_value);

		#region Conversions
		public static implicit operator NumberCard(byte _value)
		{
			return new NumberCard(_value);
		}
		public static implicit operator int(NumberCard _card)
		{
			return _card.value;
		}
		#endregion
		#region Operators
		#region Arithmatic Operations
		public static NumberCard operator +(NumberCard _this, NumberCard _other)
		{
			var _newValue = (byte)(_this.value + _other.value);
			var _newCard = new NumberCard(_newValue);

			return _newCard;
		}
		public static NumberCard operator -(NumberCard _this, NumberCard _other)
		{
			var _workingValue = _this.value - _other.value;

			var _newValue = (byte)(_workingValue < 0 ? 0 : _workingValue);

			var _newCard = new NumberCard(_newValue);

			return _newCard;
		}
		public static NumberCard operator *(NumberCard _this, NumberCard _other)
		{
			var _workingValue = _this.value * _other.value;

			var _newValue = (byte)(_workingValue > byte.MaxValue ? 255 : _workingValue);

			var _newCard = new NumberCard(_newValue);

			return _newCard;
		}
		public static NumberCard operator /(NumberCard _this, NumberCard _other)
		{
			var _newValue = (byte)(_this.value / _other.value);

			var _newCard = new NumberCard(_newValue);

			return _newCard;
		}
		#endregion
		#region Comparitive Operators
		public static bool operator >(NumberCard _this, NumberCard _other)
		{
			return _this.value > _other.value;
		}
		public static bool operator <(NumberCard _this, NumberCard _other)
		{
			return _this.value < _other.value;
		}
		public static bool operator >=(NumberCard _this, NumberCard _other)
		{
			return _this.value >= _other.value;
		}
		public static bool operator <=(NumberCard _this, NumberCard _other)
		{
			return _this.value <= _other.value;
		}
		#endregion
		#endregion
	}
}
