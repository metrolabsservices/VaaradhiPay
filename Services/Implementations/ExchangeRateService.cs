using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VaaradhiPay.Data;
using VaaradhiPay.DTOs;

namespace VaaradhiPay.Services.Implementations
{
 
    public class ExchangeRateService
    {
        private readonly ApplicationDbContext _dbContext;

        public ExchangeRateService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task FetchAndStoreExchangeRatesAsync()
        {
            var auditLog = new CurrencyExtractionAuditDTO();
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                // Step 1: Extract data
                var url = "https://www.x-rates.com/table/?from=USD&amount=1";
                var httpClient = new HttpClient();
                var response = await httpClient.GetStringAsync(url);

                var doc = new HtmlDocument();
                doc.LoadHtml(response);

                var tableNode = doc.DocumentNode.SelectSingleNode("//*[@id='content']/div[1]/div/div[1]/div[1]/table[2]");
                if (tableNode == null)
                {
                    throw new Exception("Exchange rate table not found.");
                }

                var rows = tableNode.SelectNodes("tbody/tr");
                if (rows == null || rows.Count == 0)
                {
                    throw new Exception("No exchange rate data available in the table.");
                }

                // Step 2: Parse data
                var extractedCurrencies = new List<Currency>();

                foreach (var row in rows)
                {
                    var columns = row.SelectNodes("td");
                    if (columns == null || columns.Count < 3) continue;

                    var currencyName = columns[0].InnerText.Trim().ToLower(); // Extract currency name
                    var isoCode = ExtractISOCode(currencyName); // Extract ISO code based on currency name
                    var rate = decimal.Parse(columns[1].InnerText.Trim());

                    extractedCurrencies.Add(new Currency
                    {
                        Name = currencyName,
                        ISOCode = isoCode,
                        Rate = rate,
                        LastUpdated = DateTime.UtcNow
                    });
                }

                // Step 3: Store data
                await UpdateExchangeRatesAsync(extractedCurrencies, auditLog);
                await _dbContext.SaveChangesAsync();

                // Step 4:Update CoinTypes based on updated currencies
                await UpdateCoinTypesBasedOnCurrenciesAsync();
                await _dbContext.SaveChangesAsync();

                // Step 5: Audit success
                auditLog.IsExtractionSuccessful = true;
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                // Step 6: Audit failure
                auditLog.IsExtractionSuccessful = false;
                auditLog.ErrorMessage = ex.Message;
                await transaction.RollbackAsync();
            }

            auditLog.LastUpdated = DateTime.UtcNow;

            // Step 7: Save audit log to the database
            var auditLogEntity = new CurrencyExtractionAudit
            {
                IsExtractionSuccessful = auditLog.IsExtractionSuccessful,
                ErrorMessage = auditLog.ErrorMessage,
                AddedCount = auditLog.AddedCount,
                UpdatedCount = auditLog.UpdatedCount,
                LastUpdated = auditLog.LastUpdated
            };
            _dbContext.CurrencyExtractionAudits.Add(auditLogEntity);
            await _dbContext.SaveChangesAsync();
        }

        private async Task UpdateExchangeRatesAsync(List<Currency> extractedCurrencies, CurrencyExtractionAuditDTO auditLog)
        {
            // Defensive check for null DbSet
            if (_dbContext.Currencies == null)
            {
                throw new InvalidOperationException("The Currencies DbSet is not initialized.");
            }

            // Fetch existing currencies safely
            var existingCurrencies = await _dbContext.Currencies.ToListAsync();

            // Handle null or empty list
            var currencyDictionary = existingCurrencies?.ToDictionary(c => c.Name)
                                     ?? new Dictionary<string, Currency>();

            // Log if the table is empty
            if (currencyDictionary.Count == 0)
            {
                Console.WriteLine("Currencies table is empty. Adding all extracted currencies as new records.");
            }

            foreach (var currency in extractedCurrencies)
            {
                if (currencyDictionary.TryGetValue(currency.Name, out var existingCurrency))
                {
                    // Update existing currency
                    existingCurrency.Rate = currency.Rate;
                    existingCurrency.LastUpdated = DateTime.UtcNow;
                    auditLog.UpdatedCount++;
                }
                else
                {
                    // Add new currency
                    _dbContext.Currencies.Add(currency);
                    auditLog.AddedCount++;
                }
            }

            // Save changes to the database
            await _dbContext.SaveChangesAsync();
        }

        private static string ExtractISOCode(string currencyName)
        {
            var isoCodeMap = new Dictionary<string, string>
    {
        { "argentine peso", "ARS" },
        { "australian dollar", "AUD" },
        { "bahraini dinar", "BHD" },
        { "botswana pula", "BWP" },
        { "brazilian real", "BRL" },
        { "bruneian dollar", "BND" },
        { "bulgarian lev", "BGN" },
        { "canadian dollar", "CAD" },
        { "chilean peso", "CLP" },
        { "chinese yuan renminbi", "CNY" },
        { "colombian peso", "COP" },
        { "czech koruna", "CZK" },
        { "danish krone", "DKK" },
        { "euro", "EUR" },
        { "hong kong dollar", "HKD" },
        { "hungarian forint", "HUF" },
        { "icelandic krona", "ISK" },
        { "indian rupee", "INR" },
        { "indonesian rupiah", "IDR" },
        { "iranian rial", "IRR" },
        { "israeli shekel", "ILS" },
        { "japanese yen", "JPY" },
        { "kazakhstani tenge", "KZT" },
        { "south korean won", "KRW" },
        { "kuwaiti dinar", "KWD" },
        { "libyan dinar", "LYD" },
        { "malaysian ringgit", "MYR" },
        { "mauritian rupee", "MUR" },
        { "mexican peso", "MXN" },
        { "nepalese rupee", "NPR" },
        { "new zealand dollar", "NZD" },
        { "norwegian krone", "NOK" },
        { "omani rial", "OMR" },
        { "pakistani rupee", "PKR" },
        { "philippine peso", "PHP" },
        { "polish zloty", "PLN" },
        { "qatari riyal", "QAR" },
        { "romanian new leu", "RON" },
        { "russian ruble", "RUB" },
        { "saudi arabian riyal", "SAR" },
        { "singapore dollar", "SGD" },
        { "south african rand", "ZAR" },
        { "sri lankan rupee", "LKR" },
        { "swedish krona", "SEK" },
        { "swiss franc", "CHF" },
        { "taiwan new dollar", "TWD" },
        { "thai baht", "THB" },
        { "trinidadian dollar", "TTD" },
        { "turkish lira", "TRY" },
        { "emirati dirham", "AED" },
        { "british pound", "GBP" },
        { "venezuelan bolivar", "VES" }
    };

            return isoCodeMap.TryGetValue(currencyName.ToLower(), out var isoCode) ? isoCode : "UNKNOWN";
        }

        public async Task UpdateCoinTypesBasedOnCurrenciesAsync()
        {
            // Step 1: Fetch all currencies
            var currencies = await _dbContext.Currencies.ToListAsync();

            // Step 2: Fetch all coin types
            var coinTypes = await _dbContext.CoinTypes.ToListAsync();

            // Step 3: Create a dictionary for quick lookup of currencies by ISOCode
            var currencyByISOCode = currencies.ToDictionary(c => c.ISOCode.ToUpper(), c => c);

            // Step 4: Iterate through CoinTypes and update based on Currency table
            foreach (var coinType in coinTypes)
            {
                if (currencyByISOCode.TryGetValue(coinType.Symbol.ToUpper(), out var matchedCurrency))
                {
                    // Update Exchange Rate for matching CoinType
                    coinType.ExchangeRateToBaseCurrency = matchedCurrency.Rate;
                    coinType.IsActive = true; // Ensure active status if currency exists
                }
                else
                {
                    // Deactivate CoinType if no matching Currency or ISOCode is UNKNOWN
                    coinType.IsActive = false;
                }
            }

            // Step 5: Add new CoinTypes for any currencies that don't already exist in the CoinTypes table
            foreach (var currency in currencies)
            {
                if (currency.ISOCode == "UNKNOWN") continue; // Skip unknown ISO codes

                if (!coinTypes.Any(ct => ct.Symbol.ToUpper() == currency.ISOCode.ToUpper()))
                {
                    _dbContext.CoinTypes.Add(new CoinType
                    {
                        Name = currency.Name,
                        Symbol = currency.ISOCode.ToUpper(),
                        ExchangeRateToBaseCurrency = currency.Rate,
                        IsActive = true
                    });
                }
            }

            // Step 6: Save changes to the database
            await _dbContext.SaveChangesAsync();
        }


    }
}
