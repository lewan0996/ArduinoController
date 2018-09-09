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

            var toUpdate = _proceduresRepository.Get(id);

            if (toUpdate == null)
            {
                throw new RecordNotFoundException();
            }

            toUpdate.Name = newProcedure.Name;
            toUpdate.Device = newProcedure.Device == null
                ? null
                : _devicesRepository.Get(newProcedure.Device.Id);

            foreach (var command in toUpdate.Commands.ToArray())
            {
                _commandsRepository.Delete(command);
            }

            foreach (var command in newProcedure.Commands?.ToArray() ?? new Command[] { })
            {
                toUpdate.Commands.Add(command);
            }
        }
    }
}
