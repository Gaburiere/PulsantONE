using System;
using System.Threading.Tasks;

namespace Formazione2019.PulsantONE.Services
{
    public interface IRemoteService
    {
        Task<bool> SendMessage();
        Task<object> Negotiate();
        Task<bool> Register();
    }
}