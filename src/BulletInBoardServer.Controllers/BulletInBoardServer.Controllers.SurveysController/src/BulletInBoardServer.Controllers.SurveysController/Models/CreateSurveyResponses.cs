/*
 * API Шлюз. Опросы
 *
 * API шлюза для управления опросами
 *
 * The version of the OpenAPI document: 0.0.2
 *
 * Generated by: https://openapi-generator.tech
 */

using System.ComponentModel;
using System.Runtime.Serialization;
using BulletInBoardServer.Controllers.SurveysController.Converters;
using Newtonsoft.Json;

namespace BulletInBoardServer.Controllers.SurveysController.Models
{ 
        /// <summary>
        /// Ответы:   createSurveyForbidden - Пользователь не имеет права создать опрос   votingClosesBeforeAnnouncementPublication - Срок окончания голосования наступит до момента публикации объявления 
        /// </summary>
        /// <value>Ответы:   createSurveyForbidden - Пользователь не имеет права создать опрос   votingClosesBeforeAnnouncementPublication - Срок окончания голосования наступит до момента публикации объявления </value>
        [TypeConverter(typeof(CustomEnumConverter<CreateSurveyResponses>))]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public enum CreateSurveyResponses
        {
            
            /// <summary>
            /// Enum CreateSurveyForbidden for createSurveyForbidden
            /// </summary>
            [EnumMember(Value = "createSurveyForbidden")]
            CreateSurveyForbidden = 1,
            
            /// <summary>
            /// Enum VotingClosesBeforeAnnouncementPublication for votingClosesBeforeAnnouncementPublication
            /// </summary>
            [EnumMember(Value = "votingClosesBeforeAnnouncementPublication")]
            VotingClosesBeforeAnnouncementPublication = 2
        }
}