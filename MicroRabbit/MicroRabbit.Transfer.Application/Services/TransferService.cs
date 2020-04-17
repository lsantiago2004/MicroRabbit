using MicroRabbit.Transfer.Application.Interfaces;
//using MicroRabbit.Transfer.Domain.Commands;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Domain.Models;
using MicroRabbit.Domain.Core.Bus;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Transfer.Application.Services
{
    public class TransferService : ITransferService
    {
        private readonly ITransferRepository _transferRepository;
        private readonly IEventBus _bus;

        public TransferService(ITransferRepository transferRepository, IEventBus bus)
        {
            _transferRepository = transferRepository;
            _bus = bus;
        }

        public IEnumerable<TransferLog> GetTransferLogs()
        {
            return _transferRepository.GetTransferLogs();
        }


        //We dont need this now
        //public void Transfer(AccountTransfer accountTransfer)
        //{
        //    var createTransferCommand = new CreateTransferCommand(
        //        accountTransfer.FromAccount,
        //        accountTransfer.ToAccount,
        //        accountTransfer.TransferAmount
        //     );

        //    //There is MY CreateTransferCommand and now we use the Bus to send the command to our TransferCommandHandler.
        //    _bus.SendCommand(createTransferCommand);

        //}
    }
}
