(function () {
    TLRGRP.namespace('TLRGRP.BADGER.Machines');

    TLRGRP.BADGER.Machines = (function () {
        var ranges = {};

        return {
            getAllServers: function() {
                var allServers = [];

                for (range in ranges) {
                    if (!ranges.hasOwnProperty(range)) {
                        continue;
                    }

                    allServers = allServers.concat(ranges[range]);
                }

                return allServers;
            },
            getServerRange: function(range) {
                return ranges[range];
            },
            register: function(range, machineName) {
                if (!ranges[range]) {
                    ranges[range] = [machineName];
                } else {
                    ranges[range][ranges[range].length] = machineName;
                }
            }
        };
    })();
})();