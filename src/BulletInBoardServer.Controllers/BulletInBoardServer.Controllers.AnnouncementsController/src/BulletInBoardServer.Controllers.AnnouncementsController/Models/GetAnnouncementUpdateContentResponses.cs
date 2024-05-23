/*
 * API Шлюз. Объявления
 *
 * API шлюза для управления объявлениями
 *
 * The version of the OpenAPI document: 0.0.3
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
        /// Ответы:   getAnnouncementUpdateContentForbidden - Пользователь не имеет права получить данные для редактирования объявление   announcementDoesNotExist - Объявление не существует 
        /// </summary>
        /// <value>Ответы:   getAnnouncementUpdateContentForbidden - Пользователь не имеет права получить данные для редактирования объявление   announcementDoesNotExist - Объявление не существует </value>
        [TypeConverter(typeof(CustomEnumConverter<GetAnnouncementUpdateContentResponses>))]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public enum GetAnnouncementUpdateContentResponses
        {
            
            /// <summary>
            /// Enum GetAnnouncementUpdateContentForbidden for getAnnouncementUpdateContentForbidden
            /// </summary>
            [EnumMember(Value = "getAnnouncementUpdateContentForbidden")]
            GetAnnouncementUpdateContentForbidden = 1,
            
            /// <summary>
            /// Enum AnnouncementDoesNotExist for announcementDoesNotExist
            /// </summary>
            [EnumMember(Value = "announcementDoesNotExist")]
            AnnouncementDoesNotExist = 2
        }
}