using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ShockTherapy.Utils
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumConvertAttribute(string name) : Attribute
    {
        public string Name { get; } = name;
    }

    public class EnumJsonConverter<TEnum> : JsonConverter<TEnum> where TEnum : struct, Enum
    {
        public override void WriteJson(JsonWriter writer, TEnum value, JsonSerializer serializer)
        {
            var variantName = value.ToString();
            var (_, attribute) = GetVariants(value.GetType()).First((it) => it.Variant == variantName);
            writer.WriteValue(attribute?.Name ?? variantName);
        }

        public override TEnum ReadJson(JsonReader reader, Type objectType, TEnum existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var variantName = reader.Value?.ToString();
            if (variantName is null)
                return default;

            var (variant, _) = GetVariants(objectType).First(it => (it.Attribute?.Name ?? it.Variant) == variantName);
            Enum.TryParse<TEnum>(variant, true, out var result);
            return result;
        }

        private IReadOnlyCollection<(string Variant, EnumConvertAttribute? Attribute)> GetVariants(Type type)
        {
            return type.GetMembers()
                .Select(member => (
                    member.Name,
                    member.GetCustomAttributes(typeof(EnumConvertAttribute), true).FirstOrDefault() as
                        EnumConvertAttribute
                ))
                .ToArray();
        }
    }
}