using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Dal
{
    public abstract class BaseModel
    {
        protected BaseModel()
        {
            Id = Guid.NewGuid();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int RowId { get; set; }

        [Index(IsUnique = true)]
        public Guid Id { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }

    public abstract class WalletDataModel : BaseModel
    {
        public string Account { get; set; }
        public string Address { get; set; }
        public float Amount { get; set; }
    }

    public abstract class TransactionModel : WalletDataModel
    {
        public string TxId { get; set; }
        public int Confirmations { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime TimeTransaction { get; set; }
    }

    [Table("Seeding")]
    public class SeedModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public bool IsSeed { get; set; }
    }

    [Table("EnumTypes")]
    public class EnumTypeModel : BaseModel
    {
        public EnumTypeModel()
        {
        }

        protected EnumTypeModel(string type, ICollection<EnumValueModel> values)
        {
            Type = type;
            Values = values;
        }

        public static EnumTypeModel Create(string type, ICollection<EnumValueModel> values)
        {
            return new EnumTypeModel(type, values);
        }

        public static EnumTypeModel Create(string type)
        {
            return new EnumTypeModel(type, Enumerable.Empty<EnumValueModel>().ToList());
        }

        public string Type { get; private set; }
        public virtual ICollection<EnumValueModel> Values { get; private set; }
    }

    [Table("EnumValues")]
    public class EnumValueModel : BaseModel
    {
        public EnumValueModel()
        {
        }

        protected EnumValueModel(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public static EnumValueModel Create(string name, int value)
        {
            return new EnumValueModel(name, value);
        }

        public virtual EnumTypeModel Type { get; set; }
        public string Name { get; private set; }
        public int Value { get; private set; }
    }

    [Table("SendBtcRequests")]
    public class SendBtcModel : BaseModel
    {
        public string Address { get; set; }
        public float Amount { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Time { get; set; }
        public bool IsExecute { get; set; }
    }

    [Table("HotWallets")]
    public class HotWalletModel: WalletDataModel
    {
        public HotWalletModel()
        {
            IncomingTransactions = new HashSet<IncomingTransactionModel>();
            OutgoingTransactions = new HashSet<OutgoingTransactionModel>();
        }

        public virtual ICollection<IncomingTransactionModel> IncomingTransactions { get; set; }
        public virtual ICollection<OutgoingTransactionModel> OutgoingTransactions { get; set; }
    }

    [Table("IncomingTransactions")]
    public class IncomingTransactionModel : TransactionModel
    {
        public bool IsInterrogation { get; set; }
    }

    [Table("OutgoingTransactions")]
    public class OutgoingTransactionModel : TransactionModel { }
}
