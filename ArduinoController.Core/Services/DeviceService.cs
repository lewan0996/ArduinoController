using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ArduinoController.Core.Contract.DataAccess;
using ArduinoController.Core.Contract.Services;
using ArduinoController.Core.Exceptions;
using ArduinoController.Core.Models;
using Microsoft.Azure.Devices;

namespace ArduinoController.Core.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IRepository<ArduinoDevice> _deviceRepository;
        private readonly RegistryManager _registryManager;

        public DeviceService(IRepository<ArduinoDevice> deviceRepository, string ioTHubConnectionString)
        {
            _deviceRepository = deviceRepository;
            _registryManager = RegistryManager.CreateFromConnectionString(ioTHubConnectionString);
        }
        public async Task AddAsync(ArduinoDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _deviceRepository.Add(device);
            await RegisterDeviceToIoTHubAsync(device);
        }

        public void Delete(int id)
        {
            if (id == 0)
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

        public async Task UpdateAsync(int id, ArduinoDevice newDevice)
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

            await _registryManager.RemoveDeviceAsync(toUpdate.MacAddress);

            toUpdate.MacAddress = newDevice.MacAddress;
            toUpdate.Name = newDevice.Name;

            await RegisterDeviceToIoTHubAsync(toUpdate);
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

        private async Task RegisterDeviceToIoTHubAsync(ArduinoDevice device)
        {
            var iotHubDevice = new Device(device.MacAddress)
            {
                Authentication = new AuthenticationMechanism
                {
                    SymmetricKey = new SymmetricKey
                    {
                        PrimaryKey = GetSha1Hash(device.MacAddress).ToUpper(),
                        SecondaryKey = GetMD5Hash(device.MacAddress).ToUpper()
                    }
                }
            };

            await _registryManager.AddDeviceAsync(iotHubDevice);
        }

        private static string GetSha1Hash(string value)
        {
            var stringBuilder = new StringBuilder();

            using (var hash = SHA1.Create())
            {
                var enc = Encoding.UTF8;
                var result = hash.ComputeHash(enc.GetBytes(value));

                foreach (var b in result)
                    stringBuilder.Append(b.ToString("x2"));
            }

            return stringBuilder.ToString();
        }

        private static string GetMD5Hash(string value)
        {
            var stringBuilder = new StringBuilder();

            using (var hash = MD5.Create())
            {
                var enc = Encoding.UTF8;
                var result = hash.ComputeHash(enc.GetBytes(value));

                foreach (var b in result)
                    stringBuilder.Append(b.ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
}
