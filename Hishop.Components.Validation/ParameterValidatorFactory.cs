using System;
using System.Reflection;

namespace Hishop.Components.Validation
{
	public static class ParameterValidatorFactory
	{
		public static Validator CreateValidator(ParameterInfo paramInfo)
		{
			MetadataValidatedParameterElement metadataValidatedParameterElement = new MetadataValidatedParameterElement();
			metadataValidatedParameterElement.UpdateFlyweight(paramInfo);
			CompositeValidatorBuilder compositeValidatorBuilder = new CompositeValidatorBuilder(metadataValidatedParameterElement);
			foreach (IValidatorDescriptor current in metadataValidatedParameterElement.GetValidatorDescriptors())
			{
				compositeValidatorBuilder.AddValueValidator(current.CreateValidator(paramInfo.ParameterType, null, null));
			}
			return compositeValidatorBuilder.GetValidator();
		}
	}
}
