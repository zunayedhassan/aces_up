using System;
using System.Collections.Generic;

namespace AcesUp_CoreLibrary
{
    public class PlayingCards
    {
        // Generates playing cards
        public static List<Card> GetSortedPlayingCards()
        {
            List<Card> cards = new List<Card>();

            for (int i = 1; i <= 52; i++)
            {
                if ((i >= 1) & (i <= 13))
                    cards.Add(new Card(Suit.Spades, (i + 1)));
                else if ((i >= 14) & (i <= 26))
                    cards.Add(new Card(Suit.Hearts, (i - 12)));
                else if ((i >= 27) & (i <= 39))
                    cards.Add(new Card(Suit.Diamonds, (i - 25)));
                else
                    cards.Add(new Card(Suit.Clubs, (i - 38)));
            }

            return cards;
        }

        // Shuffles cards from 200 to 500 randomly
        public static List<Card> GetShuffledCards(List<Card> cards)
        {
            Random random = new Random();

            for (int swapTime = 1; swapTime <= new Random().Next(200, 500); swapTime++)
            {
                int firstSwapableCardIndex = random.Next(0, cards.Count),
                    secondSwapableCardIndex = random.Next(0, cards.Count);

                // Swaping
                Card tempCard = cards[firstSwapableCardIndex];
                cards[firstSwapableCardIndex] = cards[secondSwapableCardIndex];
                cards[secondSwapableCardIndex] = tempCard;
            }

            return cards;
        }
    }
}
