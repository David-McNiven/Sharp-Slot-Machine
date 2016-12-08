using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Sharp Slot Machine
/// Original Framework provided by Tom Siliopoulos
/// Modifed by David McNiven
/// Student # 200330143
/// Created on December 8th, 2016
/// A basic 3 reel slot machine simulator 
/// Music is property of Square Enix
/// </summary>
namespace SlotMachine
{
    public partial class SlotMachineForm : Form
    {
        private int playerMoney = 1000;
        private int winnings = 0;
        private int jackpot = 5000;
        private int playerBet = 10;
        private int grapes = 0;
        private int bananas = 0;
        private int oranges = 0;
        private int cherries = 0;
        private int bars = 0;
        private int bells = 0;
        private int sevens = 0;
        private int blanks = 0;
        private Stream bgm = Properties.Resources.Theme;
        private SoundPlayer player;

        private Random random = new Random();

        public SlotMachineForm()
        {
            InitializeComponent();
        }

        /* Utility function to show Player Stats */
        private void showPlayerStats()
        {
            CreditLabel.Text = playerMoney.ToString();
            WinningsLabel.Text = winnings.ToString();
            JackpotLabel.Text = jackpot.ToString();
            CurrentBetLabel.Text = playerBet.ToString();
            checkMoney();
        }

        /* Utility function to reset all fruit tallies*/
        private void resetFruitTally()
        {
            grapes = 0;
            bananas = 0;
            oranges = 0;
            cherries = 0;
            bars = 0;
            bells = 0;
            sevens = 0;
            blanks = 0;
        }

        /* Utility function to reset the player stats */
        private void resetAll()
        {
            playerMoney = 1000;
            playerBet = 10;
            winnings = 0;
            jackpot = 5000;
            resetFruitTally();
            checkMoney();
            showPlayerStats();
            ReelPictureBox1.Image = (Image)Properties.Resources.ResourceManager.GetObject("Reel1");
            ReelPictureBox2.Image = (Image)Properties.Resources.ResourceManager.GetObject("Reel2");
            ReelPictureBox3.Image = (Image)Properties.Resources.ResourceManager.GetObject("Reel3");
        }

        /* Check to see if the player won the jackpot */
        private void checkJackPot()
        {
            /* compare two random values */
            var jackPotTry = this.random.Next(51) + 1;
            var jackPotWin = this.random.Next(51) + 1;
            if (jackPotTry == jackPotWin)
            {
                MessageBox.Show("You Won the $" + jackpot + " Jackpot!!", "Jackpot!!");
                playerMoney += jackpot;
                winnings += jackpot;
                jackpot = 1000;
            }
        }

        /* When this function is called it determines the betLine results. */
        private void Reels()
        {
            string betLine = "";
            int outcome = 0;

            for (var spin = 1; spin <= 3; spin++)
            {
                outcome = this.random.Next(65) + 1;

                if (outcome <= 27)
                {  // 41.5% probability
                    betLine = "Blank";
                    blanks++;
                }
                else if (outcome <= 37)
                { // 15.4% probability
                    betLine = "Grapes";
                    grapes++;
                }
                else if (outcome <= 46)
                { // 13.8% probability
                    betLine = "Banana";
                    bananas++;
                }
                else if (outcome <= 54)
                { // 12.3% probability
                    betLine = "Orange";
                    oranges++;
                }
                else if (outcome <= 59)
                { //  7.7% probability
                    betLine = "Cherry";
                    cherries++;
                }
                else if (outcome <= 62)
                { //  4.6% probability
                    betLine = "Bar";
                    bars++;
                }
                else if (outcome <= 64)
                { //  3.1% probability
                    betLine = "Bell";
                    bells++;
                }
                else
                { //  1.5% probability
                    betLine = "Seven";
                    sevens++;
                }
               ((PictureBox)Controls.Find(String.Concat("ReelPictureBox", spin), true)[0])
                    .Image = (Image)Properties.Resources.ResourceManager.GetObject(betLine);
            }
        }

        /* This function calculates the player's winnings, if any */
        private void determineWinnings()
        {
            if (blanks == 0)
            {
                if (sevens == 3)
                {
                    winnings = playerBet * 100;
                }
                else if (bells == 3)
                {
                    winnings = playerBet * 75;
                }
                else if (bars == 3)
                {
                    winnings = playerBet * 50;
                }
                else if (cherries == 3)
                {
                    winnings = playerBet * 40;
                }
                else if (oranges == 3)
                {
                    winnings = playerBet * 30;
                }
                else if (bananas == 3 || sevens == 2)
                {
                    winnings = playerBet * 20;
                }
                else if (grapes == 3 || bells == 2)
                {
                    winnings = playerBet * 10;
                }
                else if (sevens == 1 || bars == 2)
                {
                    winnings = playerBet * 5;
                }
                else if (cherries == 2)
                {
                    winnings = playerBet * 4;
                }
                else if (oranges == 2)
                {
                    winnings = playerBet * 3;
                }
                else if (grapes == 2 || bananas == 2)
                {
                    winnings = playerBet * 2;
                }
                else
                {
                    winnings = playerBet * 1;
                }
                playerMoney += winnings;
            }
            else
            {
                winnings = 0;
                playerMoney -= playerBet;
            }
            resetFruitTally();
        }

        /// <summary>
        /// event handler for making a spin, 10%(rounded down) of each bet is added to the jackpot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpinPictureBox_Click(object sender, EventArgs e)
        {
            jackpot += (int)Math.Floor((double)playerBet / 10);
            Reels();
            determineWinnings();
            if(playerBet >= 25)
            {
                checkJackPot();
            }
            showPlayerStats();
        }

        /// <summary>
        /// event handler for simulating a button being pressed on MouseDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox sentPicture = (PictureBox)sender;

            sentPicture.Image = (Image)Properties.Resources.ResourceManager
                    .GetObject(sentPicture.Name.Replace("PictureBox", "") + "_pressed");
        }

        /// <summary>
        /// event handler for simulating a button being depressed on MouseUp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            PictureBox sentPicture = (PictureBox)sender;

            if (sentPicture.Enabled)
            {
                sentPicture.Image = (Image)Properties.Resources.ResourceManager
                    .GetObject(sentPicture.Name.Replace("PictureBox", ""));
            }
        }

        /// <summary>
        /// event handler for reseting the slot machine back to default values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetPictureBox_Click(object sender, EventArgs e)
        {
            resetAll();
        }

        /// <summary>
        /// event handler for exiting the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitPictureBox_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// resets everything to defaut values on form load and starts background music
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SlotMachineForm_Load(object sender, EventArgs e)
        {
            resetAll();
            player = new SoundPlayer(bgm);
            player.PlayLooping();
        }

        /// <summary>
        /// event handler for all 8 bet buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BetPictureBox_Click(object sender, EventArgs e)
        {
            playerBet = Int32.Parse(((PictureBox)sender).Name.Substring(13));
            CurrentBetLabel.Text = playerBet.ToString();
            checkMoney();
        }

        /// <summary>
        /// compares players remaining credit against their current bet and enables or disables the spin button accordingly
        /// </summary>
        private void checkMoney()
        {
            if (playerMoney == 0 || playerBet > playerMoney)
            {
                SpinPictureBox.Image = (Image)Properties.Resources.ResourceManager.GetObject("Spin_disabled");
                SpinPictureBox.Enabled = false;
            }
            else
            {
                SpinPictureBox.Enabled = true;
                SpinPictureBox.Image = (Image)Properties.Resources.ResourceManager.GetObject("Spin");
            }
        }

    }
}
