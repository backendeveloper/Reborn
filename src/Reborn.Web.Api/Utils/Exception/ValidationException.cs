using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Reborn.Web.Api.Utils.Exception
{

    public class ValidationException : System.Exception
    {
        private readonly string _message;

        public ValidationException() { }

        public ValidationException(string message, ModelStateDictionary modelState)
        {
            _message = message;
            _modelState = modelState;
        }

        public override string Message => _message ?? base.Message;


        private ModelStateDictionary _modelState;
        public ModelStateDictionary ModelState
        {
            get
            {
                return _modelState ?? new ModelStateDictionary();
            }
            set { _modelState = value; }
        }
    }
}
