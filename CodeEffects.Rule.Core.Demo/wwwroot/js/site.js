
var codeeffects;

$(function ()
{
	post("/LoadSettings", null, function (data)
	{
		// Init the editor and store its reference in a global variable
		codeeffects = $rule.init(data.editorData);

		// Make sure to remove any markup from the editor before initing it
		codeeffects.clear();

		// Set action delegates that Code Effects editor calls when the rule is being saved, loaded or deleted
		// These functions are declared later in this script
		codeeffects.setClientActions(loadRule, deleteRule, saveRule);

		// Load the source settings
		codeeffects.loadSettings(data.sourceData);

		// Use the evaluateRule method as Test button's click event handler
		$("#Button").click(evaluateRule);
	});
});

// Client action method that loads rule from the storage.
function loadRule(ruleId)
{
	// Call the LoadRule MVC action declared in HomeController
	post("/LoadRule", JSON.stringify({ Data: ruleId }), ruleLoaded);
};

// Handler of the successful loadRule() call
function ruleLoaded(ruleData)
{
	// Load the rule into the Rule Area by passing json to the editor
	codeeffects.loadRule(ruleData);

	// Notify user that the rule was loaded
	$("#Info").text("Rule is loaded");
};

// Client action method that deletes the current rule from the storage.
function deleteRule(ruleId)
{
	// Call the DeleteRule MVC action declared in HomeController
	post("/DeleteRule", JSON.stringify({ Data: ruleId }), ruleDeleted);
};

// Handler of the successful deleteRule() call
function ruleDeleted()
{
	// Let the editor know that there were no errors and
	// the rule of codeeffects.getRuleId() ID was deleted successfully
	codeeffects.deleted(codeeffects.getRuleId());

	// Clear the Rule Area (we just deleted the rule,
	// there is no point of displaying it anymore)
	codeeffects.clear();

	// Notify user that the rule was deleted
	$("#Info").text("Rule was deleted successfully");
};

// Client action method that saves the new or existing rule in the storage.
function saveRule(ruleData)
{
	post("/SaveRule", JSON.stringify({ Data: ruleData }), ruleSaved);
};

// Handler of the successful saveRule() call
function ruleSaved(data)
{
	if (data.isRuleEmpty) $("#Info").text("The rule is empty"); // The Rule Area is empty
	else if (!data.isRuleValid)
	{
		// The rule is invalid. Pass the received invalid client data to the editor
		codeeffects.loadInvalids(data.clientInvalidData);

		// Notify user
		$("#Info").text("The rule is not valid");
	}
	else
	{
		// Server returns rule ID using the Output property of ProcessingResult C# type.
		// The editor needs this ID if the saved rule was a new rule. In any case, pass it to the editor
		codeeffects.saved(data.output);

		// Inform user
		$("#Info").text("The rule was saved successfully");
	}
};

// Function that handles the evaluation of the current rule
function evaluateRule()
{
	$("#Info").text("Evaluating (6 second trial delay)...");
	// The codeeffects.extract() method returns rule's data
	post("/EvaluateRule", JSON.stringify({ Data: codeeffects.extract() }), ruleEvaluated);
};

// Handler of the successful evaluateRule() call
function ruleEvaluated(data)
{
	if (data.isRuleEmpty) $("#Info").text("The rule is empty"); // The Rule Area was empty
	else if (!data.isRuleValid)
	{
		// The rule is not valid, pass invalid data to the editor
		codeeffects.loadInvalids(data.clientInvalidData);

		// Notify user
		$("#Info").text("The rule is not valid");
	}
	else
	{
		// Display the evaluation output returned by the server
		$("#Info").text(data.output);
	}
};

// Utility wrapper that handles HTTP posts to the server.
// jQuery is used here as an example; obviously, you can use
// any framework that wraps calls to xmlHttpRequest
function post(url, data, delegate)
{
	$.ajax({
		type: "POST",
		url: "/Home" + url,
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		data: data,
		cache: false,
		success: delegate,
		error: error
	});
};

// Error handler
function error(e)
{
	if (!e) throw Eror("Generic server error.");
	else
	{
		if (e.statusText) $("#Info").text(e.statusText);
		else throw Error(e);
	}
};