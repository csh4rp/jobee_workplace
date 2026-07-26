using System.Text.RegularExpressions;

namespace Jobee.Workplace.Shared.Application.Identity;

public partial record Permission
{
    private const string Wildcard = "*";
    private const string Separator = "/";
    private static readonly Regex ValidationRegex = GetValidationRegex();

    private readonly string _value;

    private Permission(string value) => _value = value;

    [GeneratedRegex(@"^(([a-zA-Z]+)|\*)(\/(([a-zA-Z]+)|\*))*$", RegexOptions.Compiled)]
    private static partial Regex GetValidationRegex();

    public bool GrantsAccessTo(string permission)
    {
        var permissionParts = permission.Split(Separator);
        var valueParts = _value.Split(Separator);

        var valuesIndex = 0;
        var permissionsIndex = 0;

        while (permissionsIndex < permissionParts.Length)
        {
            var currentPermission = permissionParts[permissionsIndex];
            var currentValue = valueParts[valuesIndex];

            // Current parts match
            if (currentPermission.Equals(currentValue, StringComparison.OrdinalIgnoreCase))
            {
                valuesIndex++;
                permissionsIndex++;
                continue;
            }

            if (currentValue.Equals(Wildcard))
            {
                // If wildcard is last part - it's a march
                if (valuesIndex == valueParts.Length - 1)
                {
                    return true;
                }

                valuesIndex++;
                currentValue = valueParts[valuesIndex];

                // Try to find next matching part
                while (permissionsIndex < permissionParts.Length)
                {
                    currentPermission = permissionParts[permissionsIndex];

                    // Next matching part found
                    if (currentValue.Equals(currentPermission, StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }

                    // No matching part found
                    if (permissionsIndex == permissionParts.Length - 1)
                    {
                        return false;
                    }

                    permissionsIndex++;
                }

                continue;
            }

            return false;
        }

        return true;
    }

    public static Permission Parse(string value)
    {
        if (!ValidationRegex.IsMatch(value))
        {
            throw new ArgumentException("Value does not match permission format");
        }

        return new Permission(value);
    }

    public override string ToString() => _value;
}