using System;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Common;

/// <summary>
///     Represents a monetary amount with currency.
/// </summary>
public sealed record Money
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="Money" /> record.
    /// </summary>
    /// <param name="amount">The amount in the smallest currency unit.</param>
    /// <param name="currency">The currency code.</param>
    public Money(long amount, string currency = "NGN")
    {
        if (amount < 0) throw new ArgumentException("Amount cannot be negative", nameof(amount));

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be null or empty", nameof(currency));

        Amount = amount;
        Currency = currency.ToUpperInvariant();
    }

    /// <summary>
    ///     Gets the amount in the smallest currency unit (e.g., kobo for NGN).
    /// </summary>
    [JsonPropertyName("amount")]
    public long Amount { get; init; }

    /// <summary>
    ///     Gets the currency code (ISO 4217).
    /// </summary>
    [JsonPropertyName("currency")]
    public string Currency { get; init; } = "NGN";

    /// <summary>
    ///     Gets the decimal amount (e.g., 1000 kobo = 10.00 NGN).
    /// </summary>
    [JsonIgnore]
    public decimal DecimalAmount => Amount / 100m;

    /// <summary>
    ///     Creates a Money instance from a decimal amount.
    /// </summary>
    /// <param name="decimalAmount">The decimal amount.</param>
    /// <param name="currency">The currency code.</param>
    /// <returns>A new Money instance.</returns>
    public static Money FromDecimal(decimal decimalAmount, string currency = "NGN")
    {
        if (decimalAmount < 0) throw new ArgumentException("Decimal amount cannot be negative", nameof(decimalAmount));

        var amount = (long)(decimalAmount * 100);
        return new Money(amount, currency);
    }

    /// <summary>
    ///     Creates a Money instance from kobo amount.
    /// </summary>
    /// <param name="kobo">The amount in kobo.</param>
    /// <returns>A new Money instance in NGN.</returns>
    public static Money FromKobo(long kobo)
    {
        return new Money(kobo);
    }

    /// <summary>
    ///     Creates a Money instance from naira amount.
    /// </summary>
    /// <param name="naira">The amount in naira.</param>
    /// <returns>A new Money instance in NGN.</returns>
    public static Money FromNaira(decimal naira)
    {
        return FromDecimal(naira);
    }

    /// <summary>
    ///     Adds two Money instances.
    /// </summary>
    public static Money operator +(Money left, Money right)
    {
        ValidateSameCurrency(left, right);
        return new Money(left.Amount + right.Amount, left.Currency);
    }

    /// <summary>
    ///     Subtracts two Money instances.
    /// </summary>
    public static Money operator -(Money left, Money right)
    {
        ValidateSameCurrency(left, right);
        if (left.Amount < right.Amount) throw new InvalidOperationException("Cannot subtract to negative amount");
        return new Money(left.Amount - right.Amount, left.Currency);
    }

    /// <summary>
    ///     Multiplies a Money instance by a factor.
    /// </summary>
    public static Money operator *(Money money, decimal factor)
    {
        if (factor < 0) throw new ArgumentException("Factor cannot be negative", nameof(factor));

        var newAmount = (long)(money.Amount * factor);
        return new Money(newAmount, money.Currency);
    }

    /// <summary>
    ///     Divides a Money instance by a divisor.
    /// </summary>
    public static Money operator /(Money money, decimal divisor)
    {
        if (divisor <= 0) throw new ArgumentException("Divisor must be positive", nameof(divisor));

        var newAmount = (long)(money.Amount / divisor);
        return new Money(newAmount, money.Currency);
    }


    /// <summary>
    ///     Compares if left is greater than right.
    /// </summary>
    public static bool operator >(Money left, Money right)
    {
        ValidateSameCurrency(left, right);
        return left.Amount > right.Amount;
    }

    /// <summary>
    ///     Compares if left is less than right.
    /// </summary>
    public static bool operator <(Money left, Money right)
    {
        ValidateSameCurrency(left, right);
        return left.Amount < right.Amount;
    }

    /// <summary>
    ///     Compares if left is greater than or equal to right.
    /// </summary>
    public static bool operator >=(Money left, Money right)
    {
        ValidateSameCurrency(left, right);
        return left.Amount >= right.Amount;
    }

    /// <summary>
    ///     Compares if left is less than or equal to right.
    /// </summary>
    public static bool operator <=(Money left, Money right)
    {
        ValidateSameCurrency(left, right);
        return left.Amount <= right.Amount;
    }

    private static void ValidateSameCurrency(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException(
                $"Cannot operate on different currencies: {left.Currency} and {right.Currency}");
    }

    /// <summary>
    ///     Returns a string representation of the money amount.
    /// </summary>
    public override string ToString()
    {
        return Currency switch
        {
            "NGN" => $"₦{DecimalAmount:N2}",
            "USD" => $"${DecimalAmount:N2}",
            "EUR" => $"€{DecimalAmount:N2}",
            "GBP" => $"£{DecimalAmount:N2}",
            _ => $"{DecimalAmount:N2} {Currency}"
        };
    }

    /// <summary>
    ///     Returns a formatted string representation using the specified format.
    /// </summary>
    public string ToString(string format, IFormatProvider? formatProvider = null)
    {
        formatProvider ??= CultureInfo.CurrentCulture;
        return DecimalAmount.ToString(format, formatProvider) + " " + Currency;
    }
}