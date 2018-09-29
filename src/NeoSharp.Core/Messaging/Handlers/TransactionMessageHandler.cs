﻿using System;
using System.Threading.Tasks;
using NeoSharp.Core.Blockchain.Processing;
using NeoSharp.Core.Blockchain.Repositories;
using NeoSharp.Core.Logging;
using NeoSharp.Core.Messaging.Messages;
using NeoSharp.Core.Models;
using NeoSharp.Core.Models.OperationManger;
using NeoSharp.Core.Network;

namespace NeoSharp.Core.Messaging.Handlers
{
    public class TransactionMessageHandler : MessageHandler<TransactionMessage>
    {
        #region Private Fields 
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionPool _transactionPool;
        private readonly ITransactionOperationsManager _transactionOperationsManager;
        private readonly ILogger<TransactionMessageHandler> _logger;
        #endregion

        #region Constructor 
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="transactionRepository">Transaction respository access.</param>
        /// <param name="transactionPool">Transaction Pool</param>
        /// <param name="transactionOperationsManager">The transaction operation manager</param>
        /// <param name="logger">Logger</param>
        public TransactionMessageHandler(
            ITransactionRepository transactionRepository, 
            ITransactionPool transactionPool, 
            ITransactionOperationsManager transactionOperationsManager,
            ILogger<TransactionMessageHandler> logger)
        {
            _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
            _transactionPool = transactionPool ?? throw new ArgumentNullException(nameof(transactionPool));
            _transactionOperationsManager = transactionOperationsManager ?? throw new ArgumentNullException(nameof(transactionOperationsManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        #endregion

        #region MessageHandler override methods
        /// <inheritdoc />
        public override bool CanHandle(Message message)
        {
            return message is TransactionMessage;
        }

        /// <inheritdoc />
        public override async Task Handle(TransactionMessage message, IPeer sender)
        {
            var transaction = message.Payload;

            if (transaction is MinerTransaction) return;

            // TODO #373: check if the hash of the transaction is known already
            if (transaction.Hash == null)
            {
                this._transactionOperationsManager.Sign(transaction);
            }

            if (await this._transactionRepository.ContainsTransaction(transaction.Hash))
            {
                _logger.LogInformation($"The transaction \"{transaction.Hash?.ToString(true)}\" exists already on the blockchain.");
                return;
            }

            // Transaction is not added right away but queued to be verified and added.
            // It is the reason why we do not broadcast immediately.

            // TODO #374: It is a bit more complicated

            _transactionPool.Add(transaction);
            _logger.LogInformation($"Transaction with Hash {transaction.Hash?.ToString(true)} added to the TransactionPool.");
        }
        #endregion
    }
}