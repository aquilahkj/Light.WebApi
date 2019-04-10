using System;
using System.Collections.Generic;
using Light.WebApi.Core;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Microsoft.AspNetCore.Mvc
{
    public static class MvcExtensions
    {

        public static void AddInvalidModelStateException(this ApiBehaviorOptions options, InvalidModelStateSetting setting = null)
        {
            options.SuppressModelStateInvalidFilter = false;
            int code = 400;
            string message = SR.RequestModelError;
            bool showFieldName = true;
            bool showErrorDetail = false;
            if (setting != null) {
                if (setting.Code != null) {
                    code = setting.Code.Value;
                }
                if (!string.IsNullOrEmpty(setting.Message)) {
                    message = setting.Message;
                }
                if (setting.ShowErrorDetail != null) {
                    showFieldName = setting.ShowErrorDetail.Value;
                }
                if (setting.ShowErrorDetail != null) {
                    showErrorDetail = setting.ShowErrorDetail.Value;
                }
            }
            options.InvalidModelStateResponseFactory += (ActionContext arg) => {
                string msg;
                var state = arg.ModelState;
                if (showFieldName) {
                    string fields = string.Join(",", state.Keys);
                    msg = $"{message}:{fields}";
                }
                else {
                    msg = message;
                }
                var result = new ErrorResult(code, msg);
                if (showErrorDetail) {
                    var ie = state as IEnumerable<KeyValuePair<string, ModelStateEntry>>;
                    var errors = new ErrorData[state.ErrorCount];
                    int i = 0;
                    foreach (var item in ie) {
                        var count = item.Value.Errors.Count;
                        var errorMsgs = new string[count];
                        for (int j = 0; j < count; j++) {
                            errorMsgs[j] = item.Value.Errors[j].ErrorMessage;
                        }
                        var info = string.Join(';', errorMsgs);

                        var data = new ErrorData() {
                            Name = item.Key,
                            Info = info
                        };
                        errors[i] = data;
                        i++;
                    }
                    result.Errors = errors;
                }
                return new JsonResult(result);
            };
        }
    }
}
