(function () {
    TLRGRP.namespace('TLRGRP.BADGER');

    TLRGRP.BADGER.MetricsRequest = function () {
        var getParameterByName = TLRGRP.BADGER.Utilities.getParameterByName;
        var dashboard = getParameterByName('metric');
        var currentSubmetric = getParameterByName('subMetric');

        return {
            dashboard: function () {
                return dashboard;
            },
            subMetric: function () {
                return currentSubmetric;
            },
            timePeriod: function () {
                return TLRGRP.BADGER.Utilities.getParameterByName('timePeriod');
            }
        };
    };
})();