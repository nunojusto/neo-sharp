﻿using System;
using System.Threading.Tasks;
using NeoSharp.Core.Blockchain.Processing;
using NeoSharp.Core.Logging;
using NeoSharp.Core.Messaging.Messages;
using NeoSharp.Core.Models.OperationManger;
using NeoSharp.Core.Network;

namespace NeoSharp.Core.Messaging.Handlers
{
    public class BlockMessageHandler : MessageHandler<BlockMessage>
    {
        #region Private Fields 
        private readonly IBlockProcessor _blockProcessor;
        private readonly IBlockOperationsManager _blockOperationsManager;
        private readonly IBroadcaster _broadcaster;
        private readonly ILogger<BlockMessageHandler> _logger;
        #endregion

        #region Constructor 

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="blockProcessor">Block Pool</param>
        /// <param name="blockOperationsManager">The block operations mananger.</param>
        /// <param name="broadcaster">Broadcaster</param>
        /// <param name="logger">Logger</param>
        public BlockMessageHandler(
            IBlockProcessor blockProcessor,
            IBlockOperationsManager blockOperationsManager,
            IBroadcaster broadcaster,
            ILogger<BlockMessageHandler> logger)
        {
            this._blockProcessor = blockProcessor ?? throw new ArgumentNullException(nameof(blockProcessor));
            this._blockOperationsManager = blockOperationsManager ?? throw new ArgumentNullException(nameof(blockOperationsManager));
            this._broadcaster = broadcaster ?? throw new ArgumentNullException(nameof(broadcaster));
            this._logger = logger;
        }
        #endregion

        #region MessageHandler override methods 
        /// <inheritdoc />
        public override bool CanHandle(Message message)
        {
            return message is BlockMessage;
        }

        /// <inheritdoc />
        public override async Task Handle(BlockMessage message, IPeer sender)
        {
            var block = message.Payload;
            
            if (block.Hash == null)
            {
                this._blockOperationsManager.Sign(block);
            }

            if (this._blockOperationsManager.Verify(block))
            {
                await _blockProcessor.AddBlock(block);
            }

            this._logger.LogInformation($"Adding block {block.Hash} to the BlockPool with Index {block.Index}.");
            await this._blockProcessor.AddBlock(block);

            this._logger.LogInformation($"Broadcasting block {block.Hash} with Index {block.Index}.");
            this._broadcaster.Broadcast(message, sender);
        }
        #endregion
    }
}
