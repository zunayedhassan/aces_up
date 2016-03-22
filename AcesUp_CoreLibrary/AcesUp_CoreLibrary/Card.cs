namespace AcesUp_CoreLibrary
{
    public struct Card
    {
        public Suit CardSuit;   // Spades, Hearts, Diamonds, Clubs
        public int Rank;        // 2, 3, 4, 5, ....., 10, J, Q, K, A

        public Card(Suit cardSuit, int rank)
        {
            this.CardSuit = cardSuit;
            this.Rank = rank;
        }

        // GetRankSigneture() method is only required for ToString() method
        private static string GetRankSigneture(int rank)
        {
            string rankSigneture = string.Empty;

            if (rank > 10)
            {
                switch (rank)
                {
                    case 11:
                        rankSigneture = "J";
                        break;

                    case 12:
                        rankSigneture = "Q";
                        break;

                    case 13:
                        rankSigneture = "K";
                        break;

                    case 14:
                        rankSigneture = "A";
                        break;
                }
            }
            else
                rankSigneture = rank.ToString();

            if (rankSigneture.Length == 1)
                rankSigneture = (" " + rankSigneture);

            return rankSigneture;
        }

        // Returns string of card type. Example: '♠ K' or '♥10'
        public override string ToString()
        {
            string printedCard = string.Empty;

            switch (CardSuit)
            {
                case Suit.Spades:
                    printedCard += ("♠" + Card.GetRankSigneture(this.Rank));
                    break;

                case Suit.Hearts:
                    printedCard += ("♥" + Card.GetRankSigneture(this.Rank));
                    break;

                case Suit.Diamonds:
                    printedCard += ("♦" + Card.GetRankSigneture(this.Rank));
                    break;

                case Suit.Clubs:
                    printedCard += ("♣" + Card.GetRankSigneture(this.Rank));
                    break;
            }

            return printedCard;
        }
    }
}
