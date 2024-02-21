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
        /// Ответы:   ok - Ок   unauthorized - Пользователь не авторизован для выполнения операции   postedAnnouncementsListAccessForbidden - Пользователь не имеет права просматривать списки опубликованных объявлений 
        /// </summary>
        /// <value>Ответы:   ok - Ок   unauthorized - Пользователь не авторизован для выполнения операции   postedAnnouncementsListAccessForbidden - Пользователь не имеет права просматривать списки опубликованных объявлений </value>
        [TypeConverter(typeof(CustomEnumConverter<GetPostedAnnouncementListResponses>))]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public enum GetPostedAnnouncementListResponses
        {
            
            /// <summary>
            /// Enum Ok for ok
            /// </summary>
            [EnumMember(Value = "ok")]
            Ok = 1,
            
            /// <summary>
            /// Enum PostedAnnouncementsListAccessForbidden for postedAnnouncementsListAccessForbidden
            /// </summary>
            [EnumMember(Value = "postedAnnouncementsListAccessForbidden")]
            PostedAnnouncementsListAccessForbidden = 2
        }
}
