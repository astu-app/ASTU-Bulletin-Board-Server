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
        /// Ответы:   announcementHidingForbidden - Пользователь не имеет права скрыть объявление   announcementDoesNotExist - Объявление не существует   announcementAlreadyHidden - Объявление уже скрыто   announcementNotYetPublished - Объявление еще не опубликовано 
        /// </summary>
        /// <value>Ответы:   announcementHidingForbidden - Пользователь не имеет права скрыть объявление   announcementDoesNotExist - Объявление не существует   announcementAlreadyHidden - Объявление уже скрыто   announcementNotYetPublished - Объявление еще не опубликовано </value>
        [TypeConverter(typeof(CustomEnumConverter<HidePostedAnnouncementResponses>))]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public enum HidePostedAnnouncementResponses
        {
            
            /// <summary>
            /// Enum AnnouncementHidingForbidden for announcementHidingForbidden
            /// </summary>
            [EnumMember(Value = "announcementHidingForbidden")]
            AnnouncementHidingForbidden = 1,
            
            /// <summary>
            /// Enum AnnouncementDoesNotExist for announcementDoesNotExist
            /// </summary>
            [EnumMember(Value = "announcementDoesNotExist")]
            AnnouncementDoesNotExist = 2,
            
            /// <summary>
            /// Enum AnnouncementAlreadyHidden for announcementAlreadyHidden
            /// </summary>
            [EnumMember(Value = "announcementAlreadyHidden")]
            AnnouncementAlreadyHidden = 3,
            
            /// <summary>
            /// Enum AnnouncementNotYetPublished for announcementNotYetPublished
            /// </summary>
            [EnumMember(Value = "announcementNotYetPublished")]
            AnnouncementNotYetPublished = 4
        }
}
