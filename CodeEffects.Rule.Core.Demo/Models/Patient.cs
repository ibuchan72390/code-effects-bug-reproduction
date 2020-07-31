using System;
using System.Linq;
using CodeEffects.Rule.Attributes;
using CodeEffects.Rule.Common;
using CodeEffects.Rule.Core.Demo.Services;

namespace CodeEffects.Rule.Core.Demo.Models
{
	// External methods and actions
	[ExternalMethod(typeof(PatientService), "IsToday")]
	[ExternalAction(typeof(PatientService), "RequestInfo")]

	// Dynamic Menu Data Sources; details at http://rule.codeeffects.com/Doc/Business-Rules-Dynamic-Menu-Data-Sources
	[Data("Education", "getEducationTypes")] // client-side function declared in _Layout.cshtml file
	[Data("Physicians", typeof(Physician), "List")]
	public class Patient
	{
		// C-tor
		public Patient()
		{
			this.ID = Guid.Empty;
			this.Gender = Gender.Unknown;
			this.Alcohol = this.Allergies = this.Headaches = this.Tobacco = false;
		}

		// This property will not appear in the Rule Editor - Code Effects component ignores Guids
		// Details at http://codeeffects.com/Doc/Business-Rules-Data-Types
		public Guid ID { get; set; }

		[Field(DisplayName = "First Name", Description = "Patient's first name", ValueInputType = ValueInputType.User, Max = 30)]
		public string FirstName { get; set; }

		[Field(DisplayName = "Last Name", Max = 30, ValueInputType = ValueInputType.User, Description = "Patient's last name")]
		public string LastName { get; set; }

		// The Email is declared as a field, not a property. This shows that any
		// public instance field is treated the same way as a property,
		// unless its decorated with ExcludeFromEvaluation attribute.
		[Field(DisplayName = "Email Address", Max = 150, Description = "Email address of the patient")]
		public string Email;

		[Field(DisplayName = "Date of Birth", DateTimeFormat = "MMM dd, yyyy")]
		public DateTime? DOB { get; set; }

		[Field(ValueInputType = ValueInputType.User, Description = "Patient's gender")]
		public Gender Gender { get; set; }

		// This field uses the "Physicians" dynamic menu source (declared at class level)
		[Field(DisplayName = "Physician", DataSourceName = "Physicians", ValueInputType = ValueInputType.All, Description = "Patient's primary physician")]
		public int PhysicianID { get; set; }

		// This field uses the "Education" client-side dynamic menu source (declared at class level)
		[Field(DisplayName = "Education", DataSourceName = "Education", ValueInputType = ValueInputType.Fields)]
		public int EducationTypeID { get; set; }

		[Field(Min = 0, Max = 200, Description = "Current pulse")]
		public int? Pulse { get; set; }

		[Field(Min = 0, Max = 200, DisplayName = "Systolic Pressure", Description = "Current systolic pressure")]
		public int? SystolicPressure { get; set; }

		[Field(Min = 0, Max = 200, DisplayName = "Diastolic Pressure", Description = "Current Diastolic pressure")]
		public int? DiastolicPressure { get; set; }

		[Field(Min = 0, Max = 110, Description = "Current temperature")]
		public decimal? Temperature { get; set; }

		[Field(DisplayName = "Headaches Box", Description = "Does the patient have frequent headaches?")]
		public bool Headaches { get; set; }

		[Field(DisplayName = "Allergies Box", Description = "Any allergies?")]
		public bool Allergies { get; set; }

		[Field(DisplayName = "Tobacco Box", Description = "Does the patient smoke?")]
		public bool Tobacco { get; set; }

		[Field(DisplayName = "Alcohol Box", Description = "Alcohol use?")]
		public bool Alcohol { get; set; }

		public Address Home { get; set; }
		public Address Work { get; set; }

		// This property is used to display the output of rule actions
		[ExcludeFromEvaluation]
		public string Output { get; set; }

		[Method("Full Name", "Joins together patient's first and last names")]
		public string FullName()
		{
			return string.Format("{0} {1}", this.FirstName, this.LastName);
		}

		// Empty overload of the Register method.
		[Action(Description = " Registers new patient")]
		public void Register()
		{
			this.Output += " The patient has been registered";
		}

		// This is the overload of the Register method that takes one param.
		// Both overloads can be used in Code Effects as two different actions
		// as long as their display names are different.
		[Action("Register with a Message", "Registers new patient with additional info")]
		public void Register([Parameter(ValueInputType.User, Description = "Output message")] string message)
		{
			this.Output += " " + message;
		}

		[Method("Count Characters in FullName", "Counts the number of occurrences of the provided string in the person's full name.")]
		public int CountCharactersInFullName()
		{
			return FullName().ToArray().Length;
		}

		[Field(DisplayName = "Calculated Value", Description = "Custom calculated value.")]
		public int CalculatedValue { get; set; }
	}
}