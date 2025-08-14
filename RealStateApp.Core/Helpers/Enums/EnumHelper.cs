using RealStateApp.Core.Domain.Base;

namespace RealStateApp.Core.Application.Helpers.Enums
{
    public static class EnumHelper
    {
        public static IEnumerable<Ident<int>> GetEnumsAsIdent<TEnum>() where TEnum : Enum
         => Enum.GetValues(typeof(TEnum))
            .Cast<TEnum>()
            .Select(en => new Ident<int>
            {
                Id = Convert.ToInt32(en),
                Name = en.ToString()
            });

        
        public static bool TryParseEnum<TEnum>(int value, out TEnum result) where TEnum : struct, Enum
        {
            if (Enum.IsDefined(typeof(TEnum), value))
            {
                result = (TEnum)(object)value;
                return true;
            }

            result = default;
            return false;
        }
    }
}
