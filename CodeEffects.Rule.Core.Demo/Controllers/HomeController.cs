using System;
using System.Diagnostics;
using CodeEffects.Rule.Core.Demo.Models;
using CodeEffects.Rule.Core.Demo.Services;
using CodeEffects.Rule.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace CodeEffects.Rule.Core.Demo.Controllers
{
	public class HomeController : Controller
    {
		private IHostingEnvironment environment;

		public HomeController(IHostingEnvironment environment)
		{
			this.environment = environment;
		}

		public IActionResult Index()
        {
			return View();
        }

		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpPost]
		public IActionResult LoadSettings()
		{
			ClientSettings s = new ClientSettings();

			RuleEditor editor = this.GetRuleEditor();

			s.EditorData = editor.GetInitialSettings();
			s.SourceData = editor.GetClientSettings();

			// Send the settings back to the client
			return Json(s);
		}

		[HttpPost]
		public IActionResult LoadRule([FromBody] ClientRequest data)
		{
			// Load the rule from the storage file by its ID
			string ruleXml = new StorageService(this.environment).LoadRuleXml(data.Data);

			// See the comments in the LoadSettings() method
			RuleEditor editor = this.GetRuleEditor(ruleXml);

			// Get the rule's client data
			string ruleJson = editor.GetClientRuleData();

			// Send it back to the server
			return Json(ruleJson);
		}

		[HttpPost]
		public IActionResult DeleteRule([FromBody] ClientRequest data)
		{
			try
			{
				// Delete the rule from the storage file by its ID
				new StorageService(this.environment).DeleteRule(data.Data);
			}
			catch (Exception ex)
			{
				return Json(ex.Message);
			}

			// Respond to the client's request with no data
			return Json(null);

		}

		[HttpPost]
		public ActionResult SaveRule([FromBody] ClientRequest data)
		{
			Result result = new Result();

			// See the comments in the LoadSettings() method
			RuleEditor editor = this.GetRuleEditor();

			// Load the rule into the editor
			editor.LoadClientData(data.Data);

			if (editor.Rule.IsEmpty())
			{
				result.IsRuleEmpty = true;
			}
			else if (!editor.Rule.IsValid(new StorageService(this.environment).LoadRuleXml))
			{
				result.IsRuleValid = false;
				// Load the json string of invalid data into the Result object
				result.ClientInvalidData = editor.GetClientInvalidData();
			}
			else
			{
				// Save the rule
				new StorageService(this.environment)
					.SaveRule(editor.Rule.Id.ToString(), editor.Rule.GetRuleXml(), editor.Rule.IsLoadedRuleOfEvalType == null ? true : (bool)editor.Rule.IsLoadedRuleOfEvalType);
				// Send ID of this rule to the client
				result.Output = editor.Rule.Id.ToString();
			}

			return Json(result);
		}

		[HttpPost]
		public JsonResult EvaluateRule([FromBody] ClientRequest data)
		{
			Result result = new Result();

			// See the comments in the LoadSettings() method
			RuleEditor editor = this.GetRuleEditor();

			// Load the rule into the editor
			editor.LoadClientData(data.Data);

			// We are not saving the rule, just evaluating it. Tell the editor not to enforce the rule name validation
			editor.Rule.SkipNameValidation = true;

			StorageService storage = new StorageService(this.environment);

			if (editor.Rule.IsEmpty())
			{
				result.IsRuleEmpty = true;
			}
			else if (!editor.Rule.IsValid(storage.LoadRuleXml))
			{
				result.IsRuleValid = false;
				// Load the json string of invalid data into the Result object
				result.ClientInvalidData = editor.GetClientInvalidData();
			}
			else
			{
				// Create an instance of the Evaluator class. Because our rules might reference other rules of evaluation type
				// we use constructor that takes rule's XML and delegate of the method that can load referenced rules by their IDs.
				Evaluator<Patient> evaluator = new Evaluator<Patient>(editor.Rule.GetRuleXml(), storage.LoadRuleXml);

				Patient patient = GetSource();

				// Evaluate the patient against the rule
				bool success = evaluator.Evaluate(patient);

				// Return the evaluated patient back to the client
				result.Patient = patient;

				// Output the result of the evaluation to the client
				result.Output = string.IsNullOrWhiteSpace(patient.Output) ? "The rule evaluated to " + success.ToString() : patient.Output;
			}

			return Json(result);
		}

		private Patient GetSource()
		{
			return new Patient
			{
				Alcohol = true,
				Allergies = false,
				DiastolicPressure = 120,
				DOB = new DateTime(1995, 5, 17),
				EducationTypeID = 1, // See the client function getEducationTypes() declared in /Views/Shared/_Layout/cshtml for ID values
				Email = "test@test.com",
				FirstName = "John",
				Gender = Gender.Male,
				Headaches = true,
				Home = new Address { City = "Alpharetta", State = State.Georgia, Street = "123 Main Street", Zip = "30005" },
				ID = Guid.NewGuid(),
				LastName = "Smith",
				PhysicianID = 4, // See the Models.Physician.List() method for values
				Pulse = 77,
				SystolicPressure = 80,
				Temperature = 96,
				Tobacco = false,
				Work = new Address { City = "Roswell", State = State.Arizona, Street = "51 Area Drive", Zip = "12345" }
			};
		}

		private RuleEditor GetRuleEditor()
		{
			return this.GetRuleEditor(null);
		}

		private RuleEditor GetRuleEditor(string ruleXml)
		{
			// Pass ID of the DIV elemnt that hosts the editor
			RuleEditor editor = new RuleEditor("divRuleEditor")
			{
				Mode = Common.RuleType.Execution,
				SourceType = typeof(Patient)
			};

			if (ruleXml != null) editor.Rule = Rule.Models.RuleModel.Create(ruleXml, typeof(Models.Patient));

			//if (ruleXml == null) editor.Rule = Rule.Models.RuleModel.Create(typeof(Models.Patient));
			//else editor.Rule = Rule.Models.RuleModel.Create(ruleXml, typeof(Models.Patient));

			StorageService storage = new StorageService(this.environment);

			// These are the rules displayed in the Rules menu of the ToolBar
			editor.ToolBarRules = storage.GetAllRules();
			// These are reusable evaluation rules displayed in field menus; see documentation details on reusable rules
			editor.ContextMenuRules = storage.GetEvaluationRules();

			return editor;
		}
	}
}