namespace SecureMed.SharedKernel.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class SearchableHashAttribute(string targetProperty) : Attribute
{
    // Encryption generates unique ciphertexts for the same input each time,
    // so we need to store a hash of the searchable property to allow searching.
    public string TargetProperty { get; } = targetProperty;
}
