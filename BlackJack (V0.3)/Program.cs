using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BlackJack__V0._3_
{
    public class Card
    {
        public string Value { get; }
        public string Suit { get; }

        public Card(string value, string suit)
        {
            Value = value;
            Suit = suit;
        }

        public string GetName()
        {
            return Value + " of " + Suit;
        }
    }  //step 2


    public class Deck
    {
        private List<Card> cards;

        public Deck()
        {
            cards = new List<Card>();
        }

        public void CreateDeck()
        {
            string[] values = { "Ace", "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King" };
            string[] suits = { "Clubs", "Diamonds", "Hearts", "Spades" };

            foreach (string suit in suits)
            {
                foreach (string value in values)
                {
                    cards.Add(new Card(value, suit));
                }
            }
        }

        public void Shuffle()
        {
            Random random = new Random();
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                Card card = cards[k];
                cards[k] = cards[n];
                cards[n] = card;
            }
        }

        public Card Deal()
        {
            Card card = cards[0];
            cards.RemoveAt(0);
            return card;
        }
    }  //step 3


    public class Player
    {
        public string Name { get; }
        public int Score { get; set; }
        public List<Card> Hand { get; }

        public Player(string name)
        {
            Name = name;
            Hand = new List<Card>();
        }

        public int GetCardValue(Card card)
        {
            int value = 0;
            int aceCount = 0;

            if (card.Value == "Ace")
            {
                value += 11;
                aceCount++;
            }
            else if (card.Value == "Jack" || card.Value == "Queen" || card.Value == "King")
            {
                value += 10;
            }
            else
            {
                value += int.Parse(card.Value);
            }

            while (value > 21 && aceCount > 0)
            {
                value -= 10;
                aceCount--;
            }
            this.Score += value;

            return value;
        }

        public string HandAsString()
        {
            string result = "";
            foreach (Card card in Hand)
            {
                result += card.Value;
                result += " ";
                result += card.Suit;
                result += ", ";
            }
            return result;
        }

        public void AddCard(Card card)
        {
            Hand.Add(card);
            this.GetCardValue(card);
        }
    }  //step 4


    interface IGameStrategy
    {
        bool WantCard(Player player);
    }

    class HumanStrategy : IGameStrategy
    {
        public bool WantCard(Player player)
        {
            Console.Write("Do you want another card? (Y/N): ");
            string answer = Console.ReadLine().ToLower();
            return (answer == "y");
        }
    }

    class Game
    {
        private Deck deck;
        private List<Player> players;
        private IGameStrategy gameStrategy;

        public Game(string playerName)
        {
            deck = new Deck();
            deck.CreateDeck();
            deck.Shuffle();
            players = new List<Player>();
            players.Add(new Player(playerName));
            players.Add(new Player("Dealer"));
            gameStrategy = new HumanStrategy();
        }

        public void ShowPlayer(Player player)
        {
            Console.WriteLine(player.Name + " was dealt a card.");
            Console.WriteLine(player.Name + "'s hand: " + player.HandAsString());
            Console.WriteLine(player.Name + "'s score: " + player.Score);
            Console.WriteLine();
        }

        public void Start()
        {
            Console.WriteLine("Welcome to Blackjack!");
            Console.WriteLine();

            while (true)
            {
                // Shuffle deck if less than 20% of cards remain
                //if (deck.PercentRemaining() < 20)
                //{
                //    Console.WriteLine("Shuffling deck...");
                //    deck.Shuffle();
                //}

                foreach (Player player in players)
                {
                    player.AddCard(deck.Deal());
                    player.AddCard(deck.Deal());
                    ShowPlayer(player);

                    while (true)
                    {
                        if (player != players[players.Count - 1])
                        {
                            if (gameStrategy.WantCard(player))
                            {
                                player.AddCard(deck.Deal());
                            }
                            else break;

                            if (player.Score > 21)
                            {
                                Console.WriteLine(player.Name + " busts! Game over.");
                                Console.WriteLine();
                                ShowPlayer(player);
                                return;
                            }
                        }

                        else
                        {
                            if (player.Score >= 17)
                            {
                                Console.WriteLine("Dealer stands.");
                                return;
                            }
                            else
                            {
                                player.AddCard(deck.Deal());
                                Console.WriteLine("Dealer was dealt a card.");
                                Console.WriteLine();
                            }

                            if (player.Score > 21)
                            {
                                Console.WriteLine("Dealer busts! You win!");
                                Console.WriteLine();
                                ShowPlayer(player);
                                return;
                            }
                        }
                        ShowPlayer(player);
                    }
                }


                int maxScore = players.Max(player => player.Score);

                if (maxScore == 21)
                {
                    Console.WriteLine("Blackjack! Game over.");
                    Console.WriteLine();
                    return;
                }
                else if (maxScore > 21)
                {
                    Console.WriteLine("Everyone busts! Game over.");
                    Console.WriteLine();
                    return;
                }
                else if (maxScore== players[players.Count - 1].Score)
                {
                    Console.WriteLine("Dealer won! Game over.");
                    Console.WriteLine();
                    return;
                }
                else { Console.WriteLine("irina"); }
            }
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game("Player");
            game.Start();
        }
    }
}