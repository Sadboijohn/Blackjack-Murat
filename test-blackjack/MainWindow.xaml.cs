using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Blackjack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Initialize variables
        private int balance = 100;
        private int bet = 0;
        private List<string> playerHand = new List<string>();
        private List<string> dealerHand = new List<string>();
        private Random random = new Random();
        private string[] deck =
        {
            "Ace of Clubs", "2 of Clubs", "3 of Clubs", "4 of Clubs", "5 of Clubs", "6 of Clubs", "7 of Clubs", "8 of Clubs", "9 of Clubs", "10 of Clubs", "Jack of Clubs", "Queen of Clubs", "King of Clubs",
            "Ace of Spades", "2 of Spades", "3 of Spades", "4 of Spades", "5 of Spades", "6 of Spades", "7 of Spades", "8 of Spades", "9 of Spades", "10 of Spades", "Jack of Spades", "Queen of Spades", "King of Spades",
            "Ace of Hearts", "2 of Hearts", "3 of Hearts", "4 of Hearts", "5 of Hearts", "6 of Hearts", "7 of Hearts", "8 of Hearts", "9 of Hearts", "10 of Hearts", "Jack of Hearts", "Queen of Hearts", "King of Hearts",
            "Ace of Diamonds", "2 of Diamonds", "3 of Diamonds", "4 of Diamonds", "5 of Diamonds", "6 of Diamonds", "7 of Diamonds", "8 of Diamonds", "9 of Diamonds", "10 of Diamonds", "Jack of Diamonds", "Queen of Diamonds", "King of Diamonds"
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Start a new game when the window loads
            NewGame();

        }

        private void NewGame()
        {
            // Reset variables
            playerHand.Clear();
            dealerHand.Clear();
      

            // Set the bet
            bet = (int)betSlider.Value;
            balance -= bet;
            balanceLabel.Content = "Balance: " + balance;

            // Deal the cards
            playerHand.Add(DealCard());
            playerHand.Add(DealCard());
            dealerHand.Add(DealCard());
            dealerHand.Add(DealCard());

            // Update the display
            UpdateHandDisplay(playerHand, playerHandList);
            UpdateHandDisplay(dealerHand, dealerHandList);
        }

        private void UpdateHandDisplay(List<string> hand, ItemsControl display)
        {
            display.Items.Clear();
            foreach (string card in hand)
            {
                string imageFilePath = "Cards/" + card.Replace(" ", "") + ".png";
                display.Items.Add(imageFilePath);
            }
        }



        private string DealCard()
        {
            // Check if the deck is empty
            if (deck.Length == 0)
            {
                // Create a new deck
                deck = new string[]
                {
            "Ace of Clubs", "2 of Clubs", "3 of Clubs", "4 of Clubs", "5 of Clubs", "6 of Clubs", "7 of Clubs", "8 of Clubs", "9 of Clubs", "10 of Clubs", "Jack of Clubs", "Queen of Clubs", "King of Clubs",
            "Ace of Spades", "2 of Spades", "3 of Spades", "4 of Spades", "5 of Spades", "6 of Spades", "7 of Spades", "8 of Spades", "9 of Spades", "10 of Spades", "Jack of Spades", "Queen of Spades", "King of Spades",
            "Ace of Hearts", "2 of Hearts", "3 of Hearts", "4 of Hearts", "5 of Hearts", "6 of Hearts", "7 of Hearts", "8 of Hearts", "9 of Hearts", "10 of Hearts", "Jack of Hearts", "Queen of Hearts", "King of Hearts",
            "Ace of Diamonds", "2 of Diamonds", "3 of Diamonds", "4 of Diamonds", "5 of Diamonds", "6 of Diamonds", "7 of Diamonds", "8 of Diamonds", "9 of Diamonds", "10 of Diamonds", "Jack of Diamonds", "Queen of Diamonds", "King of Diamonds"
                };

                // Shuffle the deck
                for (int i = 0; i < deck.Length; i++)
                {
                    int j = random.Next(deck.Length);
                    string temp = deck[i];
                    deck[i] = deck[j];
                    deck[j] = temp;
                }
            }

            // Choose a random card from the deck
            int index = random.Next(deck.Length);
            string card = deck[index];

            // Remove the card from the deck
            string[] newDeck = new string[deck.Length - 1];
            int newIndex = 0;
            for (int i = 0; i < deck.Length; i++)
            {
                if (i != index)
                {
                    newDeck[newIndex] = deck[i];
                    newIndex++;
                }
            }
            deck = newDeck;

            return card;
        }

        private void hitButton_Click(object sender, RoutedEventArgs e)
        {
            // Deal another card to the player
            playerHand.Add(DealCard());
            UpdateHandDisplay(playerHand, playerHandList);

            // Check if the player has bust
            if (GetHandTotal(playerHand) > 21)
            {
                MessageBox.Show("You bust!");
                NewGame();
            }
        }

        private int CalculateHandValue(List<string> hand)
        {
            return CalculateHandValue(hand, playerCardCountLabel);
        }

        private int CalculateHandValue(List<string> hand, Label playerCardCountLabel)
        {
            int value = 0;
            int numAces = 0;
          
            foreach (string card in hand)

            {
                if (card.StartsWith("Ace"))
                {
                    value += 11;
                    numAces++;
                    playerCardCountLabel.Content = "Player's Value: " + CalculateHandValue(playerHand);
                }
                else if (card.StartsWith("King") || card.StartsWith("Queen") || card.StartsWith("Jack"))
                {
                    value += 10;
                    playerCardCountLabel.Content = "Player's Value: " + CalculateHandValue(playerHand);
                }
                else
                {
                    value += int.Parse(card.Substring(0, card.IndexOf(" ")));
                    playerCardCountLabel.Content = "Player's Value: " + CalculateHandValue(playerHand);
                }
            }

            while (value > 21 && numAces > 0)
            {
                value -= 10;
                numAces--;
                playerCardCountLabel.Content = "Player's Value: " + CalculateHandValue(playerHand);
            }

            return value;
            


        }


        private void standButton_Click(object sender, RoutedEventArgs e)
        {
            // Reveal the dealer's second card
            
            UpdateHandDisplay(dealerHand, dealerHandList);

            // Dealer hits until they have at least 17
            while (GetHandTotal(dealerHand) < 17)
            {
                dealerHand.Add(DealCard());
                UpdateHandDisplay(dealerHand, dealerHandList);
            }

            // Determine the winner
            int playerTotal = GetHandTotal(playerHand);
            int dealerTotal = GetHandTotal(dealerHand);
            if (playerTotal > 21)
            {
                MessageBox.Show("You bust!");
            }
            else if (dealerTotal > 21)
            {
                MessageBox.Show("Dealer busts! You win.");
                balance += 2 * bet;
            }
            else if (playerTotal > dealerTotal)
            {
                MessageBox.Show("You win!");
                balance += 2 * bet;
            }
            else if (dealerTotal > playerTotal)
            {
                MessageBox.Show("Dealer wins.");
            }
            else
            {
                MessageBox.Show("Push.");
                balance += bet;
            }
            balanceLabel.Content = "Balance: " + balance;
            NewGame();
        }

        private int GetHandTotal(List<string> hand)
        {
            // Calculate the total value of a hand
            int total = 0;
            int aces = 0;
            foreach (string card in hand)
            {
                if (card.StartsWith("Ace"))
                {
                    total += 11;
                    aces++;
                }
                else if (card.StartsWith("Jack") || card.StartsWith("Queen") || card.StartsWith("King"))
                {
                    total += 10;
                }
                else
                {
                    total += int.Parse(card.Substring(0, 1));
                }
            }

            // Handle aces as 1s if the total is over 21
            while (total > 21 && aces > 0)
            {
                total -= 10;
                aces--;
            }

            return total;
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
        }

        private void betTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

    }
}

