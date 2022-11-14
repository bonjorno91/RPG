namespace Code.Selection.Behaviours
{
    public abstract class SelectableEntity<T> : SelectableEntityBase
    {
        protected T _selectable { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            _selectable = GetComponent<T>();
        }
    }
}