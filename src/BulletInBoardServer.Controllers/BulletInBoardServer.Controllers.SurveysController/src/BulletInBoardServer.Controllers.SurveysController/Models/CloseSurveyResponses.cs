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
        /// Ответы:   surveyClosingForbidden - Пользователь не имеет права закрыть этот опрос   surveyDoesNotExist - Опрос не существует   surveyAlreadyClosed - Опрос уже закрыт   announcementWithSurveyNotYetPublished - Объявление с опросом еще не опубликовано 
        /// </summary>
        /// <value>Ответы:   surveyClosingForbidden - Пользователь не имеет права закрыть этот опрос   surveyDoesNotExist - Опрос не существует   surveyAlreadyClosed - Опрос уже закрыт   announcementWithSurveyNotYetPublished - Объявление с опросом еще не опубликовано </value>
        [TypeConverter(typeof(CustomEnumConverter<CloseSurveyResponses>))]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public enum CloseSurveyResponses
        {
            
            /// <summary>
            /// Enum SurveyClosingForbidden for surveyClosingForbidden
            /// </summary>
            [EnumMember(Value = "surveyClosingForbidden")]
            SurveyClosingForbidden = 1,
            
            /// <summary>
            /// Enum SurveyDoesNotExist for surveyDoesNotExist
            /// </summary>
            [EnumMember(Value = "surveyDoesNotExist")]
            SurveyDoesNotExist = 2,
            
            /// <summary>
            /// Enum SurveyAlreadyClosed for surveyAlreadyClosed
            /// </summary>
            [EnumMember(Value = "surveyAlreadyClosed")]
            SurveyAlreadyClosed = 3,
            
            /// <summary>
            /// Enum AnnouncementWithSurveyNotYetPublished for announcementWithSurveyNotYetPublished
            /// </summary>
            [EnumMember(Value = "announcementWithSurveyNotYetPublished")]
            AnnouncementWithSurveyNotYetPublished = 4
        }
}