using WebApplication_JokeMachine.Models;

namespace WebApplication_JokeMachine.DataHandlers
{
    public class LocalDataHandler
    {
        List<Joke> jokesDA  = new List<Joke>()
        {
            new Joke(1, "Far", "Hvilken slags slik kan bilister ikke lide?", "Lak-riser"),
            new Joke(2, "Far", "Hvad sagde den ene cykel til den anden?", "Styr dig!"),
            new Joke(3, "Far", "Min datter skreg “Faar hører du overhovedet efter?!!!?!”", "Det en mærkelig måde, at starte en samtale på..."),
            new Joke(4, "Far", "Hvad hedder Draculas fætter, som i øvrigt er vegetar?", "Rucola"),
            new Joke(5, "Platte", "En dværg kommer ind på et værtshus\r\nen af gæsterne spørger ham:\r\n“Spiller du kort?”", "“Nej, jeg er født sådan.”"),
            new Joke(6, "Platte", "Hvad hedder den fattigste konge i verden?", "Kong Kurs"),
            new Joke(7, "Platte", "Hvor mange drenge er der i Danmark?", "2 – Dan og Mark"),
            new Joke(8, "Platte", "En politimand stopper en billist\r\n og siger “papir”", "Billisten siger “saks” og køre videre")
        };

        List<Joke> jokesEN = new List<Joke>()
        {
            new Joke(1, "Dad", "I'm afraid for the calendar", "Its days are numbered"),
            new Joke(2, "Dad", "I asked my dog what's two minus two.", "He said nothing."),
            new Joke(3, "Dad", "Where do fruits go on vacation?", "Pear-is!"),
            new Joke(4, "Dad", "I don't trust stairs", "They're always up to something."),
            new Joke(5, "Bad", "What did the the drummer call his twin daughters?", "Anna one, Anna two!"),
            new Joke(6, "Bad", "What does a nosey pepper do?", "It gets jalapeño business!"),
            new Joke(7, "Bad", "Does anyone need an ark?", "I Noah guy!"),
            new Joke(8, "Bad", "I bought a ceiling fan the other day.", "Complete waste of money. He just stands there applauding and saying “Ooh, I love how smooth it is.”")
        };

        public List<Joke> GetAllJokes(string language)
        {
            if (language.Equals("en")) return jokesEN;
            return jokesDA;
        }

        public List<Joke> GetJokesInCategory(string language, string category)
        {
            if (language.Equals("en")) return jokesEN.Where(j => j.Category == category).ToList();
            return jokesDA.Where(j => j.Category == category).ToList();
        }
    }
}
