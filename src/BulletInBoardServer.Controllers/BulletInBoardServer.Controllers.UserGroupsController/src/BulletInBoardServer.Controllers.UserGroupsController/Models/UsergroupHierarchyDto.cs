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
    /// Иерархия групп пользователей
    /// </summary>
    [DataContract]
    public class UsergroupHierarchyDto : IEquatable<UsergroupHierarchyDto>
    {
        /// <summary>
        /// Список групп пользователей иерархии с включенными участниками
        /// </summary>
        /// <value>Список групп пользователей иерархии с включенными участниками</value>
        [DataMember(Name="usergroups", EmitDefaultValue=false)]
        public List<UserGroupSummaryWithMembersDto> Usergroups { get; set; }

        /// <summary>
        /// Корни иерархии
        /// </summary>
        /// <value>Корни иерархии</value>
        [DataMember(Name="roots", EmitDefaultValue=false)]
        public List<UserGroupHierarchyNodeDto> Roots { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class UsergroupHierarchyDto {\n");
            sb.Append("  Usergroups: ").Append(Usergroups).Append("\n");
            sb.Append("  Roots: ").Append(Roots).Append("\n");
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
            return obj.GetType() == GetType() && Equals((UsergroupHierarchyDto)obj);
        }

        /// <summary>
        /// Returns true if UsergroupHierarchyDto instances are equal
        /// </summary>
        /// <param name="other">Instance of UsergroupHierarchyDto to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(UsergroupHierarchyDto other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Usergroups == other.Usergroups ||
                    Usergroups != null &&
                    other.Usergroups != null &&
                    Usergroups.SequenceEqual(other.Usergroups)
                ) && 
                (
                    Roots == other.Roots ||
                    Roots != null &&
                    other.Roots != null &&
                    Roots.SequenceEqual(other.Roots)
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
                    if (Usergroups != null)
                    hashCode = hashCode * 59 + Usergroups.GetHashCode();
                    if (Roots != null)
                    hashCode = hashCode * 59 + Roots.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(UsergroupHierarchyDto left, UsergroupHierarchyDto right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UsergroupHierarchyDto left, UsergroupHierarchyDto right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}