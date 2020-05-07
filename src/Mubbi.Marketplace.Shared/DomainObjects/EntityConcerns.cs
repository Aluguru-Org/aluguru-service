using System;
using System.Collections.Generic;
using System.Text;

namespace Mubbi.Marketplace.Shared.DomainObjects
{
    public static class EntityConcerns
    {
        public static void IsEmpty(string text, string message)
        {
            if (string.IsNullOrEmpty(text)) throw new DomainException(message);
        }

        public static void IsEmpty<T>(ICollection<T> collection, string message) where T : class
        {
            if (collection.Count == 0) throw new DomainException(message);
        }

        public static void IsNull(object a, string message)
        {
            if (a == null) throw new DomainException(message);
        }

        public static void IsTrue(bool bollean, string message)
        {
            if (bollean) throw new DomainException(message);
        }

        public static void IsFalse(bool bollean, string message)
        {
            if (!bollean) throw new DomainException(message);
        }

        public static void IsEqual(object a, object b, string message)
        {
            if (a.Equals(b)) throw new DomainException(message);
        }

        public static void IsNotEqual(object a, object b, string message)
        {
            if (!a.Equals(b)) throw new DomainException(message);
        }

        public static void SmallerThan(TimeSpan minimum, TimeSpan value, string message)
        {
            if (value < minimum) throw new DomainException(message);
        }
        public static void SmallerThan(decimal minimum, decimal value, string message)
        {
            if (value < minimum) throw new DomainException(message);
        }

        public static void SmallerThan(double minimum, double value, string message)
        {
            if (value < minimum) throw new DomainException(message);
        }

        public static void SmallerThan(float minimum, float value, string message)
        {
            if (value < minimum) throw new DomainException(message);
        }

        public static void SmallerThan(int minimum, int value, string message)
        {
            if (value < minimum) throw new DomainException(message);
        }

        public static void SmallerOrEqualThan(TimeSpan minimum, TimeSpan value, string message)
        {
            if (value <= minimum) throw new DomainException(message);
        }

        public static void SmallerOrEqualThan(decimal minimum, decimal value, string message)
        {
            if (value <= minimum) throw new DomainException(message);
        }

        public static void SmallerOrEqualThan(double minimum, double value, string message)
        {
            if (value <= minimum) throw new DomainException(message);
        }

        public static void SmallerOrEqualThan(float minimum, float value, string message)
        {
            if (value <= minimum) throw new DomainException(message);
        }

        public static void SmallerOrEqualThan(int minimum, int value, string message)
        {
            if (value <= minimum) throw new DomainException(message);
        }

        public static void GreaterThan(TimeSpan maximum, TimeSpan value, string message)
        {
            if (value > maximum) throw new DomainException(message);
        }

        public static void GreaterThan(decimal maximum, decimal value, string message)
        {
            if (value > maximum) throw new DomainException(message);
        }

        public static void GreaterThan(double maximum, double value, string message)
        {
            if (value > maximum) throw new DomainException(message);
        }

        public static void GreaterThan(float maximum, float value, string message)
        {
            if (value > maximum) throw new DomainException(message);
        }

        public static void GreaterThan(int maximum, int value, string message)
        {
            if (value > maximum) throw new DomainException(message);
        }

        public static void GreaterOrEqualThan(TimeSpan maximum, TimeSpan value, string message)
        {
            if (value >= maximum) throw new DomainException(message);
        }

        public static void GreaterOrEqualThan(decimal maximum, decimal value, string message)
        {
            if (value >= maximum) throw new DomainException(message);
        }

        public static void GreaterOrEqualThan(double maximum, double value, string message)
        {
            if (value >= maximum) throw new DomainException(message);
        }

        public static void GreaterOrEqualThan(float maximum, float value, string message)
        {
            if (value >= maximum) throw new DomainException(message);
        }

        public static void GreaterOrEqualThan(int maximum, int value, string message)
        {
            if (value >= maximum) throw new DomainException(message);
        }

    }

}
