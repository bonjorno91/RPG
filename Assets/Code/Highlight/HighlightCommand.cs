using Code.Abstract;

namespace Code.Highlight
{
    public class HighlightCommand : ICommand
    {
        public IHighlightable Highlightable { get; }
        
        public HighlightCommand(IHighlightable highlightable) => Highlightable = highlightable;

        public void Execute()
        {
            
        }
    }
}