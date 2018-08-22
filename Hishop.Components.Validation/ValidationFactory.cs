using Hishop.Components.Validation.Validators;
using System;
using System.Collections.Generic;

namespace Hishop.Components.Validation
{
	public static class ValidationFactory
	{
		private struct ValidatorCacheKey : IEquatable<ValidationFactory.ValidatorCacheKey>
		{
			private Type sourceType;

			private string ruleset;

			private bool generic;

			public ValidatorCacheKey(Type sourceType, string ruleset, bool generic)
			{
				this.sourceType = sourceType;
				this.ruleset = ruleset;
				this.generic = generic;
			}

			public override int GetHashCode()
			{
				return this.sourceType.GetHashCode() ^ ((this.ruleset != null) ? this.ruleset.GetHashCode() : 0);
			}

			bool IEquatable<ValidationFactory.ValidatorCacheKey>.Equals(ValidationFactory.ValidatorCacheKey other)
			{
				return this.sourceType == other.sourceType && ((this.ruleset == null) ? (other.ruleset == null) : this.ruleset.Equals(other.ruleset)) && this.generic == other.generic;
			}
		}

		private static IDictionary<ValidationFactory.ValidatorCacheKey, Validator> attributeOnlyValidatorsCache = new Dictionary<ValidationFactory.ValidatorCacheKey, Validator>();

		private static object attributeOnlyValidatorsCacheLock = new object();

		private static IDictionary<ValidationFactory.ValidatorCacheKey, Validator> attributeAndDefaultConfigurationValidatorsCache = new Dictionary<ValidationFactory.ValidatorCacheKey, Validator>();

		private static object attributeAndDefaultConfigurationValidatorsCacheLock = new object();

		private static IDictionary<ValidationFactory.ValidatorCacheKey, Validator> defaultConfigurationOnlyValidatorsCache = new Dictionary<ValidationFactory.ValidatorCacheKey, Validator>();

		private static object defaultConfigurationOnlyValidatorsCacheLock = new object();

		public static void ResetCaches()
		{
			lock (ValidationFactory.attributeOnlyValidatorsCacheLock)
			{
				ValidationFactory.attributeOnlyValidatorsCache.Clear();
			}
			lock (ValidationFactory.attributeAndDefaultConfigurationValidatorsCacheLock)
			{
				ValidationFactory.attributeAndDefaultConfigurationValidatorsCache.Clear();
			}
			lock (ValidationFactory.defaultConfigurationOnlyValidatorsCacheLock)
			{
				ValidationFactory.defaultConfigurationOnlyValidatorsCache.Clear();
			}
		}

		public static Validator<T> CreateValidator<T>()
		{
			return ValidationFactory.CreateValidator<T>(string.Empty, true);
		}

		public static Validator<T> CreateValidator<T>(string ruleset)
		{
			return ValidationFactory.CreateValidator<T>(ruleset, true);
		}

		private static Validator<T> CreateValidator<T>(string ruleset, bool cacheValidator)
		{
			Validator<T> validator = null;
			if (cacheValidator)
			{
				lock (ValidationFactory.attributeAndDefaultConfigurationValidatorsCacheLock)
				{
					ValidationFactory.ValidatorCacheKey key = new ValidationFactory.ValidatorCacheKey(typeof(T), ruleset, true);
					Validator validator2;
					if (ValidationFactory.attributeAndDefaultConfigurationValidatorsCache.TryGetValue(key, out validator2))
					{
						return (Validator<T>)validator2;
					}
					Validator validator3 = ValidationFactory.InnerCreateValidatorFromAttributes(typeof(T), ruleset);
					validator = ValidationFactory.WrapAndInstrumentValidator<T>(validator3);
					ValidationFactory.attributeAndDefaultConfigurationValidatorsCache[key] = validator;
					return validator;
				}
			}
			Validator validator4 = ValidationFactory.InnerCreateValidatorFromAttributes(typeof(T), ruleset);
			validator = ValidationFactory.WrapAndInstrumentValidator<T>(validator4);
			return validator;
		}

		public static Validator CreateValidator(Type targetType)
		{
			return ValidationFactory.CreateValidator(targetType, string.Empty);
		}

		public static Validator CreateValidator(Type targetType, string ruleset)
		{
			return ValidationFactory.CreateValidator(targetType, ruleset, true);
		}

		private static Validator CreateValidator(Type targetType, string ruleset, bool cacheValidator)
		{
			Validator validator = null;
			if (cacheValidator)
			{
				lock (ValidationFactory.attributeAndDefaultConfigurationValidatorsCacheLock)
				{
					ValidationFactory.ValidatorCacheKey key = new ValidationFactory.ValidatorCacheKey(targetType, ruleset, false);
					Validator result;
					if (ValidationFactory.attributeAndDefaultConfigurationValidatorsCache.TryGetValue(key, out result))
					{
						return result;
					}
					Validator validator2 = ValidationFactory.InnerCreateValidatorFromAttributes(targetType, ruleset);
					validator = ValidationFactory.WrapAndInstrumentValidator(validator2);
					ValidationFactory.attributeAndDefaultConfigurationValidatorsCache[key] = validator;
					return validator;
				}
			}
			Validator validator3 = ValidationFactory.InnerCreateValidatorFromAttributes(targetType, ruleset);
			validator = ValidationFactory.WrapAndInstrumentValidator(validator3);
			return validator;
		}

		public static Validator<T> CreateValidatorFromAttributes<T>()
		{
			return ValidationFactory.CreateValidatorFromAttributes<T>(string.Empty);
		}

		public static Validator<T> CreateValidatorFromAttributes<T>(string ruleset)
		{
			if (ruleset == null)
			{
				throw new ArgumentNullException("ruleset");
			}
			Validator<T> validator = null;
			lock (ValidationFactory.attributeOnlyValidatorsCacheLock)
			{
				ValidationFactory.ValidatorCacheKey key = new ValidationFactory.ValidatorCacheKey(typeof(T), ruleset, true);
				Validator validator2;
				if (ValidationFactory.attributeOnlyValidatorsCache.TryGetValue(key, out validator2))
				{
					return (Validator<T>)validator2;
				}
				Validator validator3 = ValidationFactory.InnerCreateValidatorFromAttributes(typeof(T), ruleset);
				validator = ValidationFactory.WrapAndInstrumentValidator<T>(validator3);
				ValidationFactory.attributeOnlyValidatorsCache[key] = validator;
			}
			return validator;
		}

		public static Validator CreateValidatorFromAttributes(Type targetType, string ruleset)
		{
			if (ruleset == null)
			{
				throw new ArgumentNullException("ruleset");
			}
			Validator validator = null;
			lock (ValidationFactory.attributeOnlyValidatorsCacheLock)
			{
				ValidationFactory.ValidatorCacheKey key = new ValidationFactory.ValidatorCacheKey(targetType, ruleset, false);
				Validator result;
				if (ValidationFactory.attributeOnlyValidatorsCache.TryGetValue(key, out result))
				{
					return result;
				}
				Validator validator2 = ValidationFactory.InnerCreateValidatorFromAttributes(targetType, ruleset);
				validator = ValidationFactory.WrapAndInstrumentValidator(validator2);
				ValidationFactory.attributeOnlyValidatorsCache[key] = validator;
			}
			return validator;
		}

		private static Validator InnerCreateValidatorFromAttributes(Type targetType, string ruleset)
		{
			MetadataValidatorBuilder metadataValidatorBuilder = new MetadataValidatorBuilder();
			return metadataValidatorBuilder.CreateValidator(targetType, ruleset);
		}

		private static Validator<T> WrapAndInstrumentValidator<T>(Validator validator)
		{
			return new GenericValidatorWrapper<T>(validator);
		}

		private static Validator WrapAndInstrumentValidator(Validator validator)
		{
			return new ValidatorWrapper(validator);
		}
	}
}
