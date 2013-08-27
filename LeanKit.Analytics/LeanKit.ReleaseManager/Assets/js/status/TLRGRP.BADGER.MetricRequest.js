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
                if (!TLRGRP.BADGER.Utilities.getParameterByName('start') && !TLRGRP.BADGER.Utilities.getParameterByName('timePeriod')) {
                    return false;
                }

                return {
                    start: TLRGRP.BADGER.Utilities.getParameterByName('start'),
                    timePeriod: TLRGRP.BADGER.Utilities.getParameterByName('timePeriod')
                };
            }
        };
    };
})();