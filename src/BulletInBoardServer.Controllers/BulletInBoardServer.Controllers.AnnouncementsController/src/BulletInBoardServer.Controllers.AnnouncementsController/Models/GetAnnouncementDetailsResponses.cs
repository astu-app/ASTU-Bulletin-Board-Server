/*
 * API Шлюз. Объявления
 *
 * API шлюза для управления объявлениями
 *
 * The version of the OpenAPI document: 0.0.2
 *
 * Generated by: https://openapi-generator.tech
 */

using System.ComponentModel;
using System.Runtime.Serialization;
using BulletInBoardServer.Controllers.AnnouncementsController.Converters;
using Newtonsoft.Json;

namespace BulletInBoardServer.Controllers.AnnouncementsController.Models
{ 
        /// <summary>
        /// Ответы:   ok - Ок   incorrectIdentifierFormat - Некорректный формат идентификатора    unauthorized - Пользователь не авторизован для выполнения операции   detailsAccessForbidden - Пользователь не имеет права просмотреть подробности объявления   announcementDoesNotExist - Объявление не существует 
        /// </summary>
        /// <value>Ответы:   ok - Ок   incorrectIdentifierFormat - Некорректный формат идентификатора    unauthorized - Пользователь не авторизован для выполнения операции   detailsAccessForbidden - Пользователь не имеет права просмотреть подробности объявления   announcementDoesNotExist - Объявление не существует </value>
        [TypeConverter(typeof(CustomEnumConverter<GetAnnouncementDetailsResponses>))]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public enum GetAnnouncementDetailsResponses
        {
            
            /// <summary>
            /// Enum Ok for ok
            /// </summary>
            [EnumMember(Value = "ok")]
            Ok = 1,
            
            /// <summary>
            /// Enum IncorrectIdentifierFormat for incorrectIdentifierFormat
            /// </summary>
            [EnumMember(Value = "incorrectIdentifierFormat")]
            IncorrectIdentifierFormat = 2,
            
            /// <summary>
            /// Enum DetailsAccessForbidden for detailsAccessForbidden
            /// </summary>
            [EnumMember(Value = "detailsAccessForbidden")]
            DetailsAccessForbidden = 3,
            
            /// <summary>
            /// Enum AnnouncementDoesNotExist for announcementDoesNotExist
            /// </summary>
            [EnumMember(Value = "announcementDoesNotExist")]
            AnnouncementDoesNotExist = 4
        }
}
