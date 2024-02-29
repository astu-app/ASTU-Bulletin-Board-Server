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
        /// Ответы:   postedAnnouncementsListAccessForbidden - Пользователь не имеет права просматривать списки опубликованных объявлений 
        /// </summary>
        /// <value>Ответы:   postedAnnouncementsListAccessForbidden - Пользователь не имеет права просматривать списки опубликованных объявлений </value>
        [TypeConverter(typeof(CustomEnumConverter<GetPostedAnnouncementListResponses>))]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public enum GetPostedAnnouncementListResponses
        {
            
            /// <summary>
            /// Enum PostedAnnouncementsListAccessForbidden for postedAnnouncementsListAccessForbidden
            /// </summary>
            [EnumMember(Value = "postedAnnouncementsListAccessForbidden")]
            PostedAnnouncementsListAccessForbidden = 1
        }
}
