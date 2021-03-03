using System;
using System.IO;
using System.Text.Json;

namespace CryptoInvest
{
    public class InputParser
    {
        public static Input Parse(string inputFilePath)
        {
            if (!File.Exists(inputFilePath))
            {
                throw new InputParserException($"Input file {inputFilePath} does not exist.");
            }

            Input input;
            try
            {
                input = JsonSerializer.Deserialize<Input>(File.ReadAllText(inputFilePath), new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            catch (Exception ex)
            {
                throw new InputParserException("Error when parsing input file.", ex);
            }

            if (!TimeSpan.TryParse(input.BuyingInterval, out _))
            {
                throw new InputParserException($"{nameof(input.BuyingInterval)} is not parsable as time.");
            }

            if (!TimeSpan.TryParse(input.RebalancingInterval, out _))
            {
                throw new InputParserException($"{nameof(input.RebalancingInterval)} is not parsable as time.");
            }

            if (!Enum.TryParse(typeof(ReferenceTotalMarketCap), input.ReferenceTotalMarketCap, out _))
            {
                throw new InputParserException(
                    $"{nameof(input.ReferenceTotalMarketCap)} is not parsable. Allowed values are {string.Join(", ", Enum.GetNames(typeof(ReferenceTotalMarketCap)))}."
                );
            }

            if (input.From > input.To)
            {
                throw new InputParserException($"{nameof(input.From)} cannot be before {nameof(input.To)}.");
            }

            return input;
        }
    }
}
