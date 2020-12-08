using MongoDB.Bson.Serialization.Attributes;
using System;
using Vrnz2.Data.MongoDB.Entities.Base;

namespace Vrnz2.Challenge.Payments.Shared.Entities
{
    [BsonIgnoreExtraElements]
    public class Payment
        : BaseMongoDbEntity, IEquatable<Payment>
    {
        #region Consts

        public const int MINUTES_QTT_FOR_PAYMENTS_IS_EQUALS = 1;

        #endregion

        #region Attributes

        public Guid Tid { get; set; }

        public string Cpf { get; set; }

        public DateTime DueDate { get; set; }

        public decimal Value { get; set; }

        public override bool Equals(object obj)
            => Equals((Payment)obj);

        public bool Equals(Payment obj)
        {
            var result = false;

            result = Cpf.Equals(obj.Cpf);
            result = result && Value.Equals(obj.Value);
            result = result && Math.Abs(ReceitpDatetTime.Subtract(obj.ReceitpDatetTime).Minutes) < MINUTES_QTT_FOR_PAYMENTS_IS_EQUALS;

            return result;
        }

        #endregion
    }
}
