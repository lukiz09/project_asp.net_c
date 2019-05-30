(function ($, undefinded) {

    "use strict";

    function getSettings(options) {

        var settings = $.extend({
            breakpoint: defaultGetBreakpoint,
            useDevicePixelRatio: true,
            refreshTimeout: null
        }, options);

        if (typeof settings.breakpoint !== "function") {
            var v = settings.breakpoint;
            settings.breakpoint = function () {
                return v;
            };
        }

        return settings;
    }

    function defaultGetBreakpoint() {

        if (window.screen.availWidth < 480) {
            return "phone";
        } else if (window.screen.availWidth < 800) {
            return "tablet";
        } else {
            return "desktop";
        }
    }

    function getImageUrl(element, settings) {

        var url = $(element).data(settings.breakpoint() + "-src");
        if (url) {
            if (settings.useDevicePixelRatio && window.devicePixelRatio) {
                if (url.indexOf("?") < 0) {
                    url += '?pr=' + escape(window.devicePixelRatio);
                } else {
                    url += '&pr=' + escape(window.devicePixelRatio);
                }
            }
        }

        return url;
    }

    function applyOnWindowResize(refreshTimeout, action) {
        if (refreshTimeout) {
            var refreshIntervalId;

            $(window).resize(function () {

                if (refreshIntervalId)
                    window.clearTimeout(refreshIntervalId);

                refreshIntervalId = setTimeout(function () {
                    action();
                    refreshIntervalId = null;
                }, refreshTimeout);
            });
        }

        return action();
    }

    $.fn.responsiveImage = function (options) {

        var settings = getSettings(options);

        return applyOnWindowResize(settings.refreshTimeout,
            function () {
                return this.each(function () {
                    this.src = getImageUrl(this, settings);
                    this.style.removeProperty("display");
                });
            } .bind(this));
    };

    $.fn.responsiveBackgroundImage = function (options) {

        var settings = getSettings(options);

        return applyOnWindowResize(settings.refreshTimeout,
            function () {
                return this.each(function () {
                    this.style.backgroundImage = "url('" + getImageUrl(this, settings) + "')";
                });
            } .bind(this));
    };

})(jQuery);