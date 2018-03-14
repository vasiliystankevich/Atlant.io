(function ($) {
    $.Site.Libraries.Common =
    {
        stringFormat: function(str, col) {
            col = typeof col === "object" ? col : Array.prototype.slice.call(arguments, 1);

            return str.replace(/\{\{|\}\}|\{(\w+)\}/g,
                function(m, n) {
                    if (m === "{{") {
                        return "{";
                    }
                    if (m === "}}") {
                        return "}";
                    }
                    return col[n];
                });
        },
        Preloader:
        {
            Show: function ()
            {
                $("#status").fadeIn();
                $("#preloader").delay(350).fadeIn("slow");
            },
            Hide: function() {
                $("#status").fadeOut(); 
                $("#preloader").delay(350).fadeOut("slow");                
            }
        }
    };
})(jQuery);