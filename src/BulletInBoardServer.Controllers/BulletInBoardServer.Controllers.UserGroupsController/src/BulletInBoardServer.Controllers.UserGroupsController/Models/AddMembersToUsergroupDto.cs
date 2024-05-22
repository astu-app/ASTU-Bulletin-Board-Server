/*
 * API Шлюз. Группы пользователей
 *
 * API шлюза для управления группами пользователей
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

namespace BulletInBoardServer.Controllers.UserGroupsController.Models
{ 
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class AddMembersToUsergroupDto : IEquatable<AddMembersToUsergroupDto>
    {
        /// <summary>
        /// Gets or Sets UserGroupId
        /// </summary>
        [DataMember(Name="userGroupId", EmitDefaultValue=false)]
        public Guid UserGroupId { get; set; }

        /// <summary>
        /// Список уникальных идентификаторов
        /// </summary>
        /// <value>Список уникальных идентификаторов</value>
        [DataMember(Name="userIds", EmitDefaultValue=true)]
        public List<Guid> UserIds { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class AddMembersToUsergroupDto {\n");
            sb.Append("  UserGroupId: ").Append(UserGroupId).Append("\n");
            sb.Append("  UserIds: ").Append(UserIds).Append("\n");
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
            return obj.GetType() == GetType() && Equals((AddMembersToUsergroupDto)obj);
        }

        /// <summary>
        /// Returns true if AddMembersToUsergroupDto instances are equal
        /// </summary>
        /// <param name="other">Instance of AddMembersToUsergroupDto to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(AddMembersToUsergroupDto other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    UserGroupId == other.UserGroupId ||
                    UserGroupId != null &&
                    UserGroupId.Equals(other.UserGroupId)
                ) && 
                (
                    UserIds == other.UserIds ||
                    UserIds != null &&
                    other.UserIds != null &&
                    UserIds.SequenceEqual(other.UserIds)
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
                    if (UserGroupId != null)
                    hashCode = hashCode * 59 + UserGroupId.GetHashCode();
                    if (UserIds != null)
                    hashCode = hashCode * 59 + UserIds.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(AddMembersToUsergroupDto left, AddMembersToUsergroupDto right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AddMembersToUsergroupDto left, AddMembersToUsergroupDto right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
