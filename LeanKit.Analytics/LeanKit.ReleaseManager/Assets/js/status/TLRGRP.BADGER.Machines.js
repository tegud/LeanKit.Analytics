(function () {
    TLRGRP.namespace('TLRGRP.BADGER.Machines');

    TLRGRP.BADGER.Machines = (function () {
        var webPrefix = 'TELWEB';

        function buildServerName(machineNumber) {
            var machineNumberLength = (machineNumber + '').length;
            var id = machineNumber;

            for (var x = machineNumberLength; x < 3; x++) {
                id = '0' + id;
            }

            return webPrefix + id + 'P';
        }

        return {
            getFullNameForWebServer: function (machineNumber) {
                return buildServerName(machineNumber);
            }
        };
    })();
})();