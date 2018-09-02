using System;
using System.Linq;
using ArduinoController.Core.Contract.DataAccess;
using ArduinoController.Core.Contract.Services;
using ArduinoController.Core.Exceptions;
using ArduinoController.Core.Models;
using ArduinoController.Core.Models.Commands;

namespace ArduinoController.Core.Services
{
    public class ProcedureService : IProcedureService
    {
        private readonly IRepository<Command> _commandsRepository;
        private readonly IRepository<ArduinoDevice> _devicesRepository;
        private readonly IRepository<Procedure> _proceduresRepository;

        public ProcedureService(IRepository<Command> commandsRepository, IRepository<ArduinoDevice> devicesRepository,
            IRepository<Procedure> proceduresRepository)
        {
            _commandsRepository = commandsRepository;
            _devicesRepository = devicesRepository;
            _proceduresRepository = proceduresRepository;
        }

        public Procedure Add(Procedure procedure)
        {
            if (procedure == null)
            {
                throw new ArgumentNullException();
            }

            return _proceduresRepository.Add(procedure);
        }

        public void Delete(string userId, string name)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var toDelete = _proceduresRepository.Get(new { userId, name });

            if (toDelete == null)
            {
                throw new RecordNotFoundException();
            }

            _proceduresRepository.Delete(toDelete);
        }

        public Procedure Update(Procedure newProcedure)
        {
            if (newProcedure == null)
            {
                throw new ArgumentNullException(nameof(newProcedure));
            }

            var toUpdate = _proceduresRepository.Get(new { newProcedure.UserId, newProcedure.Name });

            toUpdate.Name = newProcedure.Name;
            toUpdate.Device = newProcedure.Device?.MacAddress == null
                ? null
                : _devicesRepository.Get(newProcedure.Device.MacAddress);

            foreach (var command in toUpdate.Commands.ToArray())
            {
                _commandsRepository.Delete(command);
            }

            foreach (var command in newProcedure.Commands?.ToArray() ?? new Command[] { })
            {
                toUpdate.Commands.Add(command);
            }

            return toUpdate;
        }
    }
}
