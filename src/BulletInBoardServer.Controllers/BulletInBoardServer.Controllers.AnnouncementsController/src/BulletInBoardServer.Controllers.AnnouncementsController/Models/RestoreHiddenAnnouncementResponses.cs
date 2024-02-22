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
        /// Ответы:   restoreForbidden - Пользователь не имеет права восстановить скрытое объявление   announcementDoesNotExist - Объявление не существует   announcementNotHidden - Объявление не является скрытым 
        /// </summary>
        /// <value>Ответы:   restoreForbidden - Пользователь не имеет права восстановить скрытое объявление   announcementDoesNotExist - Объявление не существует   announcementNotHidden - Объявление не является скрытым </value>
        [TypeConverter(typeof(CustomEnumConverter<RestoreHiddenAnnouncementResponses>))]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public enum RestoreHiddenAnnouncementResponses
        {
            
            /// <summary>
            /// Enum RestoreForbidden for restoreForbidden
            /// </summary>
            [EnumMember(Value = "restoreForbidden")]
            RestoreForbidden = 1,
            
            /// <summary>
            /// Enum AnnouncementDoesNotExist for announcementDoesNotExist
            /// </summary>
            [EnumMember(Value = "announcementDoesNotExist")]
            AnnouncementDoesNotExist = 2,
            
            /// <summary>
            /// Enum AnnouncementNotHidden for announcementNotHidden
            /// </summary>
            [EnumMember(Value = "announcementNotHidden")]
            AnnouncementNotHidden = 3
        }
}
