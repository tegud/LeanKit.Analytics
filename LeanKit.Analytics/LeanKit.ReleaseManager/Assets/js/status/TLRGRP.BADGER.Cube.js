(function () {
    TLRGRP.namespace('TLRGRP.BADGER.Cube');

    var STEP = {
        TenSeconds: '1e4',
        OneMinute: '6e4',
        FiveMinutes: '3e5',
        OneHour: '36e5',
        OneDay: '864e5',
    };

    var timePeriodMappings = {
        '5mins': { step: STEP.TenSeconds, limit: 30 },
        '15mins': { step: STEP.OneMinute, limit: 15 },
        '30mins': { step: STEP.OneMinute, limit: 30 },
        '1hour': { step: STEP.OneMinute, limit: 60 },
        '4hours': { step: STEP.FiveMinutes, limit: 48 },
        '12hours': { step: STEP.OneHour, limit: 12 },
        '1day': { step: STEP.OneHour, limit: 24 }
    };

    TLRGRP.BADGER.Cube.convertTimePeriod = function (timePeriod) {
        var timePeriodMapping = timePeriodMappings[timePeriod];

        return 'step=' + timePeriodMapping.step + '&limit=' + timePeriodMapping.limit;
    };

    TLRGRP.BADGER.Cube.WMI = (function () {
        return {
            buildExpression: function (selectedView, machineName, stepAndLimit) {
                var metric = selectedView.metric;
                var metricGroup = selectedView.group;
                var eventType = selectedView.eventType;
                var divideBy = selectedView.divideBy;

                return ['median(' + eventType + '(' + metric + ')',
                    '.eq(source_host,"' + machineName + '")',
                    '.eq(metricGroup,"' + metricGroup + '"))' + (divideBy || '') + '&',
                    stepAndLimit].join('');
            }
        };
    })();
})();