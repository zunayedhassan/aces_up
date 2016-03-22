using System.Collections.Generic;

namespace AcesUp_CoreLibrary
{
    /// <summary>
    /// AcesUp class contains core elements for  maintaining  the  game.
    /// This class is responsible for suffling, dealing, playing, moving
    /// cards and it also determines wether game over or not etc.
    /// </summary>
    public class AcesUp
    {
        public List<Card>[] StackOfPiles = new List<Card>[4];      // There will be 4 piles and each of the pile holds cards after dealing.
        public List<Card> Deck;     // Deck holds suffled cards. On each dealing (13 times highest) it gives 4 cards to StackOfPiles.
        public int Score;
        public bool Won;

        public AcesUp()
        {
            Deck = PlayingCards.GetShuffledCards(PlayingCards.GetSortedPlayingCards());

            // Initializing StackOfPiles array. Each of pile will contain list of cards.
            for (int pileIndex = 0; pileIndex < StackOfPiles.Length; pileIndex++)
                StackOfPiles[pileIndex] = new List<Card>();
        }

        // Deals card. It sets 4 suffled cards on StackOfPiles.
        public void Deal()
        {
            if (Deck.Count != 0)
            {
                for (int index = 0; index < StackOfPiles.Length; index++)
                {
                    // On each time remove card from Deck and add that card on Pile.
                    StackOfPiles[index].Add(Deck[Deck.Count - 1]);
                    Deck.Remove(Deck[Deck.Count - 1]);
                }
            }
        }

        // When gamer/user found comparativly small card, this method helps user to remove that card from pile.
        public void RemoveCardFromPile(int location)
        {
            int pileIndex = location - 1;

            // When pileIndex is within the range of StackOfPiles AND selected pile contains at least one card
            // AND that last card of the pile is really valid to remove then go on.
            if ((pileIndex < StackOfPiles.Length) & (StackOfPiles[pileIndex].Count != 0) & IsValidToRemoveCardFromPile(pileIndex))
            {
                StackOfPiles[pileIndex].RemoveAt(StackOfPiles[pileIndex].Count - 1);            // Remove card from that pile
                Score += 500;                                                                   // Add Score, 500 on each time
            }
        }

        // User might need to move card from one pile to another, but only under valid condition
        public void MoveCard(int from, int to)
        {
            // When, one of the pile must have at least one card AND the other must not have any.
            if ((StackOfPiles[from - 1].Count > 0) & (StackOfPiles[to - 1].Count == 0))
            {
                StackOfPiles[to - 1].Add(StackOfPiles[from - 1][StackOfPiles[from - 1].Count - 1]);         // Removing card from pile
                StackOfPiles[from - 1].RemoveAt(StackOfPiles[from - 1].Count - 1);                          // Adding card to an empty pile
            }
        }

        // IsValidToRemoveCardFromPile() method takes location of the card and  it  checks  that,  is  there  any
        // card contains in that suit or not, but not in same pile obviously. If our selected card has lower than
        // the highest ranked card, then it only returns true.
        private bool IsValidToRemoveCardFromPile(int pileIndex)
        {
            int[] activeCardsStatistics = ActiveCardsStatistics();          // Get the list of suit from piles

            if (NoMoreMoves())      // If, each of piles conatins different of suits, the our selected card is not eligeble to remove from pile.
                return false;

            int maxCardRank = int.MinValue;

            // When there is more than one card in same suit, then find the maximum ranked card.
            if (activeCardsStatistics[(int)StackOfPiles[pileIndex][StackOfPiles[pileIndex].Count - 1].CardSuit] > 1)
            {
                foreach (var piles in StackOfPiles)
                    // If our selected card has the same suit as current card
                    if (piles[piles.Count - 1].CardSuit == StackOfPiles[pileIndex][StackOfPiles[pileIndex].Count - 1].CardSuit)
                        // Then find the maximum ranked card within that suit (as same suit as our selected card)
                        if (maxCardRank < piles[piles.Count - 1].Rank)
                            maxCardRank = piles[piles.Count - 1].Rank;

                // If our selected card has not the highest rank, then return true.
                if (StackOfPiles[pileIndex][StackOfPiles[pileIndex].Count - 1].Rank < maxCardRank)
                    return true;
            }

            // Return false otherwise.
            return false;
        }

        // When all active card has different suit then return true, as no more moves left now.
        private bool NoMoreMoves()
        {
            int[] activeCardsStatistics = ActiveCardsStatistics();

            foreach (var data in activeCardsStatistics)
                if (data > 1)
                    return false;

            return true;
        }

        // When the game is won (4 Aces on each pile as in active card state) or the deck is empty
        // AND no more moves left then the game is over.
        public bool IsGameOver()
        {
            if (Won)
                return true;
            else if ((Deck.Count == 0) & NoMoreMoves())
                return true;
            else
                return false;
        }

        // Get how many type of active suits on each pile. Consider that, on each pile only the last card is in active state.
        private int[] ActiveCardsStatistics()
        {
            Card[] activeCards = new Card[StackOfPiles.Length];
            int[] activeCardsStatistics = new int[StackOfPiles.Length];

            // Initializing activeCardsStatistics variable
            for (int i = 0; i < activeCardsStatistics.Length; i++)
                activeCardsStatistics[i] = new int();

            // Getting number of card on each suit on activeCardsStatistics
            for (int index = 0; index < StackOfPiles.Length; index++)
            {
                // If current pile contains at least one card
                if (StackOfPiles[index].Count != 0)
                {
                    // Get active card, this will be required later
                    activeCards[index] = StackOfPiles[index][StackOfPiles[index].Count - 1];

                    switch (activeCards[index].CardSuit)
                    {
                        case Suit.Spades:
                            activeCardsStatistics[(int)Suit.Spades] += 1;
                            break;

                        case Suit.Hearts:
                            activeCardsStatistics[(int)Suit.Hearts] += 1;
                            break;

                        case Suit.Diamonds:
                            activeCardsStatistics[(int)Suit.Diamonds] += 1;
                            break;

                        case Suit.Clubs:
                            activeCardsStatistics[(int)Suit.Clubs] += 1;
                            break;
                    }
                }

                // This will determines the game is won or not. If all 4 different aces on all active card then this game is already won.
                bool completeAces = false;

                foreach (var card in activeCards)
                {
                    if (card.Rank == 14)            // Aces has rank 14, the highest rank of each suit.
                        completeAces = true;
                    else
                    {
                        completeAces = false;       // All 4 different active card must has aces. Or otherwise assign false immediately.
                        break;
                    }
                }

                // 4 different active card has aces then assign true into Won variable.
                if (completeAces)
                    Won = true;
            }

            return activeCardsStatistics;
        }

        // ToString() method will return a string of the current state of StackOfPiles.
        //
        // Example:
        // ───────────────────────────────
        // PILE 1   PILE 2  PILE 3  PILE 4
        // ───────────────────────────────
        // ♥ A      ♦ 9     ♣10     ♠ 3
        //          ♣ K     ♦ 5     ♥ J
        //          ♦10     ♠ 6
        //                  ♣ 7
        // ───────────────────────────────
        public override string ToString()
        {
            string printedPilesOfCards = "──────────────────────────────\n" +
                                         "PILE 1\tPILE 2\tPILE 3\tPILE 4\n" +
                                         "──────────────────────────────\n";

            int maxNumberOfStackedCards = int.MinValue;

            // Determine which pile has maxmum number of card and how much it holds.
            foreach (var stackOfPile in StackOfPiles)
                if (maxNumberOfStackedCards < stackOfPile.Count)
                    maxNumberOfStackedCards = stackOfPile.Count;

            for (int i = 0; i < maxNumberOfStackedCards; i++)
            {
                int pileNumber = 0;          // This variable holds pile number. If it exceeds 4 then it will be assigned 0 again.

                for (int pileOfCardsIndex = 0; pileOfCardsIndex < StackOfPiles.Length; pileOfCardsIndex++)
                {
                    if (i < StackOfPiles[pileOfCardsIndex].Count)
                        printedPilesOfCards += (StackOfPiles[pileOfCardsIndex][i] + "\t");
                    else
                        printedPilesOfCards += "    \t";

                    pileNumber++;

                    // When pile no. 4 visited then add a new line and assign pile number 0 again.
                    if (pileNumber == StackOfPiles.Length)
                    {
                        printedPilesOfCards += "\n";
                        pileNumber = 0;
                    }
                }
            }

            printedPilesOfCards += "──────────────────────────────";

            return printedPilesOfCards;
        }
    }
}
