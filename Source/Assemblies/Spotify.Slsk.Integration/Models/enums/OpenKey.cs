using System;
namespace Spotify.Slsk.Integration.Models.enums
{
    public class OpenKey
    {
        private const string _01d_key = "01d";
        private const string _01m_key = "01m";
        private const string _02d_key = "02d";
        private const string _02m_key = "02m";
        private const string _03d_key = "03d";
        private const string _03m_key = "03m";
        private const string _04d_key = "04d";
        private const string _04m_key = "04m";
        private const string _05d_key = "05d";
        private const string _05m_key = "05m";
        private const string _06d_key = "06d";
        private const string _06m_key = "06m";
        private const string _07d_key = "07d";
        private const string _07m_key = "07m";
        private const string _08d_key = "08d";
        private const string _08m_key = "08m";
        private const string _09d_key = "09d";
        private const string _09m_key = "09m";
        private const string _10d_key = "10d";
        private const string _10m_key = "10m";
        private const string _11d_key = "11d";
        private const string _11m_key = "11m";
        private const string _12d_key = "12d";
        private const string _12m_key = "12m";

        public static readonly OpenKey _01d = new OpenKey(_01d_key);
        public static readonly OpenKey _01m = new OpenKey(_01m_key);
        public static readonly OpenKey _02d = new OpenKey(_02d_key);
        public static readonly OpenKey _02m = new OpenKey(_02m_key);
        public static readonly OpenKey _03d = new OpenKey(_03d_key);
        public static readonly OpenKey _03m = new OpenKey(_03m_key);
        public static readonly OpenKey _04d = new OpenKey(_04d_key);
        public static readonly OpenKey _04m = new OpenKey(_04m_key);
        public static readonly OpenKey _05d = new OpenKey(_05d_key);
        public static readonly OpenKey _05m = new OpenKey(_05m_key);
        public static readonly OpenKey _06d = new OpenKey(_06d_key);
        public static readonly OpenKey _06m = new OpenKey(_06m_key);
        public static readonly OpenKey _07d = new OpenKey(_07d_key);
        public static readonly OpenKey _07m = new OpenKey(_07m_key);
        public static readonly OpenKey _08d = new OpenKey(_08d_key);
        public static readonly OpenKey _08m = new OpenKey(_08m_key);
        public static readonly OpenKey _09d = new OpenKey(_09d_key);
        public static readonly OpenKey _09m = new OpenKey(_09m_key);
        public static readonly OpenKey _10d = new OpenKey(_10d_key);
        public static readonly OpenKey _10m = new OpenKey(_10m_key);
        public static readonly OpenKey _11d = new OpenKey(_11d_key);
        public static readonly OpenKey _11m = new OpenKey(_11m_key);
        public static readonly OpenKey _12d = new OpenKey(_12d_key);
        public static readonly OpenKey _12m = new OpenKey(_12m_key);

        public string Value { get; private set; }

        private OpenKey(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }

        public override bool Equals(object? obj)
        {
            if (obj is OpenKey other)
            {
                return Value.Equals(other.Value);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static implicit operator string(OpenKey keyFormat)
        {
            return keyFormat.Value;
        }

        public static OpenKey? From(string openKeyString)
        {
            return (openKeyString.ToLower()) switch
            {
                _01d_key => OpenKey._01d,
                _01m_key => OpenKey._01m,
                _02d_key => OpenKey._02d,
                _02m_key => OpenKey._02m,
                _03d_key => OpenKey._03d,
                _03m_key => OpenKey._03m,
                _04d_key => OpenKey._04d,
                _04m_key => OpenKey._04m,
                _05d_key => OpenKey._05d,
                _05m_key => OpenKey._05m,
                _06d_key => OpenKey._06d,
                _06m_key => OpenKey._06m,
                _07d_key => OpenKey._07d,
                _07m_key => OpenKey._07m,
                _08d_key => OpenKey._08d,
                _08m_key => OpenKey._08m,
                _09d_key => OpenKey._09d,
                _09m_key => OpenKey._09m,
                _10d_key => OpenKey._10d,
                _10m_key => OpenKey._10m,
                _11d_key => OpenKey._11d,
                _11m_key => OpenKey._11m,
                _12d_key => OpenKey._12d,
                _12m_key => OpenKey._12m,
                _ => null
            };
        }
    }
}

