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
        /// Ответы:    getDelayedPublishingAnnouncementListResponsesAccessForbidden - Пользователь не имеет права просматривать списки объявлений, ожидающих отложенное сокрытие 
        /// </summary>
        /// <value>Ответы:    getDelayedPublishingAnnouncementListResponsesAccessForbidden - Пользователь не имеет права просматривать списки объявлений, ожидающих отложенное сокрытие </value>
        [TypeConverter(typeof(CustomEnumConverter<GetDelayedPublishingAnnouncementListResponses>))]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public enum GetDelayedPublishingAnnouncementListResponses
        {
            
            /// <summary>
            /// Enum GetDelayedPublishingAnnouncementListResponsesAccessForbidden for getDelayedPublishingAnnouncementListResponsesAccessForbidden
            /// </summary>
            [EnumMember(Value = "getDelayedPublishingAnnouncementListResponsesAccessForbidden")]
            GetDelayedPublishingAnnouncementListResponsesAccessForbidden = 1
        }
}
