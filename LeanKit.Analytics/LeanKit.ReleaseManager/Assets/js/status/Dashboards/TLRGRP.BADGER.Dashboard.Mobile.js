(function () {
    TLRGRP.namespace('TLRGRP.BADGER.Dashboard');

    var colors = ['steelblue', 'red', 'orange', 'green', 'purple'];

    TLRGRP.BADGER.Dashboard.Mobile = function () {
        var subMetrics = {
            'Summary': [
                { id: 'MobileTrafficSplit', slots: 2 },
                { id: 'MobileTrafficByPage', slots: 2 },
                { id: 'MobilePageResponseTime', slots: 2 },
                { id: 'MobileTrafficOnDesktopByPage', slots: 2 }
            ]
        };
        
        return new TLRGRP.BADGER.Dashboard.GraphSet({
            metric: 'Mobile',
            subMetrics: subMetrics
        });
    };
})();