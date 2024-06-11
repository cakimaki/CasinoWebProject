using CasinoWebProject.DTOs;
using CasinoWebProject.ViewModels;
using System;

namespace CasinoWebProject.Services
{
    public class FlipACoinGame : IGame
    {
        private static readonly Random random = new Random();

        public GameResultViewModel Play(decimal betAmount)
        {
            // 20% win, 80% lose
            bool isWin = random.NextDouble() < 0.20;

            decimal winAmount = isWin ? betAmount * 2 : 0; // Win amount is double the bet amount if win

            return new GameResultViewModel
            {
                IsWin = isWin,
                Message = isWin ? $"Congratulations! You won ${winAmount}!" : "Sorry, you lost. Try again!",
                Amount = winAmount
            };
        }
    }
}

