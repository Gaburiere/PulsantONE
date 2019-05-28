using System;
using System.Threading.Tasks;

namespace Formazione2019.PulsantONE.Services
{
    public interface IRemoteService
    {
        Task<Tuple<string, string>> MoveSpaceShip();
    }
}