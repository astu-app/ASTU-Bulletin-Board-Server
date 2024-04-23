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
        /// Ответы:   createSurveyForbidden - Пользователь не имеет права создать опрос   surveyContainsQuestionSerialsDuplicates - Опрос содержит вопросы с одинаковыми порядковыми номерами   questionContainsAnswersSerialsDuplicates - Вопрос содержит варианты ответов с повторяющимися порядковыми номерами 
        /// </summary>
        /// <value>Ответы:   createSurveyForbidden - Пользователь не имеет права создать опрос   surveyContainsQuestionSerialsDuplicates - Опрос содержит вопросы с одинаковыми порядковыми номерами   questionContainsAnswersSerialsDuplicates - Вопрос содержит варианты ответов с повторяющимися порядковыми номерами </value>
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
            /// Enum SurveyContainsQuestionSerialsDuplicates for surveyContainsQuestionSerialsDuplicates
            /// </summary>
            [EnumMember(Value = "surveyContainsQuestionSerialsDuplicates")]
            SurveyContainsQuestionSerialsDuplicates = 2,
            
            /// <summary>
            /// Enum QuestionContainsAnswersSerialsDuplicates for questionContainsAnswersSerialsDuplicates
            /// </summary>
            [EnumMember(Value = "questionContainsAnswersSerialsDuplicates")]
            QuestionContainsAnswersSerialsDuplicates = 3
        }
}
