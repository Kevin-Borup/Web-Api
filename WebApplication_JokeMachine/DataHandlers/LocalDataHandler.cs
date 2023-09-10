using WebApplication_JokeMachine.Interfaces;
using WebApplication_JokeMachine.Models;

namespace WebApplication_JokeMachine.DataHandlers
{
    public class LocalDataHandler : IDataHandler
    {
        List<Joke> jokes  = new List<Joke>()
        {
            new Joke(1, "Basic", "da", "Far", "Hvilken slags slik kan bilister ikke lide?", "Lak-riser"),
            new Joke(2, "Basic", "da", "Far", "Hvad sagde den ene cykel til den anden?", "Styr dig!"),
            new Joke(3, "Premium", "da", "Far", "Min datter skreg “Faar hører du overhovedet efter?!!!?!”", "Det en mærkelig måde, at starte en samtale på..."),
            new Joke(4, "Premium", "da", "Far", "Hvad hedder Draculas fætter, som i øvrigt er vegetar?", "Rucola"),
            new Joke(5, "Basic", "da", "Platte", "En dværg kommer ind på et værtshus\r\nen af gæsterne spørger ham:\r\n“Spiller du kort?”", "“Nej, jeg er født sådan.”"),
            new Joke(6, "Basic", "da", "Platte", "Hvad hedder den fattigste konge i verden?", "Kong Kurs"),
            new Joke(7, "Premium", "da", "Platte", "Hvor mange drenge er der i Danmark?", "2 – Dan og Mark"),
            new Joke(8, "Premium", "da", "Platte", "En politimand stopper en billist\r\n og siger “papir”", "Billisten siger “saks” og køre videre"),
            new Joke(9, "Basic", "en", "Dad", "I'm afraid for the calendar", "Its days are numbered"),
            new Joke(10, "Basic", "en", "Dad", "I asked my dog what's two minus two.", "He said nothing."),
            new Joke(11, "Premium", "en", "Dad", "Where do fruits go on vacation?", "Pear-is!"),
            new Joke(12, "Premium", "en", "Dad", "I don't trust stairs", "They're always up to something."),
            new Joke(13, "Basic", "en", "Bad", "What did the the drummer call his twin daughters?", "Anna one, Anna two!"),
            new Joke(14, "Basic", "en", "Bad", "What does a nosey pepper do?", "It gets jalapeño business!"),
            new Joke(15, "Premium", "en", "Bad", "Does anyone need an ark?", "I Noah guy!"),
            new Joke(16, "Premium", "en", "Bad", "I bought a ceiling fan the other day.", "Complete waste of money. He just stands there applauding and saying “Ooh, I love how smooth it is.”")
        };

        List<ApiKey> apiKeys = new List<ApiKey>();

        public List<Joke> GetJokes(string access, string language = "da", string category = "")
        {
            List<Joke> filteredJokes = jokes.Where(j => j.AccessLevel.Equals(access) && j.Language.Equals(language)).ToList();

            if (!string.IsNullOrWhiteSpace(category)) return filteredJokes.Where(j => j.Category.Equals(category)).ToList();

            return filteredJokes;
        }

        public ApiKey? GetKeyIfIdentifierExists(string identifier)
        {
            if (CheckIdentifier(identifier)) return apiKeys.Where(k => k.Identifier.Equals(identifier)).First();
            return null;
        }

        public void InsertApiKey(ApiKey key)
        {
            apiKeys.Add(key);
        }

        private bool CheckIdentifier(string identifier)
        {
            return apiKeys.Any(k => k.Identifier.Equals(identifier));
        }
    }
}
