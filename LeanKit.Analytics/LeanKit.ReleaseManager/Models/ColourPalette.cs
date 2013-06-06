namespace LeanKit.ReleaseManager.Models
{
    public class ColourPalette : IRotateThroughASetOfColours
    {
        private readonly string[] _colourRange;
        private int _index;

        public ColourPalette(string[] colourRange)
        {
            _colourRange = colourRange;
        }

        public string Next()
        {
            var colour = _colourRange[_index++ % _colourRange.Length];
            return colour;
        }
    }
}