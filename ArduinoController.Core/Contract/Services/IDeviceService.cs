using System.Linq;
using System.Threading.Tasks;
using ArduinoController.Core.Models;

namespace ArduinoController.Core.Contract.Services
{
    public interface IDeviceService
    {
        /// <exception cref="System.ArgumentNullException">Thrown when given device is null</exception>
        void Add(ArduinoDevice device);
        /// <exception cref="System.ArgumentException">Thrown when id=0 is given</exception>
        /// <exception cref="Exceptions.RecordNotFoundException">Thrown when no record with given id exists</exception>
        void Delete(int id);
        /// <exception cref="System.ArgumentException">Thrown when id=0 is given</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when given device is null</exception>
        /// <exception cref="Exceptions.RecordNotFoundException">Thrown when no record with given id exists</exception>
        void Update(int id, ArduinoDevice newDevice);
        /// <exception cref="System.ArgumentNullException">Thrown when given  userId is null</exception>
        IQueryable<ArduinoDevice> GetAllUserDevices(string userId);

        Task RegisterDeviceToIoTHub(ArduinoDevice device);
    }
}
