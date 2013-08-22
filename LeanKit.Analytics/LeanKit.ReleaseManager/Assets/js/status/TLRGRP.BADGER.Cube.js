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
        '15mins': { step: STEP.OneMinute, limit: 15 },
        '30mins': { step: STEP.OneMinute, limit: 30 },
        '1hour': { step: STEP.OneMinute, limit: 60 },
        '4hours': { step: STEP.FiveMinutes, limit: 48 },
        '24hours': { step: STEP.OneHour, limit: 24 }
    };

    TLRGRP.BADGER.Cube.convertTimePeriod = function (timePeriod) {
        var timePeriodMapping = timePeriodMappings[timePeriod];

        return 'step=' + timePeriodMapping.step + '&limit=' + timePeriodMapping.limit;
    };
})();