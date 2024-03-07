using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
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

namespace Mutex_ATM_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MyContext _context;
        private decimal selectedAmount;
        private Mutex transferMutex = new Mutex();
        private const int TransferCooldownSeconds = 5;
        private DateTime lastTransferTime = DateTime.MinValue;
        public MainWindow()
        {
            InitializeComponent();
            _context = new MyContext();
        }


        private void InsertCard_Click(object sender, RoutedEventArgs e)
        {
            long cardNumberr;
            if (!long.TryParse(cardNumber.Text, out cardNumberr))
            {
                MessageBox.Show("Please enter a valid card number.");
                return;
            }

            Account user = _context.Accounts.FirstOrDefault(a => a.CardNumber == cardNumberr);

            if (user != null)
            {
                Fullname.Content = user.Fullname;
                balance.Content = user.Balance;
            }
            else
            {
                Fullname.Content = "User not found!";
            }

            //List<Account> accounts = new List<Account>
            //{
            //    new Account { Fullname = "John Doe", CardNumber = 416973883399, Balance = 10000 },
            //    new Account { Fullname = "Jane Doe", CardNumber = 416973884488, Balance = 2500 },
            //    new Account { Fullname = "Alice Smith", CardNumber = 416973882277, Balance = 5500 },
            //    new Account { Fullname = "Bob Johnson", CardNumber = 416973881122, Balance = 6000 },
            //    new Account { Fullname = "Eve Wilson", CardNumber = 416973885533, Balance = 3000 }
            //};

            //_context.Accounts.AddRange(accounts);
            //_context.SaveChanges();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            long cardNumbers;
            if (!long.TryParse(cardNumber.Text, out cardNumbers))
            {
                MessageBox.Show("Please enter a valid card number.", "Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Account user = _context.Accounts.FirstOrDefault(a => a.CardNumber == cardNumbers);

            if (user != null)
            {
                MessageBox.Show($"User: {user.Fullname}\nBalance: {user.Balance}", "User Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("User not found!", "Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btn20_Click(object sender, RoutedEventArgs e)
        {
            selectedAmount = 20;
        }

        private void btn50_Click(object sender, RoutedEventArgs e)
        {
            selectedAmount = 50;
        }

        private void btn100_Click(object sender, RoutedEventArgs e)
        {
            selectedAmount = 100;
        }

        private void btn200_Click(object sender, RoutedEventArgs e)
        {
            selectedAmount = 200;
        }

        private void btn300_Click(object sender, RoutedEventArgs e)
        {
            selectedAmount = 300;
        }

        private void TransferMoney_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(cardNumber.Text))
            {
                MessageBox.Show("Please enter a card number", "Transfer Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(moneytxt.Text))
            {
                MessageBox.Show("Please enter an amount to transfer", "Transfer Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            decimal transferAmount;
            if (!decimal.TryParse(moneytxt.Text, out transferAmount))
            {
                MessageBox.Show("Please enter a valid amount to transfer", "Transfer Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (transferAmount <= 0)
            {
                MessageBox.Show("Please select an amount greater than zero to transfer", "Transfer Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!CanTransfer())
            {
                MessageBox.Show($"Please wait {TransferCooldownSeconds} seconds before initiating another transfer", "Transfer Cooldown", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!transferMutex.WaitOne(TimeSpan.FromSeconds(1)))
            {
                MessageBox.Show("Transfer operation is currently in progress. Please try again later", "Transfer Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                lastTransferTime = DateTime.Now;

                MessageBox.Show($"Transferred amount: {transferAmount}", "Transfer Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            finally
            {
                transferMutex.ReleaseMutex();
            }
        }

        private bool CanTransfer()
        {
            return (DateTime.Now - lastTransferTime).TotalSeconds >= TransferCooldownSeconds;
        }   
    }
}
