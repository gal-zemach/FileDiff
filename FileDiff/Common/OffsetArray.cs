namespace FileDiff.Common;

/// <summary>
/// An array that can be accessed using offset indexing
/// </summary>
public class OffsetArray<T>
{
    private readonly T[] _data;
    private readonly int _offset;

    public OffsetArray(int size, int offset)
    {
        _data = new T[size];
        _offset = offset;
    }

    /// <summary>
    /// Retrieves the offset index
    /// </summary>
    public T this[int index]
    {
        get => _data[_offset + index];
        set => _data[_offset + index] = value;
    }
    
    /// <inheritdoc cref="Array.Length"/>
    public int Length => _data.Length;

    /// <summary>
    /// Returns a shallow copy of the array
    /// </summary>
    public OffsetArray<T> Clone()
    {
        var copy = new OffsetArray<T>(_data.Length, _offset);
        Array.Copy(this._data, copy._data, this.Length);
        return copy;
    }
}
