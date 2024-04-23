/*
 * API Шлюз. Опросы
 *
 * API шлюза для управления опросами
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

namespace BulletInBoardServer.Controllers.SurveysController.Models
{ 
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class QuestionDetailsDto : IEquatable<QuestionDetailsDto>
    {
        /// <summary>
        /// Идентификатор опроса
        /// </summary>
        /// <value>Идентификатор опроса</value>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public Guid Id { get; set; }

        /// <summary>
        /// Порядковый номер вопроса в списке вопросов
        /// </summary>
        /// <value>Порядковый номер вопроса в списке вопросов</value>
        [DataMember(Name="serial", EmitDefaultValue=true)]
        public int Serial { get; set; }

        /// <summary>
        /// Текстовое содержимое вопроса
        /// </summary>
        /// <value>Текстовое содержимое вопроса</value>
        [DataMember(Name="content", EmitDefaultValue=false)]
        public string Content { get; set; }

        /// <summary>
        /// Разрешен ли множественный выбор
        /// </summary>
        /// <value>Разрешен ли множественный выбор</value>
        [DataMember(Name="isMultipleChoiceAllowed", EmitDefaultValue=true)]
        public bool IsMultipleChoiceAllowed { get; set; } = false;

        /// <summary>
        /// Варианты ответов опроса
        /// </summary>
        /// <value>Варианты ответов опроса</value>
        [DataMember(Name="answers", EmitDefaultValue=false)]
        public List<QuestionAnswerDetailsDto> Answers { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class QuestionDetailsDto {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Serial: ").Append(Serial).Append("\n");
            sb.Append("  Content: ").Append(Content).Append("\n");
            sb.Append("  IsMultipleChoiceAllowed: ").Append(IsMultipleChoiceAllowed).Append("\n");
            sb.Append("  Answers: ").Append(Answers).Append("\n");
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
            return obj.GetType() == GetType() && Equals((QuestionDetailsDto)obj);
        }

        /// <summary>
        /// Returns true if QuestionDetailsDto instances are equal
        /// </summary>
        /// <param name="other">Instance of QuestionDetailsDto to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(QuestionDetailsDto other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Id == other.Id ||
                    Id != null &&
                    Id.Equals(other.Id)
                ) && 
                (
                    Serial == other.Serial ||
                    
                    Serial.Equals(other.Serial)
                ) && 
                (
                    Content == other.Content ||
                    Content != null &&
                    Content.Equals(other.Content)
                ) && 
                (
                    IsMultipleChoiceAllowed == other.IsMultipleChoiceAllowed ||
                    
                    IsMultipleChoiceAllowed.Equals(other.IsMultipleChoiceAllowed)
                ) && 
                (
                    Answers == other.Answers ||
                    Answers != null &&
                    other.Answers != null &&
                    Answers.SequenceEqual(other.Answers)
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
                    if (Id != null)
                    hashCode = hashCode * 59 + Id.GetHashCode();
                    
                    hashCode = hashCode * 59 + Serial.GetHashCode();
                    if (Content != null)
                    hashCode = hashCode * 59 + Content.GetHashCode();
                    
                    hashCode = hashCode * 59 + IsMultipleChoiceAllowed.GetHashCode();
                    if (Answers != null)
                    hashCode = hashCode * 59 + Answers.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(QuestionDetailsDto left, QuestionDetailsDto right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(QuestionDetailsDto left, QuestionDetailsDto right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
