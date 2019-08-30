
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using System.Linq;

namespace setrunner.Attributes 
{
    public enum MatchMode : int
    {
        All,
        Any,
        None
    }

    /// <summary>
    /// Flags an Action Method valid for any incoming request only if all, any or none of the given HTTP parameter(s) are set,
    /// enabling the use of multiple Action Methods with the same name (and different signatures) within the same MVC Controller.
    /// Source (before modifications) : https://www.ryadel.com/en/asp-net-mvc-fix-ambiguous-action-methods-errors-multiple-action-methods-action-name-c-sharp-core/
    /// </summary>
    public class RequireParameterAttribute : ActionMethodSelectorAttribute
    {
        public RequireParameterAttribute(string parameterName, MatchMode matchMode) : this(matchMode, new[] { parameterName })
        {
        }
    
        public RequireParameterAttribute(MatchMode mode = MatchMode.All, params string[] parameterNames)
        {
            ParameterNames = parameterNames;
            IncludeGET = true;
            IncludePOST = false;
            IncludeCookies = false;
            Mode = mode;
        }

        public string[] ParameterNames { get; private set; }
    
        /// <summary>
        /// Set it to TRUE to include GET (QueryStirng) parameters, FALSE to exclude them:
        /// default is TRUE.
        /// </summary>
        public bool IncludeGET { get; set; }
    
        /// <summary>
        /// Set it to TRUE to include POST (Form) parameters, FALSE to exclude them:
        /// default is TRUE.
        /// </summary>
        public bool IncludePOST { get; set; }
    
        /// <summary>
        /// Set it to TRUE to include parameters from Cookies, FALSE to exclude them:
        /// default is FALSE.
        /// </summary>
        public bool IncludeCookies { get; set; }
    
        /// <summary>
        /// Use MatchMode.All to invalidate the method unless all the given parameters are set (default).
        /// Use MatchMode.Any to invalidate the method unless any of the given parameters is set.
        /// Use MatchMode.None to invalidate the method unless none of the given parameters is set.
        /// </summary>
        public MatchMode Mode { get; set; }
    
    
        public override bool IsValidForRequest(RouteContext controllerContext, ActionDescriptor actionDescriptor)
        {
            switch (Mode)
            {
                case MatchMode.Any:
                    return (
                        (IncludeGET && ParameterNames.Any(p => controllerContext.HttpContext.Request.Query.Keys.Contains(p)))
                        || (IncludePOST && ParameterNames.Any(p => controllerContext.HttpContext.Request.Form.Keys.Contains(p)))
                        || (IncludeCookies && ParameterNames.Any(p => controllerContext.HttpContext.Request.Cookies.Keys.Contains(p)))
                        );
                case MatchMode.None:
                    return (
                        (!IncludeGET || !ParameterNames.Any(p => controllerContext.HttpContext.Request.Query.Keys.Contains(p)))
                        && (!IncludePOST || !ParameterNames.Any(p => controllerContext.HttpContext.Request.Form.Keys.Contains(p)))
                        && (!IncludeCookies || !ParameterNames.Any(p => controllerContext.HttpContext.Request.Cookies.Keys.Contains(p)))
                        );
                case MatchMode.All:
                default:
                    return (
                        (IncludeGET && ParameterNames.All(p => controllerContext.HttpContext.Request.Query.Keys.Contains(p)))
                        || (IncludePOST && ParameterNames.All(p => controllerContext.HttpContext.Request.Form.Keys.Contains(p)))
                        || (IncludeCookies && ParameterNames.All(p => controllerContext.HttpContext.Request.Cookies.Keys.Contains(p)))
                        );
            }
        }
    }
}

