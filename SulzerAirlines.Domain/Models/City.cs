public sealed class City : IEquatable<City>
{
    public string Name { get; }

    public City(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("City name cannot be null or empty.", nameof(name));
        Name = name.Trim().ToUpperInvariant();
    }

    public override bool Equals(object? obj) => Equals(obj as City);

    public bool Equals(City? other) => other is not null && Name == other.Name;

    public override int GetHashCode() => Name.GetHashCode();

    public override string ToString() => Name;
}