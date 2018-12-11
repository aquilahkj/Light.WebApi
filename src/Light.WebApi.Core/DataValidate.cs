using System;
using System.Text.RegularExpressions;

namespace Light.WebApi.Core
{
    public static class DataValidate
    {
        public static void ValidateString(string value, string name, bool allowNull = false, int? minLen = null, int? maxLen = null, string regex = null)
        {
            if (value == null) {
                if (allowNull) {
                    return;
                }
                else {
                    throw new ValidateException(name, SR.NotAllowNull);
                }
            }
            if (minLen != null && value.Length < minLen.Value) {
                throw new ValidateException(name, SR.DataLengthError);
            }
            if (maxLen != null && value.Length > maxLen.Value) {
                throw new ValidateException(name, SR.DataLengthError);
            }
            if (regex != null && !Regex.IsMatch(value, regex)) {
                throw new ValidateException(name, SR.DataLengthError);
            }
        }

        public static void ValidateInt(int? value, string name, bool allowNull = false, int? min = null, int? max = null)
        {
            if (value == null) {
                if (allowNull) {
                    return;
                }
                else {
                    throw new ValidateException(name, SR.NotAllowNull);
                }
            }
            if (min != null && value.Value < min.Value) {
                throw new ValidateException(name, SR.DataRangeError);
            }
            if (max != null && value.Value > max.Value) {
                throw new ValidateException(name, SR.DataRangeError);
            }
        }

        public static void ValidateDecimal(decimal? value, string name, bool allowNull = false, decimal? min = null, decimal? max = null)
        {
            if (value == null) {
                if (allowNull) {
                    return;
                }
                else {
                    throw new ValidateException(name, SR.NotAllowNull);
                }
            }
            if (min != null && value.Value < min.Value) {
                throw new ValidateException(name, SR.DataRangeError);
            }
            if (max != null && value.Value > max.Value) {
                throw new ValidateException(name, SR.DataRangeError);
            }
        }

        public static void ValidateDate(int? value, string name, bool allowNull = false, int? min = null, int? max = null)
        {
            if (value == null) {
                if (allowNull) {
                    return;
                }
                else {
                    throw new ValidateException(name, SR.NotAllowNull);
                }
            }
            if (min != null && value.Value < min.Value) {
                throw new ValidateException(name, SR.DataRangeError);
            }
            if (max != null && value.Value > max.Value) {
                throw new ValidateException(name, SR.DataRangeError);
            }
        }

        public static void ValidateArray(Array value, string name, bool allowNull = false, int? minLen = null, int? maxLen = null)
        {
            if (value == null) {
                if (allowNull) {
                    return;
                }
                else {
                    throw new ValidateException(name, SR.NotAllowNull);
                }
            }
            if (minLen != null && value.Length < minLen.Value) {
                throw new ValidateException(name, SR.DataLengthError);
            }
            if (maxLen != null && value.Length > maxLen.Value) {
                throw new ValidateException(name, SR.DataLengthError);
            }
        }
    }
}
