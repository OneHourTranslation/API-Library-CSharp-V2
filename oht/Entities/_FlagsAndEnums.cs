using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace oht.Entities
{
    /// <summary>
    /// Target language availability textual enum
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum LanguageAvailability
    {
        [EnumMember(Value = "")]
        None,
        [EnumMember(Value = "high")]
        High,
        [EnumMember(Value = "medium")]
        Medium,
        [EnumMember(Value = "low")]
        Low
    }

    /// <summary>
    /// Textual enum representation of currency code
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CurrencyType
    {
        [EnumMember(Value = "")]
        None,
        [EnumMember(Value = "USD")]
        USD,
        [EnumMember(Value = "EUR")]
        EUR,
        [EnumMember(Value = "GBP")]
        GBP
    }

    /// <summary>
    /// Textual enum representing Project Status
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProjectStatusCode
    {
        [EnumMember(Value = "")]
        None,
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "in_progress")]
        InProgress,
        [EnumMember(Value = "submitted")]
        Submitted,
        [EnumMember(Value = "signed")]
        Signed,
        [EnumMember(Value = "completed")]
        Completed,
        [EnumMember(Value = "canceled")]
        Canceled
    }

    /// <summary>
    /// Textual enum representing Project Type
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProjectType
    {
        [EnumMember(Value = "")]
        None,
        [EnumMember(Value = "Translation")]
        Translation,
        [EnumMember(Value = "Expert Translation")]
        ExpertTranslation,
        [EnumMember(Value = "Proofreading")]
        Proofreading,
        [EnumMember(Value = "Transcription")]
        Transcription,
        [EnumMember(Value = "Translation+Proofreading")]
        TranslationProofreading
    }

    /// <summary>
    /// Project rate type enum
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RateType
    {
        [EnumMember(Value = "customer")]
        Customer = 1,
        [EnumMember(Value = "service")]
        Service = 2
    }

    /// <summary>
    /// Project service rate plags
    /// </summary>
    [Flags]
    public enum ServiceRate
    {
        None = 0,
        [EnumMember(Value = "service_was_on_time")]
        WasOnTime = 1,
        [EnumMember(Value = "service_support_helpful")]
        SupportHelpful = 2,
        [EnumMember(Value = "service_good_quality")]
        GoodQuality = 4,
        [EnumMember(Value = "service_trans_responded")]
        TransResponded = 8,
        [EnumMember(Value = "service_would_recommend")]
        WouldRecommend = 16
    }

    /// <summary>
    /// Projects customer rate flags
    /// </summary>
    [Flags]
    public enum CustomerRate
    {
        None = 0,
        [EnumMember(Value = "trans_is_good")]
        TranslationIsGood = 1,                  // Good translation quality.
        [EnumMember(Value = "trans_bad_formatting")]
        BadFormatting = 2,                      // Bad formatting.
        [EnumMember(Value = "trans_misunderstood_source")]
        MisunderstoodSource = 4,                // Misrepresent / Misunderstood the source.
        [EnumMember(Value = "trans_spell_tps_grmr_mistakes")]
        SpellTyposGrammarMistakes = 8,          // Spelling / Typos / Grammar mistakes.
        [EnumMember(Value = "trans_text_miss")]
        TextMissiing = 16,                      // Missing text / Partly translated.
        [EnumMember(Value = "trans_not_followed_instrctns")]
        InstructionsNotFollowed = 32,           // Instructions not followed.
        [EnumMember(Value = "trans_inconsistent")]
        Inconsistent = 64,                      // Inconsistent translation.
        [EnumMember(Value = "trans_bad_written")]
        BadWritten = 128                        // Badly written.
    }

    /// <summary>
    /// Expertise type enum
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ExpertiseType
    {
        [EnumMember(Value = "")]
        None,
        [EnumMember(Value = "automotive-aerospace")]
        Automotive_Aerospace,
        [EnumMember(Value = "business-finance")]
        Business_Forex_Finance,
        [EnumMember(Value = "software-it")]
        Software_IT,
        [EnumMember(Value = "legal-certificate")]
        Legal,
        [EnumMember(Value = "marketing-consumer-media")]
        Marketing_Consumer_Media,
        [EnumMember(Value = "cv")]
        CV,
        [EnumMember(Value = "medical")]
        Medical,
        [EnumMember(Value = "patents")]
        Patents,
        [EnumMember(Value = "scientific-academic")]
        Scientific_Academic,
        [EnumMember(Value = "technical-engineering")]
        Technical_Engineering,
        [EnumMember(Value = "gaming-video-games")]
        Gaming_Video_Games,
        [EnumMember(Value = "ad-words-banners")]
        Ad_Words_Banners,
        [EnumMember(Value = "mobile-applications")]
        Mobile_Applications,
        [EnumMember(Value = "tourism")]
        Tourism,
        [EnumMember(Value = "certificates-translation")]
        Certificates_Translation
    }

    /// <summary>
    /// Project commenter role enum
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CommenterRole
    {
        [EnumMember(Value = "")]
        None,
        [EnumMember(Value = "admin")]
        Admin,
        [EnumMember(Value = "customer")]
        Customer,
        [EnumMember(Value = "provider")]
        Provider,
        [EnumMember(Value = "potential-provider")]
        PotentialProvider
    }

    /// <summary>
    /// Resource type enum
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ResourceType
    {
        [EnumMember(Value = "")]
        None,
        [EnumMember(Value = "text")]
        Text,
        [EnumMember(Value = "file")]
        File
    }

    /// <summary>
    /// Service type enum
    /// </summary>
    public enum ServiceType
    {
        [EnumMember(Value = "")]
        None,
        [EnumMember(Value = "translation")]
        Translation,
        [EnumMember(Value = "proofreading")]
        Proofreading,
        [EnumMember(Value = "transproof")]
        Transproof,
        [EnumMember(Value = "transcription")]
        Transcription
    }

    /// <summary>
    /// Proofreading: yes,no
    /// </summary>
    public enum ProofReading
    {
        [EnumMember(Value = "")]
        None,
        [EnumMember(Value = "0")]
        No,
        [EnumMember(Value = "1")]
        Yes
    }

    /// <summary>
    /// Class implements Enum extension to return string value
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Returns description of particular enum value set via EnumMemberAttribute 
        /// </summary>
        /// <typeparam name="EnumType">Generic enum type</typeparam>
        /// <param name="e">Enum value</param>
        /// <returns>Description of enum value</returns>
        public static string GetStringValue<EnumType>(this EnumType e) where EnumType : struct, IConvertible
        {
            var result = "";
            MemberInfo memberInfo = typeof(EnumType).GetMember(e.ToString()).FirstOrDefault();

            if (memberInfo != null)
            {
                EnumMemberAttribute attribute = (EnumMemberAttribute)memberInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false).FirstOrDefault();
                if (attribute != null)
                    result = attribute.Value;
            }

            return result;
        }
    }

    /// <summary>
    /// Class implements Enum extension method to enumerate all flags set
    /// </summary>
    internal static class FlagsExtenstions
    {
        public static IEnumerable<Enum> GetUniqueFlags(this Enum flags)
        {
            ulong flag = 1;
            foreach (var value in Enum.GetValues(flags.GetType()).Cast<Enum>())
            {
                ulong bits = Convert.ToUInt64(value);
                while (flag < bits)
                {
                    flag <<= 1;
                }

                if (flag == bits && flags.HasFlag(value))
                {
                    yield return value;
                }
            }
        }
    }    
}
