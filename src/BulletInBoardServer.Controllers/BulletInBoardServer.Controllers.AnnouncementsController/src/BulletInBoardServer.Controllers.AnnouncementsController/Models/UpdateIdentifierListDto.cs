/*
 * API Шлюз. Объявления
 *
 * API шлюза для управления объявлениями
 *
 * The version of the OpenAPI document: 0.0.3
 *
 * Generated by: https://openapi-generator.tech
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BulletInBoardServer.Controllers.AnnouncementsController.Models
{ 
    /// <summary>
    /// Объект для обновления списка прикрепленных идентификаторов. Null, если список идентификаторов не требуется изменять
    /// </summary>
    [DataContract]
    public class UpdateIdentifierListDto : IEquatable<UpdateIdentifierListDto>
    {
        /// <summary>
        /// Список уникальных идентификаторов
        /// </summary>
        /// <value>Список уникальных идентификаторов</value>
        [DataMember(Name="toAdd", EmitDefaultValue=true)]
        public List<Guid> ToAdd { get; set; }

        /// <summary>
        /// Список уникальных идентификаторов
        /// </summary>
        /// <value>Список уникальных идентификаторов</value>
        [DataMember(Name="toRemove", EmitDefaultValue=true)]
        public List<Guid> ToRemove { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class UpdateIdentifierListDto {\n");
            sb.Append("  ToAdd: ").Append(ToAdd).Append("\n");
            sb.Append("  ToRemove: ").Append(ToRemove).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((UpdateIdentifierListDto)obj);
        }

        /// <summary>
        /// Returns true if UpdateIdentifierListDto instances are equal
        /// </summary>
        /// <param name="other">Instance of UpdateIdentifierListDto to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(UpdateIdentifierListDto other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    ToAdd == other.ToAdd ||
                    ToAdd != null &&
                    other.ToAdd != null &&
                    ToAdd.SequenceEqual(other.ToAdd)
                ) && 
                (
                    ToRemove == other.ToRemove ||
                    ToRemove != null &&
                    other.ToRemove != null &&
                    ToRemove.SequenceEqual(other.ToRemove)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                    if (ToAdd != null)
                    hashCode = hashCode * 59 + ToAdd.GetHashCode();
                    if (ToRemove != null)
                    hashCode = hashCode * 59 + ToRemove.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(UpdateIdentifierListDto left, UpdateIdentifierListDto right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UpdateIdentifierListDto left, UpdateIdentifierListDto right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
