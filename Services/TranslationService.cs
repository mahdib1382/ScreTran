using System.Net.Http;
using System.Web;
using GTranslate.Translators;
using Newtonsoft.Json.Linq;

namespace ScreTran;

public class TranslationService : ITranslationService
{
    private readonly YandexTranslator _yandexTranslator;
    private readonly BingTranslator _bingTranslator;

    public TranslationService()
    {
        _yandexTranslator = new YandexTranslator();
        _bingTranslator = new BingTranslator();
    }

    /// <summary>
    /// Translate input to target language.
    /// </summary>
    /// <param name="input">Input text.</param>
    /// <param name="translator">Translator type.</param>
    /// <param name="targetLanguage">Target language code.</param>
    /// <returns>Translated text.</returns>
    public string Translate(string input, Enumerations.Translator translator, string targetLanguage = "fa")
    {
        return Task.Run(async () => await TranslateAsync(input, translator, targetLanguage)).Result;
    }

    /// <summary>
    /// Translate input to target language asynchronously.
    /// </summary>
    /// <param name="input">Input text.</param>
    /// <param name="translator">Translator type.</param>
    /// <param name="targetLanguage">Target language code.</param>
    /// <returns>Translated text.</returns>
    private async Task<string> TranslateAsync(string input, Enumerations.Translator translator, string targetLanguage = "fa")
    {
        if (translator == Enumerations.Translator.Google)
            return await TranslateGoogleAsync(input, targetLanguage);
        if (translator == Enumerations.Translator.Yandex)
            return (await _yandexTranslator.TranslateAsync(input, targetLanguage)).Translation;
        if (translator == Enumerations.Translator.Bing)
            return (await _bingTranslator.TranslateAsync(input, targetLanguage)).Translation;

        return input;
    }

    /// <summary>
    /// Translate input to target language using GoogleTranslate asynchronously.
    /// </summary>
    /// <param name="input">Input text.</param>
    /// <param name="targetLanguage">Target language code.</param>
    /// <returns>Translated text.</returns>
    public async Task<string> TranslateGoogleAsync(string input, string targetLanguage = "fa")
    {
        var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl=auto&tl={targetLanguage}&dt=t&q={HttpUtility.UrlEncode(input)}";

        using var client = new HttpClient();
        var response = await client.GetStringAsync(url).ConfigureAwait(false);
        return string.Join(string.Empty, JArray.Parse(response)[0].Select(x => x[0]));
    }
}
