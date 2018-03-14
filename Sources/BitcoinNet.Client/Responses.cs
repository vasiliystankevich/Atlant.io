using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BitcoinNet.Client
{
    public class BitcoinNetResponse<T>
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("result")]
        public T Result { get; set; }
    }

    public class ValidateAddressResponse
    {
        [JsonProperty("isvalid")]
        public bool IsValid { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("scriptPubKey")]
        public string ScriptPubKey { get; set; }

        [JsonProperty("ismine")]
        public bool IsMine { get; set; }

        [JsonProperty("iswatchonly")]
        public bool IsWatchOnly { get; set; }

        [JsonProperty("isscript")]
        public bool IsScript { get; set; }

        [JsonProperty("pubkey")]
        public string PubKey { get; set; }

        [JsonProperty("iscompressed")]
        public bool IsCompressed { get; set; }

        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("timestamp")]
        public long TimeStamp { get; set; }

        [JsonProperty("hdkeypath")]
        public string HdKeyPath { get; set; }

        [JsonProperty("hdmasterkeyid")]
        public string HdMasterKeyId { get; set; }
    }

    public class ListReceivedByAddressResponseElement
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("amount")]
        public float Amount { get; set; }

        [JsonProperty("confirmations")]
        public int Confirmations { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("txids")]
        public List<string> TxIds { get; set; }
    }

    public class ListReceivedByAccountResponseElement
    {
        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("confirmations")]
        public int Confirmations { get; set; }
    }

    public class GetTransactionResponseDetailElement
    {
        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("vout")]
        public int Vout { get; set; }
    }

    public class GetTransactionResponse
    {
        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("confirmations")]
        public int Confirmations { get; set; }

        [JsonProperty("generated")]
        public bool Generated { get; set; }

        [JsonProperty("blockhash")]
        public string BlockHash { get; set; }

        [JsonProperty("blockindex")]
        public int BlockIndex { get; set; }

        [JsonProperty("blocktime")]
        public long BlockTime { get; set; }

        [JsonProperty("txid")]
        public string TxId { get; set; }

        [JsonProperty("walletconflicts")]
        public List<string> WalletConflicts { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("timereceived")]
        public long TimeReceived { get; set; }

        [JsonProperty("bip125-replaceable")]
        public string Bip125Replaceable { get; set; }

        [JsonProperty("details")]
        public List<GetTransactionResponseDetailElement> Details { get; set; }

        [JsonProperty("hex")]
        public string Hex { get; set; }
    }

    public class ListTransactionResponseElement
    {
        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("vout")]
        public int Vout { get; set; }

        [JsonProperty("confirmations")]
        public int Confirmations { get; set; }

        [JsonProperty("generated")]
        public bool Generated { get; set; }

        [JsonProperty("blockhash")]
        public string BlockHash { get; set; }

        [JsonProperty("blockindex")]
        public long BlockIndex { get; set; }

        [JsonProperty("blocktime")]
        public long BlockTime { get; set; }

        [JsonProperty("txid")]
        public string TxId { get; set; }

        [JsonProperty("walletconflicts")]
        public List<string> WalletConflicts { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("timereceived")]
        public long TimeReceived { get; set; }

        [JsonProperty("bip125-replaceable")]
        public string Bip125Replaceable { get; set; }
    }

    public class GetInfoResponse
    {
        [JsonProperty("deprecation-warning")]
        public string DeprecationWarning { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("protocolversion")]
        public int ProtocolVersion { get; set; }

        [JsonProperty("walletversion")]
        public int WalletVersion { get; set; }

        [JsonProperty("balance")]
        public double Balance { get; set; }

        [JsonProperty("blocks")]
        public int Blocks { get; set; }

        [JsonProperty("timeoffset")]
        public int TimeOffset { get; set; }

        [JsonProperty("connections")]
        public int Connections { get; set; }

        [JsonProperty("proxy")]
        public string Proxy { get; set; }

        [JsonProperty("difficulty")]
        public double Difficulty { get; set; }

        [JsonProperty("testnet")]
        public bool TestNet { get; set; }

        [JsonProperty("keypoololdest")]
        public int KeyPoolOldEst { get; set; }

        [JsonProperty("keypoolsize")]
        public int KeypoolSize { get; set; }

        [JsonProperty("paytxfee")]
        public double PayTxFee { get; set; }

        [JsonProperty("relayfee")]
        public double RelayFee { get; set; }

        [JsonProperty("errors")]
        public string Errors { get; set; }
    }
}
