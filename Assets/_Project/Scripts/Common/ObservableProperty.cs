using System;

public class ObservableProperty<T>
{
    private T _value;
    public event Action<T> OnValueChanged;

    public T Value
    {
        get => _value;
        set
        {
            if (!_value?.Equals(value) ?? value != null)
            {
                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }
    }

    public ObservableProperty(T initialValue = default)
    {
        _value = initialValue;
    }
}
