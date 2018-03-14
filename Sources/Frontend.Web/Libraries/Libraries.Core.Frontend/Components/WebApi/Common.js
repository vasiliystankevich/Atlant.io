(function ($) {
    $.Site.Libraries.WebApi =
    {
        Common:
        {
            ExecuteAction: function (request) {
                var version = { Version: "1.0.0.0" };
                if (!!request.onBeforeAction) request.onBeforeAction();
                $.post(request.uri,
                    $.extend({}, version, request.data),
                    function(response) {
                        if (response.Status.Code === 200)
                            if (!!request.onSuccess) request.onSuccess(response);
                            else if (!!request.onException) request.onException(response);
                        if (!!request.onAfterAction) request.onAfterAction();
                    }).fail(function (exceptionFailResponse) {
                    if (!!request.onExceptionFail) request.onExceptionFail(exceptionFailResponse);
                    if (!!request.onAfterAction) request.onAfterAction();
                });
            }
        }
    };
})(jQuery);