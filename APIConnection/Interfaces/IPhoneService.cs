using APIConnection.Models;

namespace APIConnection.Interfaces
{
    public interface IPhoneService
    {
        Task<IEnumerable<Phone>> GetPhones();
    }
}
