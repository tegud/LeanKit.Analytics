namespace LeanKit.ReleaseManager.Models
{
    public class ColourPalette : IColourPalette
    {
        private readonly string[] _colourRange;
        private int _index;

        public ColourPalette(string[] colourRange)
        {
            _colourRange = colourRange;
        }

        public string Next()
        {
            var colour = _colourRange[_index % _colourRange.Length];
            _index++;
            return colour;
        }
    }
}