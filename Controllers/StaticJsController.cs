using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AjaxLogIn.Models;

namespace AjaxLogIn.Controllers
{
    public class StaticJsController : Controller
    {
        private readonly JavaScriptSerializer m_JavaScriptSerializer = new JavaScriptSerializer();

        public JavaScriptResult Index()
        {
            var jsNamespace = "var ajaxlogin = this.ajaxlogin || {};" + Environment.NewLine;
            object routes = GetRoutes();
            object validation = GetAllValidationRules();
            var parts = new[] {
                Tuple.Create("Routes", routes),
                Tuple.Create("Validation", validation)
            };
            var lines = parts.Select(t => string.Format("ajaxlogin.{0} = {1};", t.Item1, m_JavaScriptSerializer.Serialize(t.Item2)));
            return JavaScript(jsNamespace + string.Join(Environment.NewLine, lines));
        }

        private Dictionary<string, string> GetRoutes()
        {
            var routes = new Dictionary<string, IEnumerable<string>> {
                    { "Account", new[] { "LogIn", "LogOut", "SignUp", "Details", }}}
                .SelectMany(p => p.Value.Select(a => Tuple.Create(p.Key, a)))
                .ToDictionary(
                    p => p.Item1 + p.Item2,
                    p => Url.Action(p.Item2, p.Item1));
            return routes;
        }

        private static Dictionary<string, object> GetAllValidationRules()
        {
            return new Dictionary<string, object>
                {
                    {"LogIn", GetValidationRules<LogInModel>()},
                    {"SignUp", GetValidationRules<SignUpModel>()},
                };
        }

        private static object GetValidationRules<T>()
        {
            var modelProperties = typeof(T)
                .GetProperties()
                .Select(p => p.Name);
            var propToRules = modelProperties.ToDictionary(p => p, GetPropertyValidationRules<T>);
            return new {
                Rules = propToRules.ToDictionary(p => p.Key, p => p.Value.SelectMany(ToJqueryValidationRule)
                    .ToDictionary(t => t.Item1, t => t.Item2)),
                Messages = propToRules.ToDictionary(p => p.Key, p => p.Value.ToDictionary(
                    ToJqueryValidationType, r => r.ErrorMessage))
            };
        }

        private static IEnumerable<ModelClientValidationRule> GetPropertyValidationRules<T>(string property)
        {
            return ModelValidatorProviders.Providers
                .GetValidators(ModelMetadata.FromStringExpression(property, new ViewDataDictionary<T>()), new ViewContext())
                .SelectMany(v => v.GetClientValidationRules());
        }

        /// <summary>
        /// Formats the <see cref="ModelClientValidationRule"/> so that it can be used by the jQuery validate plugin
        /// </summary>
        private static IEnumerable<Tuple<string, object>> ToJqueryValidationRule(ModelClientValidationRule rule)
        {
            if (rule.ValidationType == "length")
            {
                return rule.ValidationParameters.Select(p => Tuple.Create(p.Key + "length", p.Value));
            }
            if (rule.ValidationType == "required")
            {
                return new[] {Tuple.Create<string, object>("required", true)};
            }
            if (rule.ValidationType == "equalto")
            {
                return new[] {Tuple.Create<string, object>(ToJqueryValidationType(rule), ToJquerySelector(rule))};
            }
            if (rule.ValidationType == "regex")
            {
                return new[] {Tuple.Create("regex", rule.ValidationParameters["pattern"])};
            }
            throw new NotImplementedException("Client side validation of '" + rule.ValidationType + "' is not implemented.");
        }

        /// <summary>
        /// Turns the argument to <see cref="CompareAttribute"/> on <see cref="SignUpModel.ConfirmPassword"/>
        /// in to something that can be used by the jQuery validate plugin, (i.e. jQuery selector)
        /// </summary>
        private static string ToJquerySelector(ModelClientValidationRule rule)
        {
            // HACK: turn *.Password in to #Password
            return rule.ValidationParameters["other"].ToString().Replace("*.", "#");
        }

        /// <summary>
        /// Formats the <see cref="ModelClientValidationRule"/> so that it can be used as a validation
        /// type compatible with jQuery validate plugin
        /// </summary>
        private static string ToJqueryValidationType(ModelClientValidationRule rule)
        {
            var validationType = rule.ValidationType;
            if (validationType == "equalto")
                return "equalTo";
            if (validationType == "length")
                return "minlength";
            return validationType;
        }
    }
}
