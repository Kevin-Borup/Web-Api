

namespace WebApplication_SpaceTravel.Exceptions
{
    internal class TitleException : Exception
    {
        public string GivenTitle { get; private set; }

        public TitleException(string givenTitle)
        {
               this.GivenTitle = givenTitle;
        }
    }
}
