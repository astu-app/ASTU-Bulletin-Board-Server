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
        /// Ответы:    getDelayedHiddenAnnouncementListAccessForbidden - Пользователь не имеет права просматривать списки объявлений, ожидающих отложенное сокрытие
        /// </summary>
        /// <value>Ответы:    getDelayedHiddenAnnouncementListAccessForbidden - Пользователь не имеет права просматривать списки объявлений, ожидающих отложенное сокрытие</value>
        [TypeConverter(typeof(CustomEnumConverter<GetDelayedHiddenAnnouncementListResponses>))]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public enum GetDelayedHiddenAnnouncementListResponses
        {
            
            /// <summary>
            /// Enum GetDelayedHiddenAnnouncementListAccessForbidden for getDelayedHiddenAnnouncementListAccessForbidden
            /// </summary>
            [EnumMember(Value = "getDelayedHiddenAnnouncementListAccessForbidden")]
            GetDelayedHiddenAnnouncementListAccessForbidden = 1
        }
}
