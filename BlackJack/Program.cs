using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlackJack
{
    class Program
    {
        static void Main(string[] args)
        {
            var rnd = new Random();

            //Initializing Game
            var dealingCards = new CardDealing(5000);
            int dealerTotal = dealingCards.dealerTotal;
            int userTotal = dealingCards.userTotal;
            decimal userMoney = dealingCards.userMoney;
            bool gameIsActive = dealingCards.gameIsActive;
            Dictionary<string, int> cardDeck = dealingCards.makeDeck();
            Dictionary<string, int> usedCardDeck = dealingCards.usedCardDeck;
            Dictionary<string, int> dealerCards = dealingCards.dealerCards;
            Dictionary<string, int> myCards = dealingCards.myCards;


            decimal betAmount = dealingCards.askBet(userMoney);


            //Dealer's Card
            dealingCards.executeCards(rnd, 1, "dealer");


            string answer = "p";

            do
            {
                //Game Play
                dealingCards.executeCards(rnd, 2, "player");
                userTotal = dealingCards.userTotal;
                dealingCards.displayScore();
                userMoney = dealingCards.userMoney;
                dealingCards.userPlay(rnd, dealingCards, betAmount, userMoney);
                gameIsActive = dealingCards.gameIsActive;
                if (gameIsActive)
                {
                    dealingCards.dealerPlay(rnd, dealingCards, betAmount, userMoney);
                }
                userMoney = dealingCards.userMoney;
                Console.WriteLine("Press any key to deal again or hit q to quit");
                answer = Console.ReadLine();

                if (answer == "q")
                {
                    Console.WriteLine("Final Score - {0:C}", userMoney);

                }
                else
                {
                    dealingCards = new CardDealing(userMoney);
                    betAmount = dealingCards.askBet(userMoney);
                    dealingCards.dealerTotal = 0;
                    dealingCards.userTotal = 0;
                    dealingCards.usedCardDeck = new Dictionary<string, int>();
                    dealingCards.dealerCards = new Dictionary<string, int>();
                    dealingCards.myCards = new Dictionary<string, int>();
                    cardDeck = dealingCards.makeDeck();
                    dealingCards.executeCards(rnd, 1, "dealer");
                }

            } while (answer != "q");

        }
    }

    class CardDealing
    {
        public decimal userMoney;
        public decimal betAmount = 0;
        public int dealerTotal;
        public int userTotal;
        public int selectedChoice = 1;
        public int cardsDealt = 0;
        public bool gameIsActive = true;
        public bool doubledDown = false;
        public int numberHits = 0;
        public Dictionary<string, int> usedCardDeck = new Dictionary<string, int>();

        public Dictionary<string, int> cardDeck = new Dictionary<string, int>();

        public Dictionary<string, int> dealerCards = new Dictionary<string, int>();

        public Dictionary<string, int> myCards = new Dictionary<string, int>();

        public CardDealing(decimal userMoney)
        {
            this.userMoney = userMoney;
        }

        public Dictionary<string, int> makeDeck()
        {
            cardDeck.Add("As", 11);
            cardDeck.Add("Ac", 11);
            cardDeck.Add("Ah", 11);
            cardDeck.Add("Ad", 11);
            cardDeck.Add("2s", 2);
            cardDeck.Add("2c", 2);
            cardDeck.Add("2h", 2);
            cardDeck.Add("2d", 2);
            cardDeck.Add("3s", 3);
            cardDeck.Add("3c", 3);
            cardDeck.Add("3h", 3);
            cardDeck.Add("3d", 3);
            cardDeck.Add("4s", 4);
            cardDeck.Add("4c", 4);
            cardDeck.Add("4h", 4);
            cardDeck.Add("4d", 4);
            cardDeck.Add("5s", 5);
            cardDeck.Add("5c", 5);
            cardDeck.Add("5h", 5);
            cardDeck.Add("5d", 5);
            cardDeck.Add("6s", 6);
            cardDeck.Add("6c", 6);
            cardDeck.Add("6h", 6);
            cardDeck.Add("6d", 6);
            cardDeck.Add("7s", 7);
            cardDeck.Add("7c", 7);
            cardDeck.Add("7h", 7);
            cardDeck.Add("7d", 7);
            cardDeck.Add("8s", 8);
            cardDeck.Add("8c", 8);
            cardDeck.Add("8h", 8);
            cardDeck.Add("8d", 8);
            cardDeck.Add("9s", 9);
            cardDeck.Add("9c", 9);
            cardDeck.Add("9h", 9);
            cardDeck.Add("9d", 9);
            cardDeck.Add("10s", 10);
            cardDeck.Add("10c", 10);
            cardDeck.Add("10h", 10);
            cardDeck.Add("10d", 10);
            cardDeck.Add("Js", 10);
            cardDeck.Add("Jc", 10);
            cardDeck.Add("Jh", 10);
            cardDeck.Add("Jd", 10);
            cardDeck.Add("Qs", 10);
            cardDeck.Add("Qc", 10);
            cardDeck.Add("Qh", 10);
            cardDeck.Add("Qd", 10);
            cardDeck.Add("Ks", 10);
            cardDeck.Add("Kc", 10);
            cardDeck.Add("Kh", 10);
            cardDeck.Add("Kd", 10);
            return cardDeck;
        }

        public void executeCards(Random rnd, int cardsDealing, string dealerOrPlayer)
        {
            this.cardsDealt += cardsDealing;
            do
            {
                var random = rnd.Next(0, this.cardDeck.Count);
                var cardCount = 0;

                foreach (var card in this.cardDeck)
                {
                    if (cardCount == random && !this.usedCardDeck.ContainsKey(card.Key))
                    {
                        //Console.WriteLine(card.Value);
                        this.usedCardDeck.Add(card.Key, card.Value);
                        if (dealerOrPlayer == "dealer")
                        {
                            this.dealerCards.Add(card.Key, card.Value);
                            this.dealerTotal += card.Value;
                            handlingAces("dealer");
                        }
                        else if (dealerOrPlayer == "player")
                        {
                            this.myCards.Add(card.Key, card.Value);
                            this.userTotal += card.Value;
                            handlingAces("player");
                        }

                    }
                    else if (cardCount == random && this.usedCardDeck.ContainsKey(card.Key))
                    {
                        continue;
                    }
                    cardCount++;
                }
            } while (this.usedCardDeck.Count < this.cardsDealt /*&& userTotal <= 21*/);



        }

        public void displayScore()
        {
            Console.WriteLine("\nDealers Cards:");
            foreach (var card in this.dealerCards)
            {
                Console.WriteLine("{0}", card.Key);
            }

            Console.WriteLine("\nYour Cards:");
            foreach (var card in this.myCards)
            {
                Console.WriteLine("{0}", card.Key);
            }

            Console.WriteLine("\nDealers Total: {0} - Your Total {1} \n", this.dealerTotal, this.userTotal);
        }

        public void userPlay(Random rnd, CardDealing dealingCards, decimal bet, decimal money)
        {

            while (this.selectedChoice == 1 && this.userTotal < 21)
            {
                if (this.userMoney >= 2m * bet && this.numberHits == 0)
                {
                    Console.WriteLine("Would you like to 1. Hit | 2. Stay | 3. Double Down");
                }
                else
                {
                    Console.WriteLine("Would you like to 1. Hit | 2. Stay ");
                }

                bool isChoice = int.TryParse(Console.ReadLine(), out this.selectedChoice);
                if (!isChoice)
                {
                    Console.WriteLine("Invalid Entry");
                }
                if (this.selectedChoice == 1)
                {
                    this.executeCards(rnd, 1, "player");
                    this.userTotal = dealingCards.userTotal;
                    dealingCards.displayScore();
                    this.numberHits++;
                }
                if (this.selectedChoice == 3)
                {
                    this.doubledDown = true;
                    this.executeCards(rnd, 1, "player");
                    this.userTotal = dealingCards.userTotal;
                    dealingCards.displayScore();
                    this.numberHits++;
                }
            }



            if (this.userTotal == 21)
            {
                if (this.numberHits == 0)
                {
                    Console.WriteLine("Black Jack!");
                    this.gameIsActive = false;
                    money += 1.5m * bet;
                    this.userMoney = money;
                    Console.WriteLine("Money Count: {0:C}", money);
                }
            }
            if (this.userTotal > 21)
            {
                Console.WriteLine("You Busted!");
                this.gameIsActive = false;
                if (!doubledDown)
                {
                    money -= bet;
                }
                else
                {
                    money -= 2m * bet;
                }

                this.userMoney = money;
                Console.WriteLine("Money Count: {0:C}", money);
            }
        }

        public void dealerPlay(Random rnd, CardDealing dealingCards, decimal bet, decimal money)
        {
            if (this.userTotal <= 21)
            {
                do
                {
                    Thread.Sleep(1000);
                    dealingCards.executeCards(rnd, 1, "dealer");
                    dealerTotal = dealingCards.dealerTotal;
                    dealingCards.displayScore();
                } while (dealerTotal < 17);

                if (dealerTotal > 21 || dealerTotal < userTotal)
                {
                    Console.WriteLine("You Win!");
                    if (!this.doubledDown)
                    {
                        money += bet;
                    }
                    else
                    {
                        money += 2m * bet;
                    }


                    this.userMoney = money;
                    Console.WriteLine("\nMoney Count: {0:C}", money);
                }
                else if (dealerTotal == userTotal)
                {
                    Console.WriteLine("Push!");
                    Console.WriteLine("\nMoney Count: {0:C}", money);
                    this.userMoney = money;
                }
                else
                {
                    Console.WriteLine("You Lose!");
                    if (!this.doubledDown)
                    {
                        money -= bet;
                    }
                    else
                    {
                        money -= 2m * bet;
                    }
                    this.userMoney = money;
                    Console.WriteLine("\nMoney Count: {0:C}", money);
                }
            }
        }

        public void handlingAces(string dealerOrPlayer)
        {
            if (dealerOrPlayer == "dealer")
            {
                if (this.dealerCards.ContainsKey("As") && this.dealerCards.ContainsValue(11) && dealerTotal > 21)
                {
                    this.dealerCards.Remove("As");
                    this.dealerCards.Add("As", 1);
                    this.dealerTotal -= 10;
                }
                else if (this.dealerCards.ContainsKey("Ac") && this.dealerCards.ContainsValue(11) && dealerTotal > 21)
                {
                    this.dealerCards.Remove("Ac");
                    this.dealerCards.Add("Ac", 1);
                    this.dealerTotal -= 10;
                }
                else if (this.dealerCards.ContainsKey("Ad") && this.dealerCards.ContainsValue(11) && dealerTotal > 21)
                {
                    this.dealerCards.Remove("Ad");
                    this.dealerCards.Add("Ad", 1);
                    this.dealerTotal -= 10;
                }
                else if (this.dealerCards.ContainsKey("Ah") && this.dealerCards.ContainsValue(11) && dealerTotal > 21)
                {
                    this.dealerCards.Remove("Ah");
                    this.dealerCards.Add("Ah", 1);
                    this.dealerTotal -= 10;

                }
            }
            else if (dealerOrPlayer == "player")
            {
                if (this.myCards.ContainsKey("As") && this.myCards.ContainsValue(11) && userTotal > 21)
                {
                    this.myCards.Remove("As");
                    this.myCards.Add("As", 1);
                    this.userTotal -= 10;
                }
                else if (this.myCards.ContainsKey("Ac") && this.myCards.ContainsValue(11) && userTotal > 21)
                {
                    this.myCards.Remove("Ac");
                    this.myCards.Add("Ac", 1);
                    this.userTotal -= 10;
                }
                else if (this.myCards.ContainsKey("Ad") && this.myCards.ContainsValue(11) && userTotal > 21)
                {
                    this.myCards.Remove("Ad");
                    this.myCards.Add("Ad", 1);
                    this.userTotal -= 10;
                }
                else if (this.myCards.ContainsKey("Ah") && this.myCards.ContainsValue(11) && userTotal > 21)
                {
                    this.myCards.Remove("Ah");
                    this.myCards.Add("Ah", 1);
                    this.userTotal -= 10;

                }
            }

        }

        public decimal askBet(decimal money)
        {

            Console.WriteLine("Money Count: {0:C}", money);
            Console.WriteLine("How much would you like to bet?");
            bool isProperBet = decimal.TryParse(Console.ReadLine(), out this.betAmount);

            do
            {
                if (this.betAmount > money)
                {
                    Console.WriteLine("Insufficient funds, bet another amount");
                    isProperBet = decimal.TryParse(Console.ReadLine(), out this.betAmount);

                    return this.betAmount;
                }
                else
                {
                    return this.betAmount;
                }

            } while (this.betAmount > money);


        }

    }

}
