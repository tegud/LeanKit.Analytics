(function () {
    var webPrefix = 'TELWEB';
    var serverRanges = {
        'web': [1, 19],
        'ssl': [107, 109]
    };
    var x;

    function buildServerName(machineNumber) {
        var machineNumberLength = (machineNumber + '').length;
        var id = machineNumber;

        for (var i = machineNumberLength; i < 3; i++) {
            id = '0' + id;
        }

        return webPrefix + id + 'P';
    }

    for (range in serverRanges) {
        if (!serverRanges.hasOwnProperty(range)) {
            continue;
        }

        for (x = serverRanges[range][0]; x <= serverRanges[range][1]; x++) {
            TLRGRP.BADGER.Machines.register(range, buildServerName(x));
        }
    }
})();