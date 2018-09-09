using System;
using System.Linq;
using ArduinoController.Core.Contract.DataAccess;
using ArduinoController.Core.Contract.Services;
using ArduinoController.Core.Exceptions;
using ArduinoController.Core.Models;

namespace ArduinoController.Core.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IRepository<ArduinoDevice> _deviceRepository;
        
        public DeviceService(IRepository<ArduinoDevice> deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }
        public void Add(ArduinoDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _deviceRepository.Add(device);
        }

        public void Delete(int id)
        {
            if (id==0)
            {
                throw new ArgumentException("Id cannot be 0", nameof(id));
            }

            var toDelete = _deviceRepository.Get(id);

            if (toDelete == null)
            {
                throw new RecordNotFoundException("There is no such device");
            }

            _deviceRepository.Delete(toDelete);
        }

        public void Update(int id, ArduinoDevice newDevice)
        {
            if (id == 0)
            {
                throw new ArgumentException("Id cannot be 0", nameof(id));
            }

            if (newDevice == null)
            {
                throw new ArgumentNullException(nameof(newDevice));
            }

            var toUpdate = _deviceRepository.Get(id);

            if (toUpdate == null)
            {
                throw new RecordNotFoundException("There is no such device");
            }

            toUpdate.MacAddress = newDevice.MacAddress;
            toUpdate.Name = newDevice.Name;
        }

        public IQueryable<ArduinoDevice> GetAllUserDevices(string userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return _deviceRepository.GetAll()
                .Where(d => d.UserId == userId);
        }
    }
}
