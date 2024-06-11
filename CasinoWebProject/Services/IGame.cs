using CasinoWebProject.DTOs;
using CasinoWebProject.ViewModels;

namespace CasinoWebProject.Services
{
    public interface IGame
    {
        GameResultViewModel Play(decimal betAmount);
    }
}
