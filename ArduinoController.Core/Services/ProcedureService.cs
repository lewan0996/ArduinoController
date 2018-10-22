using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArduinoController.Core.Contract.DataAccess;
using ArduinoController.Core.Contract.Services;
using ArduinoController.Core.Exceptions;
using ArduinoController.Core.Models;
using ArduinoController.Core.Models.Commands;
using Microsoft.Azure.Devices;

namespace ArduinoController.Core.Services
{
    public class ProcedureService : IProcedureService
    {
        private readonly IRepository<Command> _commandsRepository;
        private readonly IRepository<ArduinoDevice> _devicesRepository;
        private readonly IRepository<Procedure> _proceduresRepository;

        private readonly ServiceClient _serviceClient;

        public ProcedureService(IRepository<Command> commandsRepository, IRepository<ArduinoDevice> devicesRepository,
            IRepository<Procedure> proceduresRepository, string iotHubConnectionString)
        {
            _commandsRepository = commandsRepository;
            _devicesRepository = devicesRepository;
            _proceduresRepository = proceduresRepository;

            _serviceClient = ServiceClient.CreateFromConnectionString(iotHubConnectionString);
        }

        public void Add(Procedure procedure)
        {
            if (procedure == null)
            {
                throw new ArgumentNullException();
            }

            // ReSharper disable once InvertIf
            if (procedure.Device != null)
            {
                var device = _devicesRepository.Get(procedure.Device.Id);
                procedure.Device = device ?? throw new RecordNotFoundException("There is no such Arduino device");
            }

            _proceduresRepository.Add(procedure);
        }

        public void Delete(int id)
        {
            var toDelete = _proceduresRepository.Get(id);

            if (toDelete == null)
            {
                throw new RecordNotFoundException();
            }

            _proceduresRepository.Delete(toDelete);
        }

        public void Update(int id, Procedure newProcedure)
        {
            if (newProcedure == null)
            {
                throw new ArgumentNullException(nameof(newProcedure));
            }

            var toUpdate = _proceduresRepository.GetAll(p => p.Commands)
                .FirstOrDefault(p => p.Id == id);

            if (toUpdate == null)
            {
                throw new RecordNotFoundException();
            }

            toUpdate.Name = newProcedure.Name;
            toUpdate.Device = newProcedure.Device == null
                ? null
                : _devicesRepository.Get(newProcedure.Device.Id);

            foreach (var command in toUpdate.Commands?.ToArray() ?? new Command[] { })
            {
                _commandsRepository.Delete(command);
            }

            if (toUpdate.Commands == null)
            {
                toUpdate.Commands = new List<Command>();
            }

            foreach (var command in newProcedure.Commands?.ToArray() ?? new Command[] { })
            {
                toUpdate.Commands.Add(command);
            }
        }

        public IQueryable<Procedure> GetUserProcedures(string userId)
        {
            return _proceduresRepository.GetAll(p => p.Device, p => p.Commands)
                .Where(p => p.UserId == userId);
        }

        public IQueryable<Procedure> GetAllProcedures()
        {
            return _proceduresRepository.GetAll(p => p.Device, p => p.Commands);
        }

        public async Task ExecuteAsync(Procedure procedure)
        {
            var method = new CloudToDeviceMethod("ExecuteProcedure")
            {
                ResponseTimeout =
                    TimeSpan.FromMilliseconds(procedure.Commands.OfType<WaitCommand>().Sum(c => c.Duration) + 30000)
            };

            method.SetPayloadJson(procedure.GenerateDeviceMethodPayload());

            var response = await _serviceClient.InvokeDeviceMethodAsync(procedure.Device.MacAddress, method);

            if (response.Status >= 400)
            {
                throw new CloudToDeviceMethodInvocationFailedException(
                    $"Method invocation failed. Response payload: {response.GetPayloadAsJson()}");
            }
        }
    }
}
