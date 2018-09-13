﻿using System.Collections.Generic;
using System.Threading.Tasks;
using NeoSharp.Core.Cryptography;
using NeoSharp.Core.Models;
using NeoSharp.Core.Types;

namespace NeoSharp.Core.Blockchain
{
    public interface IBlockchain
    {
        /// <summary>
        /// Memory pool
        /// </summary>
        //TransactionPool MemoryPool { get; }

        Task InitializeBlockchain();

        #region Blocks & BlockHeaders

        /// <summary>
        /// Current block
        /// </summary>
        Block CurrentBlock { get; }

        /// <summary>
        /// Last block header
        /// </summary>
        BlockHeader LastBlockHeader { get; }

        /// <summary>
        /// Return the corresponding block information according to the specified height
        /// </summary>
        /// <param name="height"></param>
        /// <returns></returns>
        Task<Block> GetBlock(uint height);

        /// <summary>
        /// Return the corresponding block information according to the specified height
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        Task<Block> GetBlock(UInt256 hash);

        /// <summary>
        /// Get blocks
        /// </summary>
        /// <param name="blockHashes">Block hashes</param>
        /// <returns>Block</returns>
        Task<IEnumerable<Block>> GetBlocks(IReadOnlyCollection<UInt256> blockHashes);

        /// <summary>
        /// Returns the hash of the corresponding block based on the specified height
        /// </summary>
        /// <param name="height"></param>
        /// <returns></returns>
        Task<UInt256> GetBlockHash(uint height);

        /// <summary>
        /// Return the corresponding block header information according to the specified height
        /// </summary>
        /// <param name="height"></param>
        /// <returns></returns>
        Task<BlockHeader> GetBlockHeader(uint height);

        /// <summary>
        /// Returns the corresponding block header information according to the specified hash value
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        Task<BlockHeader> GetBlockHeader(UInt256 hash);

        /// <summary>
        /// Returns the information for the next block based on the specified hash value
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        Task<Block> GetNextBlock(UInt256 hash);

        /// <summary>
        /// Returns the hash value of the next block based on the specified hash value
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        Task<UInt256> GetNextBlockHash(UInt256 hash);

        #endregion

        #region Transactions

        /// <summary>
        /// Determine whether the specified transaction is included in the blockchain
        /// </summary>
        /// <param name="hash">Transaction hash</param>
        /// <returns>Return true if the specified transaction is included</returns>
        Task<bool> ContainsTransaction(UInt256 hash);

        #endregion

        bool ContainsUnspent(CoinReference input);

        bool ContainsUnspent(UInt256 hash, ushort index);

        /// <summary>
        /// Return the corresponding asset information according to the specified hash
        /// </summary>
        /// <param name="hash">Hash</param>
        /// <returns></returns>
        Task<Asset> GetAsset(UInt256 hash);

        /// <summary>
        /// Return the corresponding contract information according to the specified hash
        /// </summary>
        /// <param name="hash">Hash</param>
        /// <returns></returns>
        Task<Contract> GetContract(UInt160 hash);

        /// <summary>
        /// Return all contracts
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Contract>> GetContracts();

        /// <summary>
        /// Return all assets
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Asset>> GetAssets();

        ECPoint[] GetValidators();
        IEnumerable<ECPoint> GetValidators(IEnumerable<Transaction> others);

        /// <summary>
        /// Returns the total amount of system costs contained in the corresponding block and all previous blocks based on the specified block height
        /// </summary>
        /// <param name="height"></param>
        /// <returns></returns>
        Task<long> GetSysFeeAmount(uint height);

        /// <summary>
        /// Returns the total amount of system charges contained in the corresponding block and all previous blocks based on the specified block hash value
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        long GetSysFeeAmount(UInt256 hash);

        /// <summary>
        /// Returns the corresponding transaction information according to the specified hash value
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        Task<Transaction> GetTransaction(UInt256 hash);

        // TODO [AboimPinto] Async methods cannot have out parameters. Method not used for now.
        ///// <summary>
        ///// Return the corresponding transaction information and the height of the block where the transaction is located according to the specified hash value
        ///// </summary>
        ///// <param name="hash"></param>
        ///// <param name="height"></param>
        ///// <returns></returns>
        //Task<Transaction> GetTransaction(UInt256 hash, out int height);

        Task<IEnumerable<Transaction>> GetTransactions(IReadOnlyCollection<UInt256> transactionHashes);

        /// <summary>
        /// Get the corresponding unspent assets based on the specified hash value and index
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        TransactionOutput GetUnspent(UInt256 hash, ushort index);

        IEnumerable<TransactionOutput> GetUnspent(UInt256 hash);

        /// <summary>
        /// Determine if the transaction is double
        /// </summary>
        /// <param name="tx"></param>
        /// <returns></returns>
        bool IsDoubleSpend(Transaction tx);
    }
}