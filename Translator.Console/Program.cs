using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.Translation;

// 1. Translacja mowy do tekstu
// 2. Przeczytanie przetłumaczonego tekstu w konkretnym jezyku
// 3. Zapetlenie rozmowy


var key = "fd78336afaa9413e99dad46634f1cf45";
var region = "northeurope";

LanguageSettings pl = new("pl", "pl-PL", "pl-PL-MarekNeural");
LanguageSettings es = new("es", "es-MX", "es-MX-JorgeNeural");

while (true)
{
    await Translate(pl.Locale, es);
    await Translate(es.Locale, pl);
}

async Task Translate(string sourceLocale, LanguageSettings targetLanguage)
{
    var translationConfig = SpeechTranslationConfig.FromSubscription(key, region);
    translationConfig.SpeechRecognitionLanguage = sourceLocale;
    translationConfig.AddTargetLanguage(targetLanguage.Language);

    using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();

    using var recognizer = new TranslationRecognizer(translationConfig, audioConfig);
    var result = await recognizer.RecognizeOnceAsync();

    Console.WriteLine($"Input: {result.Text}");
    var translatedResult = result.Translations[targetLanguage.Language];
    Console.WriteLine($"Output: {translatedResult}");

    var config = SpeechConfig.FromSubscription(key, region);
    config.SpeechRecognitionLanguage = targetLanguage.Locale;
    config.SpeechSynthesisVoiceName = targetLanguage.VoiceName;

    using var synthesizer = new SpeechSynthesizer(config);
    await synthesizer.SpeakTextAsync(translatedResult);
}

record LanguageSettings(string Language, string Locale, string VoiceName);