/*
 * API Шлюз. Объявления
 *
 * API шлюза для управления объявлениями
 *
 * The version of the OpenAPI document: 0.0.2
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
    /// 
    /// </summary>
    [DataContract]
    public class GetDelayedPublishingAnnouncementListOk : IEquatable<GetDelayedPublishingAnnouncementListOk>
    {
        /// <summary>
        /// Gets or Sets Content
        /// </summary>
        [DataMember(Name="content", EmitDefaultValue=false)]
        public List<AnnouncementSummaryDto> Content { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class GetDelayedPublishingAnnouncementListOk {\n");
            sb.Append("  Content: ").Append(Content).Append("\n");
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
            return obj.GetType() == GetType() && Equals((GetDelayedPublishingAnnouncementListOk)obj);
        }

        /// <summary>
        /// Returns true if GetDelayedPublishingAnnouncementListOk instances are equal
        /// </summary>
        /// <param name="other">Instance of GetDelayedPublishingAnnouncementListOk to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(GetDelayedPublishingAnnouncementListOk other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Content == other.Content ||
                    Content != null &&
                    other.Content != null &&
                    Content.SequenceEqual(other.Content)
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
                    if (Content != null)
                    hashCode = hashCode * 59 + Content.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(GetDelayedPublishingAnnouncementListOk left, GetDelayedPublishingAnnouncementListOk right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(GetDelayedPublishingAnnouncementListOk left, GetDelayedPublishingAnnouncementListOk right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
